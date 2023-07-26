using Azure;
using Azure.Data.Tables;

namespace WeightTracker.Infrastructure.Data;

internal sealed class WeightDataEntity : ITableEntity
{
    public double Weight { get; set; }
    
    public string PartitionKey { get; set; }
    
    public string RowKey { get; set; }
    
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }
}
