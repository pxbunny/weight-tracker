import typer
from rich.console import Console
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

console = Console(width=120)


@app.command('login')
def login():
    with console.status('Signing in...'):
        tokens = auth.acquire_token()
        auth.store_tokens(tokens['access_token'], None)

    console.print('Signed in.')


@app.command('logout')
def logout():
    auth.delete_tokens()
    console.print('Signed out.')


@app.command('add')
def add_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str, typer.Option('-d', '--date')] = None
):
    with console.status('Adding data...'):
        access_token = _get_access_token()
        api.add_weight_data(date, weight, access_token)

    console.print('Data added.')


@app.command('get')
def get_weight_data(
    date_from: Annotated[str, typer.Option('--from')] = None,
    date_to: Annotated[str, typer.Option('--to')] = None
):
    with console.status('Fetching data...'):
        access_token = _get_access_token()
        response = api.get_weight_data(date_from, date_to, access_token)

    table = Table()

    table.add_column('Date')
    table.add_column('Weight', justify='right')
    table.add_column('+/-', justify='right')

    weight_data = response['data']

    if not weight_data:
        console.print('No data found.')
        return

    for index, item in enumerate(weight_data):
        diff = item['weight'] - weight_data[index - 1]['weight'] if index > 0 else 0
        diff = f'+{diff:.2f}' if diff > 0 else f'[bold]{diff:.2f}[/]'
        table.add_row(item['date'], f'{item['weight']:.2f}', diff)

    console.print()
    console.print(table)

    console.print(f"\nMax: [bright_cyan]{response['max']:>6.2f}[/]")
    console.print(f"Min: [bright_cyan]{response['min']:>6.2f}[/]")
    console.print(f"Avg: [bright_cyan]{response['avg']:>6.2f}[/]")

    min_date = weight_data[0]['date']
    max_date = weight_data[-1]['date']

    console.print(
        f"\nDate range: [bright_cyan]{min_date}[/] - [bright_cyan]{max_date}[/]" +
        f" | Count: [bright_cyan]{len(weight_data)}\n[/]"
    )


@app.command('update')
def update_weight_data(
    date: Annotated[str, typer.Argument()],
    weight: Annotated[float, typer.Argument()]
):
    with console.status('Updating data...'):
        access_token = _get_access_token()
        api.update_weight_data(date, weight, access_token)

    console.print('Data updated.')


@app.command('remove')
def remove_weight_data(
    date: Annotated[str, typer.Argument()]
):
    with console.status('Removing data...'):
        access_token = _get_access_token()
        api.delete_weight_data(date, access_token)

    console.print('Data removed.')


@app.command('forecast')
def get_weight_forecast():
    console.print('Not implemented yet.')


@app.command('show')
def show_weight_chart():
    console.print('Not implemented yet.')


@app.command('ping')
def ping_server():
    console.print('Not implemented yet.')


def _get_access_token() -> str:
    return auth.get_tokens()['access_token']
