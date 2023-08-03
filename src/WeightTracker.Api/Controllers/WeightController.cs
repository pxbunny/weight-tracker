using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Models;
using WeightTracker.Contracts.Filters;
using WeightTracker.Contracts.Requests;
using WeightTracker.Contracts.Responses;

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
    public async Task<IActionResult> Add([FromBody] AddWeightDataRequest request)
    {
        var data = (UserId, request).Adapt<WeightData>();
        await _weightDataService.AddAsync(data);
        return Ok(); // TODO: correct response
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetWeightDataResponse>>> Get([FromQuery] GetWeightDataFilter filter)
    {
        var domainFilter = (UserId, filter).Adapt<WeightDataFilter>();
        var data = await _weightDataService.GetAsync(domainFilter);
        var dto = data.Adapt<IEnumerable<GetWeightDataResponse>>();
        return Ok(dto);
    }

    [HttpPut("{date}")]
    public async Task<IActionResult> Update([FromRoute] string date, [FromBody] UpdateWeightDataRequest request)
    {
        var data = (UserId, date, request).Adapt<WeightData>();
        await _weightDataService.UpdateAsync(data);
        return Ok();
    }
    
    [HttpDelete("{date}")]
    public async Task<IActionResult> Delete([FromRoute] string date)
    {
        await _weightDataService.DeleteAsync(UserId, DateOnly.Parse(date));
        return Ok();
    }
}
