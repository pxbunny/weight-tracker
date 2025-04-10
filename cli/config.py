import json
import os.path
import sys

CONFIG_FILENAME = 'config.json'


def get_server_config() -> dict:
    return config['server']


def get_auth_config() -> dict:
    return config['auth']


def _load_config(filename: str = CONFIG_FILENAME) -> dict:
    if os.path.exists(filename):
        with open(filename, encoding='utf-8') as f:
            return json.load(f)

    file_path = os.path.abspath(os.path.dirname(sys.executable))
    file_path = os.path.join(file_path, filename)

    with open(file_path, encoding='utf-8') as f:
        return json.load(f)


config = _load_config()
