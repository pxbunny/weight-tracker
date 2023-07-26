using Mapster;
using Microsoft.AspNetCore.Mvc;
using WeightTracker.Api.Interfaces;
using WeightTracker.Api.Models;

namespace WeightTracker.Api.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class WeightController : ControllerBase
{
    private const string UserId = "123456789";
    
    private readonly IWeightDataService _weightDataService;

    public WeightController(IWeightDataService weightDataService)
    {
        _weightDataService = weightDataService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddWeightDataRequest request)
    {
        var data = new WeightData
        {
            UserId = UserId,
            Date = DateOnly.Parse(request.Date),
            Weight = request.Weight
        };
        await _weightDataService.AddAsync(data);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<WeightDataResponse>> Get([FromQuery] string date)
    {
        var data = await _weightDataService.GetAsync(UserId, DateOnly.Parse(date));
        var dto = data.Adapt<WeightDataResponse>();
        return Ok(dto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromQuery] string date, [FromBody] UpdateWeightDataRequest request)
    {
        var data = new WeightData
        {
            UserId = UserId,
            Date = DateOnly.Parse(date),
            Weight = request.Weight
        };
        await _weightDataService.UpdateAsync(data);
        return Ok();
    }
}

public sealed class AddWeightDataRequest
{
    public double Weight { get; set; }
    
    public string Date { get; set; }
}

public sealed class UpdateWeightDataRequest
{
    public double Weight { get; set; }
}

public sealed class WeightDataResponse
{
    public double Weight { get; set; }
    
    public string Date { get; set; }
}
