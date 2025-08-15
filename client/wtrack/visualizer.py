from typing import Iterator

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
    length = len(dates)

    window_size = 5
    sma_weight = list(_calculate_sma(weights, window_size))

    fig.add_trace(go.Scatter(
        x=dates,
        y=weights,
        name='Weight'
    ))

    fig.add_trace(go.Scatter(
        x=dates[window_size - 1:],
        y=sma_weight,
        name=f'Weight (SMA{window_size})',
        line={'shape': 'spline',  'smoothing': 0.5}
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[max] * length,
        name='Max',
        visible='legendonly'
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[min] * length,
        name='Min',
        visible='legendonly'
    ))

    fig.add_trace(go.Scatter(
        x=dates,
        y=[avg] * length,
        name='Avg',
        visible='legendonly'
    ))

    fig.show(config=DEFAULT_CONFIG, post_script=[JAVASCRIPT])


def _calculate_sma(data: list, window_size) -> Iterator[float]:
    i = 0
    while i < len(data) - window_size + 1:
        window = data[i : i + window_size]
        window_average = round(sum(window) / window_size, 2)
        i += 1
        yield window_average
