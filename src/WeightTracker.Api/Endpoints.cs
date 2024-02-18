using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Models;
using WeightTracker.Api.Services;
using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Api;

internal static class Endpoints
{
    private const string TagName = "Weight";

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
        if (currentUser.UserId is null)
        {
            return Results.Unauthorized();
        }

        var data = (currentUser.UserId, request).Adapt<WeightData>();
        await weightDataService.AddAsync(data);
        return Results.Ok(); // TODO: correct response
    }

    private static async Task<IResult> GetWeightDataAsync(// [FromQuery] GetWeightDataQueryParams queryParams,
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        if (currentUser.UserId is null)
        {
            return Results.Unauthorized();
        }

        // var domainFilter = (UserId, queryParams).Adapt<DataFilter>();
        var domainFilter = new DataFilter
        {
            UserId = currentUser.UserId,
            DateFrom = DateOnly.MinValue,
            DateTo = DateOnly.MaxValue
        };

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
        if (currentUser.UserId is null)
        {
            return Results.Unauthorized();
        }

        var data = (currentUser.UserId, date, request).Adapt<WeightData>();
        await weightDataService.UpdateAsync(data);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteWeightDataAsync(
        [FromRoute] string date,
        [FromServices] IWeightDataService weightDataService,
        [FromServices] ICurrentUserService currentUser)
    {
        if (currentUser.UserId is null)
        {
            return Results.Unauthorized();
        }

        await weightDataService.DeleteAsync(currentUser.UserId, DateOnly.Parse(date));
        return Results.Ok();
    }
}
