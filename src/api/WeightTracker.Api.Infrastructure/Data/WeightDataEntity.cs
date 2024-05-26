using Azure;
using Azure.Data.Tables;

namespace WeightTracker.Api.Infrastructure.Data;

/// <summary>
/// Represents the weight data entity.
/// </summary>
/// <remarks>
/// This class is used to store weight data in the Azure Table Storage.
/// </remarks>
/// <seealso cref="ITableEntity" />
internal sealed class WeightDataEntity : ITableEntity
{
    /// <summary>
    /// Gets or sets the weight.
    /// </summary>
    /// <value>The weight.</value>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the partition key.
    /// </summary>
    /// <remarks>
    /// The partition key is used to group entities together.
    /// In this case, the partition key is the user ID.
    /// </remarks>
    /// <value>The partition key.</value>
    public string PartitionKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the row key.
    /// </summary>
    /// <remarks>
    /// The row key is used to uniquely identify an entity within a partition.
    /// In this case, the row key is the date of the weight data entry.
    /// </remarks>
    /// <value>The row key.</value>
    public string RowKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    /// <value>The timestamp.</value>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the entity's ETag.
    /// </summary>
    /// <value>The entity's ETag.</value>
    public ETag ETag { get; set; }
}
