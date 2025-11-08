using WeightTracker.Api.Extensions;

namespace WeightTracker.Api.Endpoints.Weight.GetByDate;

internal sealed class WeightGetByDateEndpoint : Endpoint<WeightGetByDateRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Get("api/weight/{Date}");

        Description(b => b
            .Produces<WeightGetByDateResponse>()
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightGetByDateRequest request, CancellationToken ct)
    {
        var command = request.ToCommand(CurrentUser.Id);
        var result = await command.ExecuteAsync(ct);
        return result.Match(d => TypedResults.Ok(d.ToResponse()), ErrorsService.HandleError);
    }
}
