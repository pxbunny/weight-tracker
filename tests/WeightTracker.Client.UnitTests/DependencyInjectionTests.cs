using Microsoft.Extensions.DependencyInjection;

namespace WeightTracker.Client.UnitTests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddApiClient_WithNullBaseUrl_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var action = () => services.AddApiClient(null);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void AddApiClient_WithEmptyBaseUrl_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var action = () => services.AddApiClient(string.Empty);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void AddApiClient_WithWhitespaceBaseUrl_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var action = () => services.AddApiClient(" ");

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void AddApiClient_WithValidBaseUrl_AddsApiClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApiClient("https://api.example.com");

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var apiClient = serviceProvider.GetRequiredService<IApiClient>();
        Assert.NotNull(apiClient);
    }
}
