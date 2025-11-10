using System.Collections.Generic;
using System.Linq;

using Today = (System.DateOnly Date, bool HasEntry, decimal? Weight);

namespace WeightTracker.Core.Models;

public sealed record Status(Today Today, Streak Streak, IEnumerable<Adherence> Adherence)
{
    public static Status Create(IList<WeightData> data, DateOnly? referenceDate = null)
    {
        var today = referenceDate ?? DateOnly.FromDateTime(DateTime.Today);
        var currentData = data.SingleOrDefault(d => d.Date == today);

        return new Status(
            currentData is not null ? (today, true, currentData.Weight) : (today, false, null),
            Streak.Create(data, referenceDate),
            [
                Models.Adherence.Create(data, 7, referenceDate),
                Models.Adherence.Create(data, 14, referenceDate),
                Models.Adherence.Create(data, 30, referenceDate)
            ]);
    }
}
