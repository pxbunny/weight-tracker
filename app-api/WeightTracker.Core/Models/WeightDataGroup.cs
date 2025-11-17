using System.Collections.Generic;
using System.Linq;

namespace WeightTracker.Core.Models;

public sealed class WeightDataGroup
{
    private WeightDataGroup() { }

    public required string UserId { get; init; } = string.Empty;

    public required Today Today { get; init; }

    public decimal? AverageWeight { get; set; }

    public decimal? MaxWeight { get; set; }

    public decimal? MinWeight { get; set; }

    public IEnumerable<WeightData> Data { get; set; } = [];

    public static WeightDataGroup Create(string userId, IList<WeightData> data)
    {
        var dataGroup = new WeightDataGroup
        {
            UserId = userId,
            Today = Today.Create(data),
            Data = data
        };

        if (data.Count == 0) return dataGroup;

        dataGroup.AverageWeight = data.Average(d => Convert.ToDecimal(d.Weight));
        dataGroup.MaxWeight = data.Max(x => Convert.ToDecimal(x.Weight));
        dataGroup.MinWeight = data.Min(x => Convert.ToDecimal(x.Weight));

        return dataGroup;
    }
}
