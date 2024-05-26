using JetBrains.Annotations;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using WeightTracker.Domain.Weight.Models;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Application.Weight.Commands.AddWeightData;

/// <summary>
/// Represents the command to add weight data.
/// </summary>
public sealed class AddWeightDataCommand : IRequest
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
    /// Gets or initializes the weight.
    /// </summary>
    /// <value>
    /// The weight to add for the specified date.
    /// </value>
    public required decimal Weight { get; init; }

    /// <summary>
    /// Deconstructs the command into its properties.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="date">The date.</param>
    /// <param name="weight">The weight.</param>
    public void Deconstruct(out string userId, out DateOnly date, out decimal weight)
    {
        userId = UserId;
        date = Date;
        weight = Weight;
    }
}

/// <summary>
/// Represents the command handler for the <see cref="AddWeightDataCommand"/>.
/// </summary>
[UsedImplicitly]
internal sealed class AddWeightDataCommandHandler(
    IWeightDataService weightDataService,
    ILogger<AddWeightDataCommandHandler> logger)
    : IRequestHandler<AddWeightDataCommand>
{
    /// <summary>
    /// Adds weight data to the data store.
    /// </summary>
    /// <param name="request">The command to add weight data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the command completion.</returns>
    public async Task Handle(AddWeightDataCommand request, CancellationToken cancellationToken)
    {
        var (userId, date, _) = request;
        logger.LogInformation("Adding weight data for user {UserId} on {Date:d}.", userId, date);
        var weightData = request.Adapt<WeightData>();
        await weightDataService.AddAsync(weightData);
        logger.LogInformation("Weight data added for user {UserId} on {Date:d}.", userId, date);
    }
}
