namespace WeightTracker.Api.Endpoints.Weight.Delete;

public class DeleteWeightEndpoint : Endpoint<DeleteWeightRequest, Results<NoContent, UnauthorizedHttpResult>>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Delete("api/weight/{Date}");

    public override async Task<Results<NoContent, UnauthorizedHttpResult>>
        HandleAsync(DeleteWeightRequest request, CancellationToken ct)
    {
        var userId = CurrentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
            return TypedResults.Unauthorized();

        var command = new RemoveWeightDataCommand(userId, DateOnly.Parse(request.Date));
        await command.ExecuteAsync(ct);

        return TypedResults.NoContent();
    }
}
