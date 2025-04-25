using System.Linq;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Get;

public static class WeightGetMappings
{
    public static GetWeightData ToCommand(this WeightGetRequest request, string userId)
    {
        var (dateFromStr, dateToStr) = request;

        var dateFrom = string.IsNullOrWhiteSpace(dateFromStr)
            ? DateOnly.MinValue
            : DateOnly.Parse(dateFromStr);

        var dateTo = string.IsNullOrWhiteSpace(dateToStr)
            ? DateOnly.MaxValue
            : DateOnly.Parse(dateToStr);

        return new GetWeightData(userId, dateFrom, dateTo);
    }

    public static WeightGetResponse ToResponse(this WeightDataGroup data) => new()
    {
        UserId = data.UserId,
        Avg = data.AverageWeight,
        Max = data.MaxWeight,
        Min = data.MinWeight,
        Data = data.Data.Select(d => new WeightDataListItem(d.Date.ToDomainDateString(), d.Weight))
    };
}
