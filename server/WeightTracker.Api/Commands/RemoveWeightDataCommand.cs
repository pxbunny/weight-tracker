namespace WeightTracker.Api.Commands;

public sealed record RemoveWeightDataCommand(string UserId, DateOnly Date) : ICommand;

internal sealed class RemoveWeightDataCommandHandler(IDataRepository repository)
    : ICommandHandler<RemoveWeightDataCommand>
{
    public async Task ExecuteAsync(RemoveWeightDataCommand request, CancellationToken ct)
    {
        var (userId, date) = request;
        await repository.DeleteAsync(userId, date);
    }
}
