class AppException(Exception):
    """Base class for all application exceptions."""

    def __init__(self, message: str) -> None:
        self.message = message


class ConfigError(AppException):
    pass


class ApiError(AppException):
    pass
