class AppError(Exception):
    """Base class for all application exceptions."""

    def __init__(self, message: str) -> None:
        self.message = message


class ConfigError(AppError):
    pass


class ApiError(AppError):
    pass
