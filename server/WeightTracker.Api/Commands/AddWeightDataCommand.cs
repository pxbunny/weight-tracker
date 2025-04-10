namespace WeightTracker.Api.Commands;

public sealed record AddWeightDataCommand(string UserId, DateOnly Date, decimal Weight) : ICommand;

internal sealed class AddWeightDataCommandHandler(IDataRepository repository)
    : ICommandHandler<AddWeightDataCommand>
{
    public async Task ExecuteAsync(AddWeightDataCommand request, CancellationToken ct)
    {
        var (userId, date, weight) = request;
        var data = new WeightData(userId, date, weight);
        await repository.AddAsync(data);
    }
}
