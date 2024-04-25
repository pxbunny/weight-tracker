using System.Text.Json;
using WeightTracker.Client.Client;

namespace WeightTracker.Client.UnitTests.Client;

public sealed class HttpResponseMessageExtensionsTests
{
    [Fact]
    public async Task ReadContentAsAsync_WithNullResponse_ThrowsException()
    {
        // Arrange
        var response = (HttpResponseMessage?)null;

        // Act
        var action = () => response!.ReadContentAsAsync<TestDto>(CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(action);
    }

    [Fact]
    public async Task ReadContentAsAsync_WithNullContent_ThrowsException()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            Content = null
        };

        // Act
        var action = () => response.ReadContentAsAsync<TestDto>(CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<JsonException>(action);
    }

    [Fact]
    public async Task ReadContentAsAsync_WithInvalidContent_ThrowsException()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            Content = new StringContent("invalid")
        };

        // Act
        var action = () => response.ReadContentAsAsync<TestDto>(CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<JsonException>(action);
    }

    [Fact]
    public async Task ReadContentAsAsync_WithValidContent_ReturnsData()
    {
        // Arrange
        var data = new TestDto("Bob", 30);

        var content = JsonSerializer.Serialize(data);
        var response = new HttpResponseMessage
        {
            Content = new StringContent(content)
        };

        // Act
        var result = await response.ReadContentAsAsync<TestDto>(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Age, result.Age);
    }

    private sealed record TestDto(string? Name, int Age);
}
