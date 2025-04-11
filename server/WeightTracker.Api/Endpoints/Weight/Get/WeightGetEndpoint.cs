namespace WeightTracker.Api.Endpoints.Weight.Get;

public class WeightGetEndpoint : Endpoint<WeightGetRequest, WeightGetResponse>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/weight");

    public override async Task HandleAsync(WeightGetRequest request, CancellationToken ct)
    {
        var query = request.ToQuery(CurrentUser.Id);
        var data = await query.ExecuteAsync(ct);
        Response = data.ToResponse();
    }
}
