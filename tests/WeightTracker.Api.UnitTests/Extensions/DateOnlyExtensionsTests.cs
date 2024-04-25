using WeightTracker.Api.Extensions;

namespace WeightTracker.Api.UnitTests.Extensions;

public sealed class DateOnlyExtensionsTests
{
    [Theory]
    [InlineData(2024, 11, 12, "2024-11-12")]
    [InlineData(1, 1, 1, "0001-01-01")]
    public void ToFormattedString_ReturnsFormattedString(int year, int month, int day, string expected)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = date.ToFormattedString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToFormattedString_ReturnsFormattedStringForDefaultDate()
    {
        // Arrange
        var date = default(DateOnly);
        const string expected = "0001-01-01";

        // Act
        var result = date.ToFormattedString();

        // Assert
        Assert.Equal(expected, result);
    }
}
