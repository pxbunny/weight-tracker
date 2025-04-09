using System.Collections.Generic;
using System.Linq;

namespace WeightTracker.Core.Models;

public sealed class WeightDataGroup
{
    private WeightDataGroup() { }

    public required string UserId { get; set; } = string.Empty;

    public decimal? AverageWeight { get; set; }

    public decimal? MaxWeight { get; set; }

    public decimal? MinWeight { get; set; }

    public IEnumerable<WeightData> Data { get; set; } = [];

    public static WeightDataGroup Create(string userId, IEnumerable<WeightData> data)
    {
        var dataList = data.ToList();

        var dataGroup = new WeightDataGroup
        {
            UserId = userId,
            Data = dataList
        };

        if (dataList.Count == 0) return dataGroup;

        dataGroup.AverageWeight = dataList.Average(d => Convert.ToDecimal(d.Weight));
        dataGroup.MaxWeight = dataList.Max(x => Convert.ToDecimal(x.Weight));
        dataGroup.MinWeight = dataList.Min(x => Convert.ToDecimal(x.Weight));

        return dataGroup;
    }
}
