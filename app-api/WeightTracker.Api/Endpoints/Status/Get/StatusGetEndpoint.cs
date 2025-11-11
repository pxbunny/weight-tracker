using WeightTracker.Api.Cache;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Status.Get;

internal sealed class StatusGetEndpoint : EndpointWithoutRequest<IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Get("api/status");
        Options(builder => builder.SetCustomCache());
        Description(builder => builder
            .Produces<StatusGetResponse>()
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        if (CurrentUser.Id is null)
            return Results.Unauthorized();

        var command = new GetStatus(CurrentUser.Id);
        var result = await command.ExecuteAsync(ct);

        return result.Match(d => TypedResults.Ok(d.ToResponse()), ErrorsService.HandleError);
    }
}
