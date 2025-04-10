namespace WeightTracker.Api.Endpoints.Weight.Delete;

public class WeightDeleteEndpoint : Endpoint<WeightDeleteRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Delete("api/weight/{Date}");

    public override async Task HandleAsync(WeightDeleteRequest request, CancellationToken ct)
    {
        var command = new RemoveWeightDataCommand(CurrentUser.Id, DateOnly.Parse(request.Date));
        await command.ExecuteAsync(ct);
    }
}
