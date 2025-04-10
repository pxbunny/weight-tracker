import typer
from typing_extensions import Annotated

from . import api_client as api
from . import auth

typer.core.rich = None

app = typer.Typer(
    add_completion=False,
    no_args_is_help=True,
    epilog="You're not fat, you're fluffy."
)


@app.command('login')
def login():
    tokens = auth.acquire_token()
    auth.store_tokens(tokens['access_token'], None)


@app.command('logout')
def logout():
    auth.delete_tokens()


@app.command('add')
def add_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str, typer.Option('-d', '--date')] = None
):
    access_token = _get_access_token()
    api.add_weight_data(date, weight, access_token)


@app.command('list')
def list_weight_data(
    date_from: Annotated[str, typer.Option('--from')] = None,
    date_to: Annotated[str, typer.Option('--to')] = None
):
    access_token = _get_access_token()
    response = api.get_weight_data(date_from, date_to, access_token)

    for item in response['data']:
        print(item['date'], item['weight'])

    print(f"Max: {response['max']}")
    print(f"Min: {response['min']}")
    print(f"Avg: {response['avg']}")


@app.command('update')
def update_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str, typer.Option('-d', '--date')]
):
    access_token = _get_access_token()
    api.update_weight_data(date, weight, access_token)


@app.command('remove')
def remove_weight_data(
    date: Annotated[str, typer.Argument()]
):
    access_token = _get_access_token()
    api.delete_weight_data(date, access_token)


@app.command('forecast')
def get_weight_forecast():
    pass


@app.command('show')
def show_weight_chart():
    pass


@app.command('ping')
def ping_server():
    pass


def _get_access_token() -> str:
    return auth.get_tokens()['access_token']
