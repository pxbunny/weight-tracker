import requests


def get_weight_data(access_token: str, date_from: str, date_to: str):
    url = f"http://localhost:5000/api/weight?dateFrom={date_from}&dateTo={date_to}"
    return _get(url)


def get_weight_forecast(access_token: str, date_from: str, date_to: str):
    url = f"http://localhost:5000/api/forecast?dateFrom={date_from}&dateTo={date_to}"
    return _get(url)


def add_weight_data(access_token: str, date: str, weight: float):
    url = f"http://localhost:5000/api/weight?date={date}&weight={weight}"
    return _post(url, {})

def update_weight_data(access_token: str, date: str, weight: float):
    url = f"http://localhost:5000/api/weight?date={date}&weight={weight}"
    return _put(url, {})


def delete_weight_data(access_token: str, date: str):
    url = f"http://localhost:5000/api/weight?date={date}"
    return _delete(url)


def ping_server(access_token: str):
    url = "http://localhost:5000/api/ping"
    return _get(url)


def _get(url: str):
    response = requests.get(url, timeout=5)
    response.raise_for_status()
    return response


def _post(url: str, data: dict):
    response = requests.post(url, json=data, timeout=5)
    response.raise_for_status()
    return response


def _put(url: str, data: dict):
    response = requests.put(url, json=data, timeout=5)
    response.raise_for_status()
    return response


def _delete(url: str):
    response = requests.delete(url, timeout=5)
    response.raise_for_status()
    return response
