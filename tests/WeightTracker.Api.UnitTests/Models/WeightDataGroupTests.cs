using WeightTracker.Api.Models;

namespace WeightTracker.Api.UnitTests.Models;

public sealed class WeightDataGroupTests
{
    [Fact]
    public void Create_WithUserIdAndData_ReturnsWeightDataGroup()
    {
        // Arrange
        var data = CreateWeightData();
        const string userId = "user-id";

        // Act
        var result = WeightDataGroup.Create(userId, data);

        // Assert
        Assert.Equal(userId, result.UserId);
        Assert.Equal(data, result.Data);
    }

    [Fact]
    public void Create_WithUserIdAndData_CorrectlyCalculatesValues()
    {
        // Arrange
        var data = CreateWeightData();
        const string userId = "user-id";

        // Act
        var result = WeightDataGroup.Create(userId, data);

        // Assert
        Assert.Equal(75.25M, result.AverageWeight);
        Assert.Equal(75.5M, result.MaxWeight);
        Assert.Equal(75.0M, result.MinWeight);
    }

    [Fact]
    public void Create_WithUserIdAndEmptyData_ReturnsEmptyWeightDataGroup()
    {
        // Arrange
        var data = Array.Empty<WeightData>();
        const string userId = "user-id";

        // Act
        var result = WeightDataGroup.Create(userId, data);

        // Assert
        Assert.Equal(userId, result.UserId);
        Assert.Equal(data, result.Data);
    }

    [Fact]
    public void Create_WithUserIdAndEmptyData_ReturnsReturnsWeightDataGroupWithNullValues()
    {
        // Arrange
        var data = Array.Empty<WeightData>();
        const string userId = "user-id";

        // Act
        var result = WeightDataGroup.Create(userId, data);

        // Assert
        Assert.Null(result.AverageWeight);
        Assert.Null(result.MaxWeight);
        Assert.Null(result.MinWeight);
    }

    private static WeightData[] CreateWeightData() =>
    [
        new WeightData { UserId = "user-id", Date = new DateOnly(2024, 11, 12), Weight = 75.5M },
        new WeightData { UserId = "user-id", Date = new DateOnly(2024, 11, 13), Weight = 75.0M }
    ];
}
