using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Status.Get;

internal sealed class StatusGetEndpoint : EndpointWithoutRequest<StatusGetResponse>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/status");

    public override async Task HandleAsync(CancellationToken ct)
    {
        var command = new GetStatus(CurrentUser.Id);
        var data = await command.ExecuteAsync(ct);
        Response = data.ToResponse();
    }
}
