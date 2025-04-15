import plotly.graph_objects as go

BACKGROUND_COLOR = '#111111'
DEFAULT_CONFIG = {'displaylogo': False}
JAVASCRIPT = f'''document.body.style.backgroundColor = "{BACKGROUND_COLOR}";'''
TEMPLATE = 'plotly_dark'

def plot_data(data: list, max: float, min: float, avg: float) -> None:
    fig = go.Figure()

    fig.update_layout(
        hovermode='x unified',
        title='Weight Data',
        xaxis_title='Date',
        yaxis_title='Weight (kg)',
        template=TEMPLATE,
        paper_bgcolor=BACKGROUND_COLOR
    )

    weights = [item['weight'] for item in data]
    dates = [item['date'] for item in data]

    fig.add_trace(go.Scatter(
        x=dates,
        y=weights,
        name='Weight'
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[max] * len(dates),
        name='Max',
        visible='legendonly'
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[min] * len(dates),
        name='Min',
        visible='legendonly'
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[avg] * len(dates),
        name='Avg',
        visible='legendonly'
    ))

    fig.show(config=DEFAULT_CONFIG, post_script=[JAVASCRIPT])
