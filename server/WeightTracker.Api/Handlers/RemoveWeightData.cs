namespace WeightTracker.Api.Handlers;

public sealed record RemoveWeightData(string UserId, DateOnly Date) : ICommand;

internal sealed class RemoveWeightDataHandler(IDataRepository repository) : ICommandHandler<RemoveWeightData>
{
    public async Task ExecuteAsync(RemoveWeightData command, CancellationToken ct)
    {
        var (userId, date) = command;
        await repository.DeleteAsync(userId, date, ct);
    }
}
