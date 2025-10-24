using System.Linq;

namespace WeightTracker.Api.Endpoints.Weight.GetByDate;

internal sealed class WeightGetByDateEndpoint : Endpoint<WeightGetByDateRequest, WeightGetByDateResponse>
{
    public required CurrentUser CurrentUser { get; init; }

    public override void Configure() => Get("api/weight/{Date}");

    public override async Task HandleAsync(WeightGetByDateRequest request, CancellationToken ct)
    {
        var command = request.ToCommand(CurrentUser.Id);
        var data = await command.ExecuteAsync(ct);

        if (!data.Data.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = data.ToResponse();
    }
}
