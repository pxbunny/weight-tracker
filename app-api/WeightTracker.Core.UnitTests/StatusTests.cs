using System.Globalization;
using WeightTracker.Core.Models;

namespace WeightTracker.Core.UnitTests;

public class StatusTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    [Theory]
    [InlineData("2024-12-01", "2024-12-31", 1, 1, "2024-12-26")]
    [InlineData("2024-12-01", "2024-12-31", 0, 0, "0001-01-01")]
    [InlineData("2024-12-01", "2024-12-31", 2, 3, "2024-12-31", "2024-12-26", "2024-12-20")]
    public void GetStatus_ShouldCalculateMissingRecordsCorrectly(
        string dateFrom,
        string dateTo,
        int expectedMissingRecordsLast7Days,
        int expectedMissingRecordsLast30Days,
        params string[] excludedDates)
    {
        const decimal weight = 50;
        var userId = Guid.NewGuid().ToString();

        var weightData = GenerateWeightData(userId, weight, dateFrom, dateTo, excludedDates);
        var status = Status.GetStatus(weightData, DateOnly.FromDateTime(DateTime.Parse(dateTo, _culture)));

        Assert.Equal(expectedMissingRecordsLast7Days, status.MissedInLast7Days);
        Assert.Equal(expectedMissingRecordsLast30Days, status.MissedInLast30Days);
    }

    [Fact]
    public void GetStatus_ShouldShowCorrectStatus_WhenDataForTodayIsAlreadyAdded()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var weightData = new WeightData("", today, 50);
        var status = Status.GetStatus([weightData], today);
        Assert.True(status.AddedForToday);
    }

    [Fact]
    public void GetStatus_ShouldShowCorrectStatus_WhenDataForTodayIsMissing()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var weightData = new WeightData("", today, 50);
        var status = Status.GetStatus([weightData], today.AddDays(1));
        Assert.False(status.AddedForToday);
    }

    private IList<WeightData> GenerateWeightData(
        string userId,
        decimal weight,
        string dateFrom,
        string dateTo,
        params IEnumerable<string> excludedDates)
    {
        var from = DateTime.Parse(dateFrom, _culture);
        var to = DateTime.Parse(dateTo, _culture);

        var dateRange = Enumerable.Range(0, 1 + to.Subtract(from).Days)
            .Select(offset => from.AddDays(offset))
            .ToList();

        foreach (var excludedDate in excludedDates)
        {
            var date = DateTime.Parse(excludedDate, _culture);
            dateRange.Remove(date);
        }

        return [.. dateRange.Select(d => new WeightData(userId, DateOnly.FromDateTime(d), weight))];
    }
}
