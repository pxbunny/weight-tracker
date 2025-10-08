from typing import Annotated

import typer
from rich.console import Console
from rich.style import Style
from rich.table import Table

from . import api_client as api
from . import auth
from .errors import AppError
from .visualizer import plot_data

WEIGHT_UNIT = 'kg'

typer.core.rich = None

app = typer.Typer(add_completion=False, no_args_is_help=True)

console = Console(width=120)
style = Style(color='bright_cyan', bold=True)


@app.command('login')
def login() -> None:
    with console.status('Signing in...', spinner='arc', spinner_style=style):
        auth.acquire_token()

    console.print('\nSigned in.\n')


@app.command('logout')
def logout() -> None:
    auth.logout()
    console.print('\nSigned out.\n')


@app.command('status')
def show_status() -> None:
    with console.status('Checking status...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        response = api.get_status(access_token)

    console.print()

    if response['addedForToday']:
        console.print('[bold]Weight data already added for today.[/]')
    else:
        console.print('[deep_pink2]Weight data not added for today.[/]')

    missed = response['missedInLast30Days']
    console.print(f'[bold bright_cyan]{missed}[/] entries missed in the last [bold bright_cyan]30 days[/].')
    console.print()


@app.command('add')
def add_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str | None, typer.Option('-d', '--date')] = None,
) -> None:
    try:
        with console.status('Adding data...', spinner='arc', spinner_style=style):
            access_token = auth.acquire_token()
            api.add_weight_data(date, weight, access_token)
    except AppError as e:
        console.print(e.message)
        return

    console.print('\nData added successfully.')

    with console.status('Fetching data...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        response = api.get_weight_data(date, date, access_token)

    weight_data = response['data']

    if not weight_data:
        console.print('There is a problem with your data.')
        console.print('Check the data and try again.')
        return

    max_value = response['max']
    min_value = response['min']
    avg_value = response['avg']

    console.print('\n=========================')
    console.print('Statistics for your data:')
    console.print('=========================\n')

    _print_date_range(weight_data)
    _print_weight_stats(max_value, min_value, avg_value)
    _print_current_weight(weight_data, avg_value)


@app.command('get')
def get_weight_data(
    date_from: Annotated[str | None, typer.Option('--date-from')] = None,
    date_to: Annotated[str | None, typer.Option('--date-to')] = None,
    tail: Annotated[int, typer.Option('--tail', help='Show only n last records in table')] = 10,
    plot: Annotated[bool, typer.Option('--plot')] = False,
) -> None:
    with console.status('Fetching data...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        response = api.get_weight_data(date_from, date_to, access_token)

    weight_data = response['data']

    if not weight_data:
        console.print('No data found.')
        return

    max_value = response['max']
    min_value = response['min']
    avg_value = response['avg']

    table = _create_weight_data_table(weight_data, tail)

    console.print()
    console.print(f'Weight unit: [bold bright_cyan]{WEIGHT_UNIT}[/]')
    console.print()
    console.print(table)

    console.print()
    console.print('Displayed:', min(len(weight_data), tail))
    console.print('Total received:', len(weight_data))
    console.print()

    _print_date_range(weight_data)

    _print_weight_stats(max_value, min_value, avg_value)
    _print_current_weight(weight_data, avg_value)

    if plot:
        plot_data(weight_data, avg_value)


@app.command('update')
def update_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str, typer.Option('-d', '--date')],
) -> None:
    with console.status('Updating data...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        api.update_weight_data(date, weight, access_token)

    console.print('\nData updated.\n')


@app.command('remove')
def remove_weight_data(
    date: Annotated[str, typer.Argument()],
) -> None:
    console.print(f'\nAre you sure you want to remove data for [bold bright_cyan]{date}[/]?', end='')

    if not typer.confirm(''):
        console.print('Operation cancelled.')
        return

    with console.status('Removing data...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        api.delete_weight_data(date, access_token)

    console.print('Data removed.\n')


def _create_weight_data_table(weight_data: list[dict], tail: int) -> Table:
    table = Table()

    table.add_column('Date')
    table.add_column('Weight', justify='right')
    table.add_column('+/-', justify='right')

    data_chunk = weight_data[-tail:]

    for index, item in enumerate(data_chunk):
        is_weight_higher = index > 0 and item['weight'] > data_chunk[index - 1]['weight']
        row_style = Style(bold=True) if is_weight_higher else None
        diff = item['weight'] - data_chunk[index - 1]['weight'] if index > 0 else 0
        table.add_row(item['date'], f'{item["weight"]:.2f}', f'{diff:.2f}', style=row_style)

    return table


def _print_weight_stats(max_value: float, min_value: float, avg_value: float) -> None:
    console.print(f'Max: [bold bright_cyan]{max_value:>6.2f} {WEIGHT_UNIT}[/]')
    console.print(f'Min: [bold bright_cyan]{min_value:>6.2f} {WEIGHT_UNIT}[/]')
    console.print(f'Avg: [bold bright_cyan]{avg_value:>6.2f} {WEIGHT_UNIT}[/]\n')


def _print_current_weight(weight_data: list[dict], avg_value: float) -> None:
    last_weight = weight_data[-1]['weight']

    is_lower_than_avg = last_weight < avg_value
    is_higher_than_avg = last_weight > avg_value

    comparison_str = '[bold bright_cyan]EQUAL[/] to'

    if is_lower_than_avg:
        comparison_str = '[bold]LOWER[/] then'
    elif is_higher_than_avg:
        comparison_str = '[bold]HIGHER[/] then'

    console.print(
        f'Current weight [bold bright_cyan]{last_weight:>.2f} {WEIGHT_UNIT}[/] is {comparison_str} average.\n'
    )


def _print_date_range(weight_data: list[dict]) -> None:
    min_date = weight_data[0]['date']
    max_date = weight_data[-1]['date']

    console.print(f'Date range: [bold bright_cyan]{min_date}[/] - [bold bright_cyan]{max_date}[/]\n')
