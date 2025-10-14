using System.Collections.Generic;
using System.Linq;

namespace WeightTracker.Core.Models;

public record Status(bool AddedForToday, int MissedInLast7Days, int MissedInLast30Days)
{
    public static Status GetStatus(IList<WeightData> data)
    {
        var lastDate = data.LastOrDefault()?.Date;

        if (lastDate is null)
            return new Status(false, 0, 0);

        return new Status(
            lastDate == DateOnly.FromDateTime(DateTime.Today),
            CalculateMissedDays(data, 7),
            CalculateMissedDays(data, 30));
    }

    private static int CalculateMissedDays(IEnumerable<WeightData> data, int totalDays)
    {
        if (totalDays <= 0) return 0;

        var lastDate = DateOnly.FromDateTime(DateTime.Today);
        var firstDate = lastDate.AddDays(-totalDays);

        var datesInRange = data
            .Select(d => d.Date)
            .Distinct()
            .Count(d => d >= firstDate && d <= lastDate);

        return totalDays - datesInRange;
    }
}
