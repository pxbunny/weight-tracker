using Microsoft.AspNetCore.Mvc;

namespace WeightTracker.Contracts.QueryParams;

public sealed class GetWeightDataQueryParams
{
    [ModelBinder(Name = "date_from")]
    public string? DateFrom { get; init; }
    
    [ModelBinder(Name = "date_to")]
    public string? DateTo { get; init; }
}
