import os
import sys

from msal import PublicClientApplication, SerializableTokenCache

from .config import get_auth_config


class PersistentTokenCache(SerializableTokenCache):
    def __init__(self):
        super().__init__()
        self.cache_file = self._get_cache_path()
        self.deserialize(self._load_cache())

    def persist_cache(self):
        os.makedirs(os.path.dirname(self.cache_file), exist_ok=True)

        with open(self.cache_file, 'wb') as f:
            f.write(self.serialize().encode('utf-8'))

        if not sys.platform.startswith('win'):
            os.chmod(self.cache_file, 0o600)

    def clear_cache(self):
        if os.path.exists(self.cache_file):
            os.remove(self.cache_file)

    def _get_cache_path(self):
        app_name = 'wtrack'
        home = os.path.expanduser('~')

        if sys.platform.startswith('win'):
            return os.path.join(home, 'AppData', 'Local', app_name, 'token_cache.bin')

        return os.path.join(home, f'.{app_name}_token_cache')

    def _load_cache(self):
        if not os.path.exists(self.cache_file):
            return ''

        with open(self.cache_file, 'rb') as f:
            return f.read().decode('utf-8')


def acquire_token() -> str:
    config = get_auth_config()
    cache = PersistentTokenCache()
    result = None

    client_id = config['client_id']
    tenant_id = config['tenant_id']
    authority = f'https://login.microsoftonline.com/{tenant_id}'

    app = PublicClientApplication(
        client_id,
        authority=authority,
        token_cache=cache
    )

    scopes = [f'api://{client_id}/access_as_user']
    accounts = app.get_accounts()

    if accounts:
        result = app.acquire_token_silent(scopes, account=accounts[0])
    else:
        result = app.acquire_token_interactive(scopes, timeout=60)

    cache.persist_cache()
    return result['access_token']


def logout() -> None:
    PersistentTokenCache().clear_cache()
