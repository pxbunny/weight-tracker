using WeightTracker.Api.Extensions;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Status.Get;

internal sealed class StatusGetEndpoint : EndpointWithoutRequest<IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Get("api/status");

        Description(b => b
            .Produces<StatusGetResponse>()
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var command = new GetStatus(CurrentUser.Id);
        var result = await command.ExecuteAsync(ct);
        return result.Match(d => TypedResults.Ok(d.ToResponse()), ErrorsService.HandleError);
    }
}
