namespace WeightTracker.Api.Endpoints.Weight.Post;

public class WeightPostEndpoint : Endpoint<WeightPostRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Post("api/weight");

    public override async Task HandleAsync(WeightPostRequest request, CancellationToken ct)
    {
        var (weight, date) = request;
        var command = new AddWeightDataCommand(CurrentUser.Id, GetDate(date), weight);
        await command.ExecuteAsync(ct);
    }

    private static DateOnly GetDate(string date)
    {
        return string.IsNullOrWhiteSpace(date)
            ? DateOnly.Parse(date)
            : DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
