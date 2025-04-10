import typer
from rich import print
from rich.table import Table
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


@app.command('get')
def get_weight_data(
    date_from: Annotated[str, typer.Option('--from')] = None,
    date_to: Annotated[str, typer.Option('--to')] = None
):
    access_token = _get_access_token()
    response = api.get_weight_data(date_from, date_to, access_token)

    table = Table()

    table.add_column('Date')
    table.add_column('Weight', justify='right')
    table.add_column('+/-', justify='right')

    weight_data = response['data']

    if not weight_data:
        print('No data found.')
        return

    for index, item in enumerate(weight_data):
        diff = item['weight'] - weight_data[index - 1]['weight'] if index > 0 else 0
        if diff == 0:
            diff = '-'
        else:
            diff = f'+{diff:.2f}' if diff > 0 else f'{diff:.2f}'
        table.add_row(item['date'], f'{item['weight']:.2f}', diff)

    print()
    print(table)

    print(f"\nMax: {response['max']:>6.2f}")
    print(f"Min: {response['min']:>6.2f}")
    print(f"Avg: {response['avg']:>6.2f}")

    min_date = weight_data[0]['date']
    max_date = weight_data[-1]['date']

    print(f"\nDate range: {min_date} - {max_date} | Count: {len(weight_data)}\n")


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
