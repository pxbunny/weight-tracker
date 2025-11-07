using System.Collections.Generic;
using System.Linq;

namespace WeightTracker.Core.Models;

public sealed record Status(bool AddedForToday, int MissedInLast7Days, int MissedInLast30Days)
{
    public static Status Empty => new(false, 0, 0);

    public static Status GetStatus(IList<WeightData> data, DateOnly? referenceDate = null)
    {
        var lastDate = data.LastOrDefault()?.Date;

        if (lastDate is null) return Empty;

        var today = referenceDate ?? DateOnly.FromDateTime(DateTime.Today);
        var isDataAddedForToday = data.Any(d => d.Date == today);
        var missingInLast7Days = CalculateMissedDays(data, 7, today);
        var missingInLast30Days = CalculateMissedDays(data, 30, today);
        return new Status(isDataAddedForToday, missingInLast7Days, missingInLast30Days);
    }

    private static int CalculateMissedDays(IEnumerable<WeightData> data, int totalDays, DateOnly? today = null)
    {
        if (totalDays <= 0) return 0;

        var lastDate = today ?? DateOnly.FromDateTime(DateTime.Today);
        var firstDate = lastDate.AddDays(-totalDays + 1);

        var datesInRange = data
            .Select(d => d.Date)
            .Distinct()
            .Count(d => d >= firstDate && d <= lastDate);

        return totalDays - datesInRange;
    }
}
