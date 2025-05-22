from datetime import datetime

import requests

from .config import get_server_config


def get_status(access_token: str):
    return _send_request('GET', 'api/status', access_token=access_token).json()


def get_weight_data(date_from: str, date_to: str, access_token: str) -> dict:
    params = {'dateFrom': date_from, 'dateTo': date_to}
    return _send_request('GET', 'api/weight', params=params, access_token=access_token).json()


def get_weight_forecast(date_from: str, date_to: str, access_token: str):
    params = {'dateFrom': date_from, 'dateTo': date_to}
    return _send_request('GET', 'api/forecast', params=params, access_token=access_token)


def add_weight_data(date: str | None, weight: float, access_token: str):
    if date is None:
        date = datetime.now().strftime('%Y-%m-%d')

    data = {'date': date, 'weight': weight}
    _send_request('POST', 'api/weight', data=data, access_token=access_token)


def update_weight_data(date: str, weight: float, access_token: str):
    data = {'weight': weight}
    _send_request('PUT', f'api/weight/{date}', data=data, access_token=access_token)


def delete_weight_data(date: str, access_token: str):
    _send_request('DELETE', f'api/weight/{date}', access_token=access_token)


def ping_server(access_token: str):
    return _send_request('GET', 'api/ping', access_token=access_token)


def _send_request(method: str, url: str, *, data: dict = None, params: dict = None, access_token: str = None):
    config = get_server_config()

    auth_header = f'Bearer {access_token}' if access_token else None
    auth_header = {'Authorization': auth_header} if auth_header else None

    url = f'{config['base_url']}/{url}' if not url.startswith('http') else url

    response = requests.request(method, url, json=data, params=params, headers=auth_header, timeout=99999)
    response.raise_for_status()

    return response
