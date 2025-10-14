namespace WeightTracker.Api.Endpoints.Weight.Get;

public class WeightGetEndpoint : Endpoint<WeightGetRequest, WeightGetResponse>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/weight");

    public override async Task HandleAsync(WeightGetRequest request, CancellationToken ct)
    {
        var command = request.ToCommand(CurrentUser.Id);
        var data = await command.ExecuteAsync(ct);
        Response = data.ToResponse();
    }
}
