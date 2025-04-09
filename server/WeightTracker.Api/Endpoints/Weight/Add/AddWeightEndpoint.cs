namespace WeightTracker.Api.Endpoints.Weight.Add;

public class AddWeightEndpoint : Endpoint<AddWeightRequest, Results<NoContent, UnauthorizedHttpResult>>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Post("api/weight");

    public override async Task<Results<NoContent, UnauthorizedHttpResult>> HandleAsync(
        AddWeightRequest request, CancellationToken ct)
    {
        var userId = CurrentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
            return TypedResults.Unauthorized();

        var (weight, date) = request;
        var command = new AddWeightDataCommand(userId, GetDate(date), weight);
        await command.ExecuteAsync(ct);

        return TypedResults.NoContent();
    }

    private static DateOnly GetDate(string date)
    {
        return string.IsNullOrWhiteSpace(date)
            ? DateOnly.Parse(date)
            : DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
