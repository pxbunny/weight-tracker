import os
import sys

from msal import PublicClientApplication, SerializableTokenCache

from .errors import AppError
from .settings import config


class _PersistentTokenCache(SerializableTokenCache):
    def __init__(self) -> None:
        super().__init__()
        self.cache_file = self._get_cache_path()
        self.deserialize(self._load_cache())

    def persist_cache(self) -> None:
        os.makedirs(os.path.dirname(self.cache_file), exist_ok=True)

        with open(self.cache_file, 'wb') as f:
            f.write(self.serialize().encode('utf-8'))

        if not sys.platform.startswith('win'):
            os.chmod(self.cache_file, 0o600)

    def clear_cache(self) -> None:
        if os.path.exists(self.cache_file):
            os.remove(self.cache_file)

    def _get_cache_path(self) -> str:
        app_name = 'wtrack'
        home = os.path.expanduser('~')

        if sys.platform.startswith('win'):
            return os.path.join(home, 'AppData', 'Local', app_name, 'token_cache.bin')

        return os.path.join(home, f'.{app_name}_token_cache')

    def _load_cache(self) -> str:
        if not os.path.exists(self.cache_file):
            return ''

        with open(self.cache_file, 'rb') as f:
            return f.read().decode('utf-8')


def acquire_token() -> str:
    cache = _PersistentTokenCache()
    result = None

    client_id = config.auth.client_id
    tenant_id = config.auth.tenant_id
    authority = f'https://login.microsoftonline.com/{tenant_id}'

    app = PublicClientApplication(client_id, authority=authority, token_cache=cache)

    scopes = [f'api://{client_id}/access_as_user']
    accounts = app.get_accounts()

    if accounts:
        result = app.acquire_token_silent(scopes, account=accounts[0])
    else:
        result = app.acquire_token_interactive(scopes, timeout=60)

    if not result or 'access_token' not in result:
        error_description = 'Unknown authentication error.'

        if isinstance(result, dict):
            error_description = result.get('error_description') or result.get('error') or error_description

        raise AppError(f'Authentication failed: {error_description}')

    cache.persist_cache()
    return result['access_token']


def logout() -> None:
    _PersistentTokenCache().clear_cache()
