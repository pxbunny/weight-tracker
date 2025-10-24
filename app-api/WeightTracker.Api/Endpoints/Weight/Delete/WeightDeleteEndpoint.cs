using System.Globalization;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Delete;

internal sealed class WeightDeleteEndpoint : Endpoint<WeightDeleteRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Delete("api/weight/{Date}");

    public override async Task HandleAsync(WeightDeleteRequest request, CancellationToken ct)
    {
        var command = new RemoveWeightData(
            UserId: CurrentUser.Id,
            Date: DateOnly.Parse(request.Date, CultureInfo.InvariantCulture));
        await command.ExecuteAsync(ct);
    }
}
