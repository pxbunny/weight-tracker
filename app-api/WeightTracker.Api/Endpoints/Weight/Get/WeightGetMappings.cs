using System.Globalization;
using System.Linq;
using WeightTracker.Api.Handlers;
using WeightTracker.Api.SharedContracts;

namespace WeightTracker.Api.Endpoints.Weight.Get;

internal static class WeightGetMappings
{
    public static GetWeightData ToCommand(this WeightGetRequest request, string userId)
    {
        var (dateFromStr, dateToStr) = request;

        var dateFrom = string.IsNullOrWhiteSpace(dateFromStr)
            ? DateOnly.MinValue
            : DateOnly.Parse(dateFromStr, CultureInfo.InvariantCulture);

        var dateTo = string.IsNullOrWhiteSpace(dateToStr)
            ? DateOnly.MaxValue
            : DateOnly.Parse(dateToStr, CultureInfo.InvariantCulture);

        return new GetWeightData(userId, dateFrom, dateTo);
    }

    public static WeightGetResponse ToResponse(this WeightDataGroup data) => new()
    {
        UserId = data.UserId,
        Today = new TodayResponse(data.Today.Date, data.Today.HasEntry, data.Today.Weight),
        Avg = data.AverageWeight,
        Max = data.MaxWeight,
        Min = data.MinWeight,
        Data = data.Data.Select(d => new WeightResponseItem(d.Date.ToDomainDateString(), d.Weight))
    };
}
