import os.path
import sys

import yaml

CONFIG_FILENAME = 'config.yaml'


def get_api_config() -> dict:
    return config['api']


def get_auth_config() -> dict:
    return config['auth']


def _load_config(filename: str = CONFIG_FILENAME) -> dict:
    if os.path.exists(filename):
        with open(filename, encoding='utf-8') as f:
            return yaml.safe_load(f)

    file_path = os.path.abspath(os.path.dirname(sys.executable))
    file_path = os.path.join(file_path, filename)

    with open(file_path, encoding='utf-8') as f:
        return yaml.safe_load(f)


config = _load_config()
