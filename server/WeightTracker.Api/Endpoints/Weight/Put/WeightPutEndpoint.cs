namespace WeightTracker.Api.Endpoints.Weight.Put;

public class WeightPutEndpoint : Endpoint<WeightPutRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Put("api/weight/{Date}");

    public override async Task HandleAsync(WeightPutRequest request, CancellationToken ct)
    {
        var (date, weight) = request;
        var command = new UpdateWeightDataCommand(CurrentUser.Id, DateOnly.Parse(date), weight);
        await command.ExecuteAsync(ct);
    }
}
