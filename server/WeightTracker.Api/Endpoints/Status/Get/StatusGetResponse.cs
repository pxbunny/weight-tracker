namespace WeightTracker.Api.Endpoints.Status.Get;

public record StatusGetResponse(bool AddedForToday, int MissedInLast7Days, int MissedInLast30Days);
