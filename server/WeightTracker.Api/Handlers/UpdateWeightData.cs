namespace WeightTracker.Api.Handlers;

public sealed record UpdateWeightData(string UserId, DateOnly Date, decimal Weight) : ICommand;

internal sealed class UpdateWeightDataHandler(IDataRepository repository)
    : ICommandHandler<UpdateWeightData>
{
    public async Task ExecuteAsync(UpdateWeightData command, CancellationToken ct)
    {
        var (userId, date, weight) = command;
        var data = new WeightData(userId, date, weight);
        await repository.UpdateAsync(data);
    }
}
