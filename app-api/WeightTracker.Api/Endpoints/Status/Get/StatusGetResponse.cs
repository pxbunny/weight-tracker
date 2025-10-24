namespace WeightTracker.Api.Endpoints.Status.Get;

internal sealed record StatusGetResponse(bool AddedForToday, int MissedInLast7Days, int MissedInLast30Days);
