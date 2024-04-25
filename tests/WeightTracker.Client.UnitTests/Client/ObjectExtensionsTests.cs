using Microsoft.AspNetCore.Mvc;
using WeightTracker.Client.Client;

namespace WeightTracker.Client.UnitTests.Client;

public sealed class ObjectExtensionsTests
{
    [Fact]
    public void BuildQueryString_WithEmptyObject_ReturnsEmptyString()
    {
        // Arrange
        var obj = new { };

        // Act
        var result = obj.BuildQueryString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void BuildQueryString_WithSingleProperty_ReturnsQueryString()
    {
        // Arrange
        var obj = new TestQueryParams
        {
            Name = "Bob",
            Age = 30
        };

        // Act
        var result = obj.BuildQueryString();

        // Assert
        Assert.Equal("name=Bob&age=30", result);
    }

    private sealed class TestQueryParams
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "age")]
        public int Age { get; set; }
    }
}
