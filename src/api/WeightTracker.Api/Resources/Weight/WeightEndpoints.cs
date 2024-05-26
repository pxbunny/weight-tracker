using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Application.Weight.Commands.AddWeightData;
using WeightTracker.Api.Application.Weight.Commands.RemoveWeightData;
using WeightTracker.Api.Application.Weight.Commands.UpdateWeightData;
using WeightTracker.Api.Application.Weight.Queries.GetWeightData;
using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;
using WeightTracker.Domain.Common.Interfaces;

namespace WeightTracker.Api.Resources.Weight;

/// <summary>
/// Contains the API endpoints.
/// </summary>
internal static class WeightEndpoints
{
    /// <summary>
    /// The tag name.
    /// </summary>
    /// <remarks>
    /// The tag name used to group the API endpoints in the Swagger documentation.
    /// </remarks>
    private const string TagName = "Weight";

    /// <summary>
    /// Registers the API endpoints to the specified route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Routes.AddWeightData, AddWeightDataAsync)
            .RequireAuthorization()
            .WithTags(TagName);

        app.MapGet(Routes.GetWeightData, GetWeightDataAsync)
            .RequireAuthorization()
            .WithTags(TagName);

        app.MapPut(Routes.UpdateWeightData, UpdateWeightDataAsync)
            .RequireAuthorization()
            .WithTags(TagName);

        app.MapDelete(Routes.DeleteWeightData, DeleteWeightDataAsync)
            .RequireAuthorization()
            .WithTags(TagName);
    }

    private static async Task<IResult> AddWeightDataAsync(
        [FromBody] AddWeightDataRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IUser currentUser)
    {
        var userId = currentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var (weight, date) = request;

        var command = new AddWeightDataCommand
        {
            UserId = userId,
            Date = date is not null
                ? DateOnly.Parse(date)
                : DateOnly.FromDateTime(DateTime.UtcNow),
            Weight = weight
        };

        await mediator.Send(command);
        return Results.Ok(); // TODO: correct response
    }

    private static async Task<IResult> GetWeightDataAsync(
        [AsParameters] GetWeightDataQueryParams queryParams,
        [FromServices] IMediator mediator,
        [FromServices] IUser currentUser)
    {
        var userId = currentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var (dateFrom, dateTo) = queryParams;

        var query = new GetWeightDataQuery
        {
            UserId = userId,
            StartDate = dateFrom is not null
                ? DateOnly.Parse(dateFrom)
                : DateOnly.MinValue,
            EndDate = dateTo is not null
                ? DateOnly.Parse(dateTo)
                : DateOnly.MaxValue
        };

        var result = await mediator.Send(query);
        var dto = result.Adapt<WeightDataGroupDto>();
        return Results.Ok(dto);
    }

    private static async Task<IResult> UpdateWeightDataAsync(
        [FromRoute] string date,
        [FromBody] UpdateWeightDataRequest request,
        [FromServices] IMediator mediator,
        [FromServices] IUser currentUser)
    {
        var userId = currentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var command = new UpdateWeightDataCommand
        {
            UserId = userId,
            Date = DateOnly.Parse(date),
            Weight = request.Weight
        };

        await mediator.Send(command);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteWeightDataAsync(
        [FromRoute] string date,
        [FromServices] IMediator mediator,
        [FromServices] IUser currentUser)
    {
        var userId = currentUser.Id;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var command = new RemoveWeightDataCommand
        {
            UserId = userId,
            Date = DateOnly.Parse(date)
        };

        await mediator.Send(command);
        return Results.Ok();
    }
}
