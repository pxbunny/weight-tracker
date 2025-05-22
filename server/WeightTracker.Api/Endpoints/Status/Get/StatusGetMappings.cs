namespace WeightTracker.Api.Endpoints.Status.Get;

public static class StatusGetMappings
{
    public static StatusGetResponse ToResponse(this WeightTracker.Core.Models.Status data) => new(
        AddedForToday: data.AddedForToday,
        MissedInLast7Days: data.MissedInLast7Days,
        MissedInLast30Days: data.MissedInLast30Days);
}
