using System.Linq;

namespace WeightTracker.Api.Endpoints.Weight.Get;

public class GetWeightEndpoint : Endpoint<GetWeightRequest, Results<Ok<GetWeightResponse>, UnauthorizedHttpResult>>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/weight");

    public override async Task<Results<Ok<GetWeightResponse>, UnauthorizedHttpResult>> HandleAsync(
        GetWeightRequest request, CancellationToken ct)
    {
        var userId = CurrentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
            return TypedResults.Unauthorized();

        var (dateFromStr, dateToStr) = request.QueryParams;

        var dateFrom = string.IsNullOrWhiteSpace(dateFromStr)
            ? DateOnly.MinValue
            : DateOnly.Parse(dateFromStr);

        var dateTo = string.IsNullOrWhiteSpace(dateToStr)
            ? DateOnly.MaxValue
            : DateOnly.Parse(dateToStr);

        var query = new GetWeightDataQuery(userId, dateFrom, dateTo);
        var data = await query.ExecuteAsync(ct);

        var dto = new GetWeightResponse
        {
            UserId = data.UserId,
            AverageWeight = data.AverageWeight,
            MaxWeight = data.MaxWeight,
            MinWeight = data.MinWeight,
            Data = data.Data.Select(d => new WeightDataListItem(d.Date.ToFormattedString(), d.Weight))
        };

        return TypedResults.Ok(dto);
    }
}
