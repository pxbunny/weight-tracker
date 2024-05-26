using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Application.Weight.Commands.RemoveWeightData;

/// <summary>
/// Represents the command to remove weight data.
/// </summary>
public sealed class RemoveWeightDataCommand : IRequest
{
    /// <summary>
    /// Gets or initializes the user ID.
    /// </summary>
    /// <value>
    /// The user ID.
    /// </value>
    public required string UserId { get; init; }

    /// <summary>
    /// Gets or initializes the date.
    /// </summary>
    /// <value>
    /// The date.
    /// </value>
    public required DateOnly Date { get; init; }

    /// <summary>
    /// Deconstructs the command into its properties.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="date">The date.</param>
    public void Deconstruct(out string userId, out DateOnly date)
    {
        userId = UserId;
        date = Date;
    }
}

/// <summary>
/// Represents the command handler for the <see cref="RemoveWeightDataCommand"/>.
/// </summary>
[UsedImplicitly]
internal sealed class RemoveWeightDataCommandHandler(
    IWeightDataService weightDataService,
    ILogger<RemoveWeightDataCommandHandler> logger)
    : IRequestHandler<RemoveWeightDataCommand>
{
    /// <summary>
    /// The weight data service.
    /// </summary>
    /// <param name="request">The command to remove weight data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the command completion.</returns>
    public async Task Handle(RemoveWeightDataCommand request, CancellationToken cancellationToken)
    {
        var (userId, date) = request;
        logger.LogInformation("Removing weight data for user {UserId} on {Date:d}.", userId, date);
        await weightDataService.DeleteAsync(userId, date);
        logger.LogInformation("Weight data removed for user {UserId} on {Date:d}.", userId, date);
    }
}
