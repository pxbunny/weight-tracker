using System.Collections.Generic;
using System.Linq;

namespace WeightTracker.Core.Models;

public sealed record Stats(decimal AverageWeight, decimal MaxWeight, decimal MinWeight)
{
    public static Stats Create(IList<WeightData> data) => new(
        AverageWeight: data.Average(d => Convert.ToDecimal(d.Weight)),
        MaxWeight: data.Max(x => Convert.ToDecimal(x.Weight)),
        MinWeight: data.Min(x => Convert.ToDecimal(x.Weight)));
}
