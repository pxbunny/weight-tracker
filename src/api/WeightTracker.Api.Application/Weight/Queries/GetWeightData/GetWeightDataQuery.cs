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

/// <summary>
/// Represents the query handler for the <see cref="GetWeightDataQuery"/>.
/// </summary>
/// <param name="weightDataService">The weight data service.</param>
/// <param name="logger">The logger.</param>
[UsedImplicitly]
internal sealed class GetWeightDataQueryHandler(
    IWeightDataService weightDataService,
    ILogger<GetWeightDataQueryHandler> logger)
    : IRequestHandler<GetWeightDataQuery, WeightDataGroup>
{
    /// <summary>
    /// Gets the weight data.
    /// </summary>
    /// <param name="request">The query to get weight data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the query completion.</returns>
    /// <exception cref="NotImplementedException">Thrown when the query fails.</exception>
    public async Task<WeightDataGroup> Handle(GetWeightDataQuery request, CancellationToken cancellationToken)
    {
        var (userId, startDate, endDate) = request;
        var filter = request.Adapt<WeightDataFilter>();
        var response = await weightDataService.GetAsync(filter);

        if (response.IsSuccess)
        {
            logger.LogInformation("Successfully retrieved weight data for user {UserId} from {StartDate} to {EndDate}", userId, startDate, endDate);
            return response.Data!;
        }

        // TODO: handle error
        logger.LogError("Failed to retrieve weight data for user {UserId} from {StartDate} to {EndDate}", userId, startDate, endDate);
        throw new NotImplementedException();
    }
}
