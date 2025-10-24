using System.Globalization;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Put;

internal sealed class WeightPutEndpoint : Endpoint<WeightPutRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Put("api/weight/{Date}");

    public override async Task HandleAsync(WeightPutRequest request, CancellationToken ct)
    {
        var (date, weight) = request;
        var command = new UpdateWeightData(
            UserId: CurrentUser.Id,
            Date: DateOnly.Parse(date, CultureInfo.InvariantCulture),
            Weight: weight);
        await command.ExecuteAsync(ct);
    }
}
