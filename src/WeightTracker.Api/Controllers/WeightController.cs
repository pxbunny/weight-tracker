using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Models;
using WeightTracker.Contracts.DTOs;
using WeightTracker.Contracts.QueryStrings;
using WeightTracker.Contracts.Requests;

namespace WeightTracker.Api.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class WeightController : ControllerBase
{
    private const string UserId = "1234";
    
    private readonly IWeightDataService _weightDataService;

    public WeightController(IWeightDataService weightDataService)
    {
        _weightDataService = weightDataService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddWeightDataRequest request)
    {
        var data = (UserId, request).Adapt<WeightData>();
        await _weightDataService.AddAsync(data);
        return Ok(); // TODO: correct response
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeightDataGroupDto>>> Get([FromQuery] GetWeightDataQueryString queryString)
    {
        var domainFilter = (UserId, queryString).Adapt<DataFilter>();
        var data = await _weightDataService.GetAsync(domainFilter);
        var dto = data.Adapt<WeightDataGroupDto>();
        return Ok(dto);
    }

    [HttpPut("{date}")]
    public async Task<IActionResult> Update(
        string date,
        UpdateWeightDataRequest request)
    {
        var data = (UserId, date, request).Adapt<WeightData>();
        await _weightDataService.UpdateAsync(data);
        return Ok();
    }
    
    [HttpDelete("{date}")]
    public async Task<IActionResult> Delete(string date)
    {
        await _weightDataService.DeleteAsync(UserId, DateOnly.Parse(date));
        return Ok();
    }
}
