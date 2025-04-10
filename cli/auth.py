from typing import Any

import keyring
from msal import PublicClientApplication

from .config import get_msal_config

APP_NAME = 'weight_tracker'
MAX_TOKEN_LENGTH = 500


def acquire_token() -> dict[str, Any]:
    config = get_msal_config()

    client_id = config['client_id']
    tenant_id = config['tenant_id']
    authority = f'https://login.microsoftonline.com/{tenant_id}'

    app = PublicClientApplication(client_id, authority=authority)
    scopes = [f'api://{client_id}/access_as_user']

    response = app.acquire_token_interactive(scopes, timeout=60)

    return {
        'access_token': response['access_token'],
        'refresh_token': response['refresh_token']
    }


def store_tokens(access_token: str, refresh_token: str) -> None:
    delete_tokens()

    try:
        _store_token(access_token, 'access_token')
        _store_token(refresh_token, 'refresh_token')
    except Exception:
        pass


def get_tokens() -> dict[str, str]:
    try:
        return {
            'access_token': _get_token('access_token'),
            'refresh_token': _get_token('refresh_token')
        }
    except Exception:
        return {
            'access_token': None,
            'refresh_token': None
        }


def delete_tokens() -> None:
    try:
        _delete_token('access_token')
        _delete_token('refresh_token')
    except Exception:
        pass


def _store_token(token: str, name: str) -> None:
    if len(token) <= MAX_TOKEN_LENGTH:
        keyring.set_password(APP_NAME, name, token)
        keyring.set_password(APP_NAME, f'{name}_parts', '1')
        return

    splitted_value = [token[i:i+MAX_TOKEN_LENGTH] for i in range(0, len(token), MAX_TOKEN_LENGTH)]

    for i, chunk in enumerate(splitted_value, start=1):
        keyring.set_password(APP_NAME, f'{name}_{i}', chunk)

    keyring.set_password(APP_NAME, f'{name}_parts', str(len(splitted_value)))


def _get_token(name: str) -> str:
    token_parts = int(keyring.get_password(APP_NAME, f'{name}_parts'))

    if token_parts == 1:
        return keyring.get_password(APP_NAME, name)

    return ''.join([keyring.get_password(APP_NAME, f'{name}_{i}') for i in range(1, token_parts + 1)])


def _delete_token(name: str) -> None:
    token_parts = int(keyring.get_password(APP_NAME, f'{name}_parts'))

    if token_parts == 1:
        keyring.delete_password(APP_NAME, name)
        keyring.delete_password(APP_NAME, f'{name}_parts')
        return

    for i in range(1, token_parts + 1):
        keyring.delete_password(APP_NAME, f'{name}_{i}')

    keyring.delete_password(APP_NAME, f'{name}_parts')
