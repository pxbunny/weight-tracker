namespace WeightTracker.Api.Endpoints.Weight.Put;

public class UpdateWeightEndpoint : Endpoint<UpdateWeightRequest, Results<NoContent, UnauthorizedHttpResult>>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Put("api/weight/{Date}");

    public override async Task<Results<NoContent, UnauthorizedHttpResult>>
        HandleAsync(UpdateWeightRequest request, CancellationToken ct)
    {
        var userId = CurrentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
            return TypedResults.Unauthorized();

        var command = new UpdateWeightDataCommand(userId, DateOnly.Parse(request.Date), request.Weight);
        await command.ExecuteAsync(ct);

        return TypedResults.NoContent();
    }
}
