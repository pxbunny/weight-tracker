using Microsoft.AspNetCore.Mvc;

namespace WeightTracker.Contracts.QueryParams;

public sealed class GetWeightDataQueryParams
{
    public GetWeightDataQueryParams()
    {
    }

    public GetWeightDataQueryParams(string? dateFrom, string? dateTo)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    [FromQuery(Name = "date_from")]
    public string? DateFrom { get; init; }

    [FromQuery(Name = "date_to")]
    public string? DateTo { get; init; }
}
