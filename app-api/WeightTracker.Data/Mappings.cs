namespace WeightTracker.Data;

internal static class Mappings
{
    public static WeightData ToDomain(this Entity entity) => new(
        entity.PartitionKey,
        DateOnly.Parse(entity.RowKey),
        Convert.ToDecimal(entity.Weight));

    public static Entity ToEntity(this WeightData domain) => new()
    {
        PartitionKey = domain.UserId,
        RowKey = domain.Date.ToDomainDateString(),
        Weight = decimal.ToDouble(domain.Weight)
    };
}
