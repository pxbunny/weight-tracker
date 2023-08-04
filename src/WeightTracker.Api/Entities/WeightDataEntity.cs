using Azure;
using Azure.Data.Tables;

namespace WeightTracker.Api.Entities;

internal sealed class WeightDataEntity : ITableEntity
{
    public double Weight { get; set; }

    public string PartitionKey { get; set; } = string.Empty;
    
    public string RowKey { get; set; } = string.Empty;
    
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }
}
