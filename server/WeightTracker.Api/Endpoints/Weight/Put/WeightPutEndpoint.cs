namespace WeightTracker.Api.Endpoints.Weight.Put;

public class WeightPutEndpoint : Endpoint<WeightPutRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Put("api/weight/{Date}");

    public override async Task HandleAsync(WeightPutRequest request, CancellationToken ct)
    {
        var (date, weight) = request;
        var command = new UpdateWeightDataCommand(
            UserId: CurrentUser.Id,
            Date: DateOnly.Parse(date),
            Weight: weight);
        await command.ExecuteAsync(ct);
    }
}
