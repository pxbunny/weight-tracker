using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;
using WeightTracker.WebApi.Models;
using WeightTracker.WebApi.Services;

namespace WeightTracker.WebApi;

/// <summary>
/// Contains the API endpoints.
/// </summary>
internal static class Endpoints
{
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
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var data = (userId, request).Adapt<WeightData>();
        await weightDataService.AddAsync(data);
        return Results.Ok(); // TODO: correct response
    }

    private static async Task<IResult> GetWeightDataAsync(
        [AsParameters] GetWeightDataQueryParams queryParams,
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var domainFilter = (userId, queryParams).Adapt<DataFilter>();
        var data = await weightDataService.GetAsync(domainFilter);
        var dto = data.Adapt<WeightDataGroupDto>();
        return Results.Ok(dto);
    }

    private static async Task<IResult> UpdateWeightDataAsync(
        [FromRoute] string date,
        [FromBody] UpdateWeightDataRequest request,
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        var data = (userId, date, request).Adapt<WeightData>();
        await weightDataService.UpdateAsync(data);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteWeightDataAsync(
        [FromRoute] string date,
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Results.Unauthorized();
        }

        await weightDataService.DeleteAsync(userId, DateOnly.Parse(date));
        return Results.Ok();
    }
}
