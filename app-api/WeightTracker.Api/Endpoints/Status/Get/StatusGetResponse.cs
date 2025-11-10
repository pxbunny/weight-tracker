namespace WeightTracker.Api.Endpoints.Status.Get;

internal sealed record StatusGetResponse(TodayResponse Today, StreakResponse Streak, IEnumerable<AdherenceItem> Adherence);

internal sealed record TodayResponse(DateOnly Date, bool HasEntry, decimal? Weight);

internal sealed record StreakResponse(int Current, int Longest);

internal sealed record AdherenceItem(int Window, int DaysWithEntry, int DaysMissed);
