from typing import Annotated

import typer
from rich.console import Console
from rich.style import Style
from rich.table import Table
from rich.text import Text

from . import api_client as api
from . import auth
from .errors import AppError
from .visualizer import plot_data

WEIGHT_UNIT = 'kg'
APP_NAME = 'wtrack'

app = typer.Typer(name=APP_NAME, add_completion=False, no_args_is_help=True)

console = Console(width=120)
style = Style(color='bright_cyan', bold=True)


@app.command('login', help='aliases: signin')
@app.command('signin', hidden=True)
def login() -> None:
    try:
        with console.status('Signing in...', spinner='arc', spinner_style=style):
            auth.acquire_token()
    except AppError as e:
        console.print(e.message)
        return

    console.print('\nSigned in.\n')


@app.command('logout', help='aliases: signout')
@app.command('signout', hidden=True)
def logout() -> None:
    auth.logout()
    console.print('\nSigned out.\n')


@app.command('status', help='aliases: streak')
@app.command('streak', hidden=True)
def show_status() -> None:
    adherence_windows = [30]

    try:
        with console.status('Checking status...', spinner='arc', spinner_style=style):
            access_token = auth.acquire_token()
            response = api.get_status(access_token)
    except AppError as e:
        console.print(e.message)
        return

    console.print()

    if response['today']['hasEntry']:
        current_weight = f'{response["today"]["weight"]} {WEIGHT_UNIT}'
        console.print(f'[[bold bright_cyan]✓[/]] Data added: [bold bright_cyan]{current_weight}[/]')
    else:
        console.print('[[bold deep_pink2]✗[/]] No entry yet today.')

    console.print()

    current_streak = f'{response["streak"]["current"]} {"day" if response["streak"]["current"] == 1 else "days"}'
    longest_streak = f'{response["streak"]["longest"]} {"day" if response["streak"]["longest"] == 1 else "days"}'

    console.print(
        f'[bold]Streak:[/] [bold bright_cyan]{current_streak}[/] (best: [bold bright_cyan]{longest_streak}[/])'
    )

    for adherence in response['adherence']:
        window = adherence['window']

        if window not in adherence_windows:
            continue

        days_missed = adherence['daysMissed']
        console.print(f'[bold]Adherence ({window}d):[/] [bold bright_cyan]{days_missed}[/] missed')

    console.print()


@app.command('add', help='aliases: new, insert')
@app.command('new', hidden=True)
@app.command('insert', hidden=True)
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

    console.print('\nData added successfully.\n')


@app.command('report', help='aliases: show, get, list, ls, display')
@app.command('show', hidden=True)
@app.command('get', hidden=True)
@app.command('list', hidden=True)
@app.command('ls', hidden=True)
@app.command('display', hidden=True)
def show_report(
    date: Annotated[str, typer.Argument()] = None,
    date_from: Annotated[str | None, typer.Option('--date-from')] = None,
    date_to: Annotated[str | None, typer.Option('--date-to')] = None,
    tail: Annotated[int, typer.Option('--tail', help='Show only n last records in table')] = 7,
    plot: Annotated[bool, typer.Option('--plot')] = False,
) -> None:
    try:
        if date:
            _handle_report_for_specific_day(date)
            return

        with console.status('Fetching data...', spinner='arc', spinner_style=style):
            access_token = auth.acquire_token()
            response = api.get_weight_data(date_from, date_to, access_token)
    except AppError as e:
        console.print(e.message)
        return

    weight_data = response['data']
    today = response['today']
    stats = response['stats']

    avg_value = stats['avg']

    if not weight_data:
        console.print('No data found.')
        return

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
    _print_weight_stats(stats)

    if today['hasEntry']:
        _print_current_weight(today['weight'], avg_value)

    if not plot:
        return

    with console.status('Plotting data...', spinner='arc', spinner_style=style):
        plot_data(weight_data, avg_value)


@app.command('update', help='aliases: edit')
@app.command('edit', hidden=True)
def update_weight_data(
    weight: Annotated[float, typer.Argument()],
    date: Annotated[str, typer.Option('-d', '--date')],
) -> None:
    try:
        with console.status('Updating data...', spinner='arc', spinner_style=style):
            access_token = auth.acquire_token()
            api.update_weight_data(date, weight, access_token)
    except AppError as e:
        console.print(e.message)
        return

    console.print('\nData updated.\n')


@app.command('remove', help='aliases: rm, delete')
@app.command('rm', hidden=True)
@app.command('delete', hidden=True)
def remove_weight_data(
    date: Annotated[str, typer.Argument()],
) -> None:
    console.print(f'\nAre you sure you want to remove data for [bold bright_cyan]{date}[/]?', end='')

    if not typer.confirm(''):
        console.print('Operation cancelled.')
        return

    try:
        with console.status('Removing data...', spinner='arc', spinner_style=style):
            access_token = auth.acquire_token()
            api.delete_weight_data(date, access_token)
    except AppError as e:
        console.print(e.message)
        return

    console.print('Data removed.\n')


def _handle_report_for_specific_day(date: str) -> None:
    with console.status('Fetching data...', spinner='arc', spinner_style=style):
        access_token = auth.acquire_token()
        response = api.get_weight_data_by_date(date, access_token)

    console.print(f'\nDate: [bold bright_cyan]{date}[/]')
    console.print(f'Weight: [bold bright_cyan]{response["weight"]} {WEIGHT_UNIT}[/]\n')


def _create_weight_data_table(weight_data: list[dict], tail: int) -> Table:
    table = Table()

    table.add_column(Text('Date', justify='center'))
    table.add_column('Weight', justify='right')
    table.add_column('±', justify='right')

    data_chunk = weight_data[-(tail + 1) :]
    is_tail_shorter_than_data = tail < len(weight_data)

    for index, item in enumerate(data_chunk):
        is_weight_higher = index > 0 and item['weight'] > data_chunk[index - 1]['weight']
        row_style = Style(bold=True) if is_weight_higher else None

        diff = item['weight'] - data_chunk[index - 1]['weight'] if index > 0 else 0
        diff = f'+{diff:.2f}' if is_weight_higher else f'{diff:.2f}'

        if index > 0 or not is_tail_shorter_than_data:
            table.add_row(item['date'], f'{item["weight"]:.2f}', diff, style=row_style)

    return table


def _print_weight_stats(stats: dict) -> None:
    console.print(f'Max: [bold bright_cyan]{stats["max"]:>6.2f} {WEIGHT_UNIT}[/]')
    console.print(f'Min: [bold bright_cyan]{stats["min"]:>6.2f} {WEIGHT_UNIT}[/]')
    console.print(f'Avg: [bold bright_cyan]{stats["avg"]:>6.2f} {WEIGHT_UNIT}[/]\n')


def _print_current_weight(weight: float, avg_value: float) -> None:
    is_lower_than_avg = weight < avg_value
    is_higher_than_avg = weight > avg_value

    comparison_str = '[bold bright_cyan]EQUAL[/] to'

    if is_lower_than_avg:
        comparison_str = '[bold]LOWER[/] then'
    elif is_higher_than_avg:
        comparison_str = '[bold]HIGHER[/] then'

    console.print(f'Current weight [bold bright_cyan]{weight:>.2f} {WEIGHT_UNIT}[/] is {comparison_str} average.\n')


def _print_date_range(weight_data: list[dict]) -> None:
    min_date = weight_data[0]['date']
    max_date = weight_data[-1]['date']

    console.print(f'Date range: [bold bright_cyan]{min_date}[/] - [bold bright_cyan]{max_date}[/]\n')
