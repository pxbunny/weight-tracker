using System.Globalization;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Delete;

internal sealed class WeightDeleteEndpoint : Endpoint<WeightDeleteRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Delete("api/weight/{Date}");

        Description(b => b
            .Produces(StatusCodes.Status200OK)
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightDeleteRequest request, CancellationToken ct)
    {
        var command = new RemoveWeightData(
            UserId: CurrentUser.Id,
            Date: DateOnly.Parse(request.Date, CultureInfo.InvariantCulture));
        var result = await command.ExecuteAsync(ct);
        return result.Match(TypedResults.Ok, ErrorsService.HandleError);
    }
}
