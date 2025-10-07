import plotly.graph_objects as go

BACKGROUND_COLOR = '#111111'
DEFAULT_CONFIG = {'displaylogo': False}
JAVASCRIPT = f'''document.body.style.backgroundColor = "{BACKGROUND_COLOR}"; document.title = "Weight Tracker";'''
TEMPLATE = 'plotly_dark'


def plot_data(data: list, avg: float) -> None:
    fig = go.Figure()

    fig.update_layout(
        title={'text': '<b>Weight Tracker - data visualization</b>', 'x': 0.5, 'xanchor': 'center', 'font_size': 24},
        hovermode='x unified',
        xaxis_title='Date',
        yaxis_title='Weight (kg)',
        template=TEMPLATE,
        paper_bgcolor=BACKGROUND_COLOR,
    )

    weights = [item['weight'] for item in data]
    dates = [item['date'] for item in data]
    length = len(dates)

    fig.add_trace(go.Scatter(x=dates, y=weights, name='Weight', line_color='cyan'))
    fig.add_trace(go.Scatter(x=dates, y=[avg] * length, name='Avg', line_color='deeppink'))

    fig.show(config=DEFAULT_CONFIG, post_script=[JAVASCRIPT])
