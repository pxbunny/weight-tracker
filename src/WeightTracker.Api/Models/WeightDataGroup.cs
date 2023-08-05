namespace WeightTracker.Api.Models;

public sealed class WeightDataGroup
{
    private WeightDataGroup()
    {
        
    }
    
    public string UserId { get; set; } = string.Empty;

    public decimal? AverageWeight { get; set; }
    
    public decimal? MaxWeight { get; set; }
    
    public decimal? MinWeight { get; set; }

    public IEnumerable<WeightData> Data { get; set; } = Enumerable.Empty<WeightData>();

    public static WeightDataGroup Create(
        string userId,
        IEnumerable<WeightData> data)
    {
        var dataList = data.ToList();
        
        var dataGroup = new WeightDataGroup
        {
            UserId = userId,
            Data = dataList
        };

        if (!dataList.Any())
        {
            return dataGroup;
        }

        dataGroup.AverageWeight = dataList.Average(x => x.Weight);
        dataGroup.MaxWeight = dataList.Max(x => x.Weight);
        dataGroup.MinWeight = dataList.Min(x => x.Weight);

        return dataGroup;
    }
}
