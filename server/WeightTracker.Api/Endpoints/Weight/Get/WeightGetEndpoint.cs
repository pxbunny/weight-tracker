using System.Linq;

namespace WeightTracker.Api.Endpoints.Weight.Get;

public class WeightGetEndpoint : Endpoint<WeightGetRequest, WeightGetResponse>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/weight");

    public override async Task HandleAsync(WeightGetRequest request, CancellationToken ct)
    {
        var (dateFromStr, dateToStr) = request;

        var dateFrom = string.IsNullOrWhiteSpace(dateFromStr)
            ? DateOnly.MinValue
            : DateOnly.Parse(dateFromStr);

        var dateTo = string.IsNullOrWhiteSpace(dateToStr)
            ? DateOnly.MaxValue
            : DateOnly.Parse(dateToStr);

        var userId = CurrentUser.Id;

        var query = new GetWeightDataQuery(userId, dateFrom, dateTo);
        var data = await query.ExecuteAsync(ct);

        Response = new WeightGetResponse
        {
            UserId = data.UserId,
            Avg = data.AverageWeight,
            Max = data.MaxWeight,
            Min = data.MinWeight,
            Data = data.Data.Select(d => new WeightDataListItem(d.Date.ToFormattedString(), d.Weight))
        };
    }
}
