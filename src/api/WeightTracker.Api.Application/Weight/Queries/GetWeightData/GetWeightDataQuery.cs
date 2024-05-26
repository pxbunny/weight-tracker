using JetBrains.Annotations;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using WeightTracker.Domain.Weight.Models;
using WeightTracker.Domain.Weight.Services;

namespace WeightTracker.Api.Application.Weight.Queries.GetWeightData;

/// <summary>
/// Represents the query to get weight data.
/// </summary>
public sealed class GetWeightDataQuery : IRequest<WeightDataGroup>
{
    /// <summary>
    /// Gets or initializes the user ID.
    /// </summary>
    /// <value>
    /// The user ID.
    /// </value>
    public required string UserId { get; init; }

    /// <summary>
    /// Gets or initializes the start date.
    /// </summary>
    /// <value>
    /// The start date.
    /// </value>
    public required DateOnly StartDate { get; init; }

    /// <summary>
    /// Gets or initializes the end date.
    /// </summary>
    /// <value>
    /// The end date.
    /// </value>
    public required DateOnly EndDate { get; init; }

    /// <summary>
    /// Deconstructs the query into its properties.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public void Deconstruct(out string userId, out DateOnly startDate, out DateOnly endDate)
    {
        userId = UserId;
        startDate = StartDate;
        endDate = EndDate;
    }
}

[UsedImplicitly]
internal sealed class GetWeightDataQueryHandler(
    IWeightDataService weightDataService,
    ILogger<GetWeightDataQueryHandler> logger)
    : IRequestHandler<GetWeightDataQuery, WeightDataGroup>
{
    public async Task<WeightDataGroup> Handle(GetWeightDataQuery request, CancellationToken cancellationToken)
    {
        var (userId, startDate, endDate) = request;
        logger.LogInformation("Getting weight data for user {UserId} between {StartDate:d} and {EndDate:d}.", userId, startDate, endDate);
        var filter = request.Adapt<WeightDataFilter>();
        var result = await weightDataService.GetAsync(filter);
        logger.LogInformation("Retrieved weight data for user {UserId} between {StartDate:d} and {EndDate:d}.", userId, startDate, endDate);
        return result;
    }
}
