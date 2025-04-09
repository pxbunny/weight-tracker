from typing import Any

from msal import PublicClientApplication

from .config import get_msal_config


def acquire_token() -> dict[str, Any]:
    config = get_msal_config()
    client_id = config['client_id']
    tenant_id = config['tenant_id']
    authority = f'https://login.microsoftonline.com/{tenant_id}'

    app = PublicClientApplication(client_id, authority=authority)
    scopes = [f'api://{client_id}/access_as_user']

    return app.acquire_token_interactive(scopes, timeout=60)
