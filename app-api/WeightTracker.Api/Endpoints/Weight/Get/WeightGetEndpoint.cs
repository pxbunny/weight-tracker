using WeightTracker.Api.Extensions;

namespace WeightTracker.Api.Endpoints.Weight.Get;

internal sealed class WeightGetEndpoint : Endpoint<WeightGetRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Get("api/weight");

        Description(b => b
            .Produces<WeightGetResponse>()
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightGetRequest request, CancellationToken ct)
    {
        var command = request.ToCommand(CurrentUser.Id);
        var result = await command.ExecuteAsync(ct);
        return result.Match(d => TypedResults.Ok(d.ToResponse()), ErrorsService.HandleError);
    }
}
