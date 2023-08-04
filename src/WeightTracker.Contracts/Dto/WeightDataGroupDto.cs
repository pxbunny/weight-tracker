namespace WeightTracker.Contracts.Dto;

public sealed class WeightDataGroupDto
{
    public string UserId { get; init; } = null!;

    public decimal? AverageWeight { get; init; }
    
    public decimal? MaxWeight { get; init; }
    
    public decimal? MinWeight { get; init; }

    public IEnumerable<WeightDataListItemDto> Data { get; init; } = null!;
}
