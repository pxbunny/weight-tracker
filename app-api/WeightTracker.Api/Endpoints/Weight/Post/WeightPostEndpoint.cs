using System.Globalization;
using WeightTracker.Api.Handlers;

namespace WeightTracker.Api.Endpoints.Weight.Post;

internal sealed class WeightPostEndpoint : Endpoint<WeightPostRequest>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Post("api/weight");

    public override async Task HandleAsync(WeightPostRequest request, CancellationToken ct)
    {
        var (weight, date) = request;
        var command = new AddWeightData(CurrentUser.Id, GetDate(date), weight);
        await command.ExecuteAsync(ct);
    }

    private static DateOnly GetDate(string date)
    {
        return string.IsNullOrWhiteSpace(date)
            ? DateOnly.Parse(date, CultureInfo.InvariantCulture)
            : DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
