namespace WeightTracker.Api.Models;

internal sealed class WeightDataGroup
{
    private WeightDataGroup()
    {
    }

    public string UserId { get; set; } = string.Empty;

    public decimal? AverageWeight { get; set; }

    public decimal? MaxWeight { get; set; }

    public decimal? MinWeight { get; set; }

    public IEnumerable<WeightData> Data { get; set; } = Enumerable.Empty<WeightData>();

    public static WeightDataGroup Create(string userId, IEnumerable<WeightData> data)
    {
        var dataList = data.ToList();

        var dataGroup = new WeightDataGroup
        {
            UserId = userId,
            Data = dataList
        };

        if (dataList.Count == 0)
        {
            return dataGroup;
        }

        dataGroup.AverageWeight = Math.Round(dataList.Average(x => x.Weight), 2);
        dataGroup.MaxWeight = dataList.Max(x => x.Weight);
        dataGroup.MinWeight = dataList.Min(x => x.Weight);

        return dataGroup;
    }
}
