from datetime import datetime

import requests
from requests import Response

from .config import get_server_config
from .errors import ApiError, ConfigError


def get_status(access_token: str) -> dict:
    return _send_request('GET', 'api/status', access_token=access_token).json()


def get_weight_data(date_from: str, date_to: str, access_token: str) -> dict:
    params = {'dateFrom': date_from, 'dateTo': date_to}
    return _send_request('GET', 'api/weight', params=params, access_token=access_token).json()


def get_weight_forecast(date_from: str, date_to: str, access_token: str) -> dict:
    params = {'dateFrom': date_from, 'dateTo': date_to}
    return _send_request('GET', 'api/forecast', params=params, access_token=access_token).json()


def add_weight_data(date: str | None, weight: float, access_token: str) -> None:
    if date is None:
        date = datetime.now().strftime('%Y-%m-%d')

    data = {'date': date, 'weight': weight}
    _send_request('POST', 'api/weight', data=data, access_token=access_token)


def update_weight_data(date: str, weight: float, access_token: str) -> None:
    data = {'weight': weight}
    _send_request('PUT', f'api/weight/{date}', data=data, access_token=access_token)


def delete_weight_data(date: str, access_token: str) -> None:
    _send_request('DELETE', f'api/weight/{date}', access_token=access_token)


def ping_server(access_token: str) -> Response:
    return _send_request('GET', 'api/ping', access_token=access_token)


def _send_request(
    method: str, url: str, *, data: dict = None, params: dict = None, access_token: str = None
) -> Response:
    """Sends an HTTP request with Bearer authorization and base URL handling.

    This internal helper function handles HTTP requests by automatically:
    - Adding Authorization header when access token is provided
    - Resolving relative URLs against the base URL from server configuration
    - Serializing JSON data payloads
    - Validating HTTP response status codes

    Args:
        method: HTTP method to use (e.g. 'GET', 'POST', 'PUT', 'DELETE')
        url: API endpoint URL. If not absolute, will be combined with configured base URL
        data: Optional JSON-serializable data for request body (default: None)
        params: Optional query parameters to append to URL (default: None)
        access_token: Optional access token for Bearer authentication (default: None)
        timeout: Request timeout in seconds (default: 15)

    Returns:
        Response: Validated requests.Response object after HTTP status check

    Raises:
        ConfigError: When missing 'base_url' in server configuration
        ApiError: When request fails (network or server errors)
    """

    config = get_server_config()

    auth_header = {'Authorization': f'Bearer {access_token}'} if access_token else None

    try:
        base_url = config['base_url']
    except KeyError as e:
        raise ConfigError("Missing 'base_url' in server configuration") from e

    is_full_url = url.startswith('http')
    timeout = 15

    url = f'{base_url}/{url}' if not is_full_url else url

    try:
        response = requests.request(method, url, json=data, params=params, headers=auth_header, timeout=timeout)
        response.raise_for_status()

    except requests.exceptions.HTTPError as e:
        raise ApiError(f'Server responded with an error: {str(e)}') from e

    except requests.exceptions.RequestException as e:
        raise ApiError('Could not reach the server') from e

    return response
