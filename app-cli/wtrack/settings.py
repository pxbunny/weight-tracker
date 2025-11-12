from dataclasses import dataclass, field, fields

import yaml

CONFIG_FILENAME = 'config.yaml'


@dataclass
class ApiConfig:
    base_url: str = ''


@dataclass
class AuthConfig:
    client_id: str = ''
    tenant_id: str = ''


@dataclass
class Config:
    api: ApiConfig = field(default_factory=ApiConfig)
    auth: AuthConfig = field(default_factory=AuthConfig)


def load_config(path: str = CONFIG_FILENAME) -> Config:
    with open(path, encoding='utf-8') as f:
        d = yaml.safe_load(f)

    c = Config()

    for f in fields(Config):
        if f.name not in d:
            continue
        setattr(c, f.name, f.type(**d[f.name]))

    return c


config = load_config()
