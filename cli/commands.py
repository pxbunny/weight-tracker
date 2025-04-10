from typer import Typer, core

from .auth import acquire_token, delete_tokens, get_tokens, store_tokens

core.rich = None

app = Typer(
    add_completion=False,
    no_args_is_help=True,
    epilog="You're not fat, you're fluffy."
)


@app.command('login')
def login():
    tokens = acquire_token()
    store_tokens(tokens['access_token'], tokens['refresh_token'])


@app.command('logout')
def logout():
    delete_tokens()


@app.command('show-tokens')
def status():
    tokens = get_tokens()
    print(tokens['access_token'])
    print(tokens['refresh_token'])


@app.command('add')
def add_weight_data():
    print('add')


@app.command('list')
def list_weight_data():
    print('list')


@app.command('update')
def update_weight_data():
    print('update')


@app.command('remove')
def remove_weight_data():
    print('remove')


@app.command('forecast')
def get_weight_forecast():
    print('forecast')


@app.command('show')
def show_weight_chart():
    print('show')


@app.command('ping')
def ping_server():
    print('pong')
