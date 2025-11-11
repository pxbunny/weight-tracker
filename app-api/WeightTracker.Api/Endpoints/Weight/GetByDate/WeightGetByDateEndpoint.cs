using WeightTracker.Api.Cache;
using WeightTracker.Api.Extensions;

namespace WeightTracker.Api.Endpoints.Weight.GetByDate;

internal sealed class WeightGetByDateEndpoint : Endpoint<WeightGetByDateRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Get("api/weight/{Date}");
        Options(builder => builder.SetCustomCache());
        Description(b => b
            .Produces<WeightGetByDateResponse>()
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightGetByDateRequest request, CancellationToken ct)
    {
        if (CurrentUser.Id is null)
            return Results.Unauthorized();

        var command = request.ToCommand(CurrentUser.Id);
        var result = await command.ExecuteAsync(ct);

        return result.Match(d => TypedResults.Ok(d.ToResponse()), ErrorsService.HandleError);
    }
}
