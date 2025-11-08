using System.Globalization;
using WeightTracker.Api.Extensions;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Post;

internal sealed class WeightPostEndpoint : Endpoint<WeightPostRequest, IResult>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure()
    {
        Post("api/weight");

        Description(b => b
            .Produces(StatusCodes.Status200OK)
            .ProducesCommonProblems());
    }

    public override async Task<IResult> ExecuteAsync(WeightPostRequest request, CancellationToken ct)
    {
        var (weight, date) = request;
        var command = new AddWeightData(CurrentUser.Id, GetDate(date), weight);
        var result = await command.ExecuteAsync(ct);
        return result.Match(TypedResults.Ok, ErrorsService.HandleError);
    }

    private static DateOnly GetDate(string date)
    {
        return string.IsNullOrWhiteSpace(date)
            ? DateOnly.Parse(date, CultureInfo.InvariantCulture)
            : DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
