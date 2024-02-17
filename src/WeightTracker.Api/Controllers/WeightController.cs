using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Models;
using WeightTracker.Contracts;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryParams;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Api.Controllers;

[Authorize]
[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public sealed class WeightController : ControllerBase
{
    private const string UserId = "1234";

    private readonly IWeightDataService _weightDataService;

    public WeightController(IWeightDataService weightDataService)
    {
        _weightDataService = weightDataService;
    }

    [HttpPost(Routes.AddWeightData)]
    public async Task<IActionResult> Add(
        [FromBody] AddWeightDataRequest request)
    {
        var data = (UserId, request).Adapt<WeightData>();
        await _weightDataService.AddAsync(data);
        return Ok(); // TODO: correct response
    }

    [HttpGet(Routes.GetWeightData)]
    public async Task<ActionResult<IEnumerable<WeightDataGroupDto>>> Get(
        [FromQuery] GetWeightDataQueryParams queryParams)
    {
        var domainFilter = (UserId, queryParams).Adapt<DataFilter>();
        var data = await _weightDataService.GetAsync(domainFilter);
        var dto = data.Adapt<WeightDataGroupDto>();
        return Ok(dto);
    }

    [HttpPut(Routes.UpdateWeightData)]
    public async Task<IActionResult> Update(
        [FromRoute] string date,
        [FromBody] UpdateWeightDataRequest request)
    {
        var data = (UserId, date, request).Adapt<WeightData>();
        await _weightDataService.UpdateAsync(data);
        return Ok();
    }

    [HttpDelete(Routes.DeleteWeightData)]
    public async Task<IActionResult> Delete(
        [FromRoute] string date)
    {
        await _weightDataService.DeleteAsync(UserId, DateOnly.Parse(date));
        return Ok();
    }
}
