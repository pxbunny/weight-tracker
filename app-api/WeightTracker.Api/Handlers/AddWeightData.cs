namespace WeightTracker.Api.Handlers;

internal sealed record AddWeightData(string UserId, DateOnly Date, decimal Weight) : ICommand;

internal sealed class AddWeightDataHandler(IDataRepository repository) : ICommandHandler<AddWeightData>
{
    public async Task ExecuteAsync(AddWeightData command, CancellationToken ct)
    {
        var (userId, date, weight) = command;
        var data = new WeightData(userId, date, weight);
        await repository.AddAsync(data, ct);
    }
}
