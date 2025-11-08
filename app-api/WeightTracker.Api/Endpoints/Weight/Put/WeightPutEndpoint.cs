using System.Globalization;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Put;

internal sealed class WeightPutEndpoint : Endpoint<WeightPutRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Put("api/weight/{Date}");

        Description(b => b
            .Produces(StatusCodes.Status200OK)
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightPutRequest request, CancellationToken ct)
    {
        var (date, weight) = request;
        var command = new UpdateWeightData(
            UserId: CurrentUser.Id,
            Date: DateOnly.Parse(date, CultureInfo.InvariantCulture),
            Weight: weight);
        var result = await command.ExecuteAsync(ct);
        return result.Match(TypedResults.Ok, ErrorsService.HandleError);
    }
}
