namespace WeightTracker.Api.Commands;

public sealed record UpdateWeightDataCommand(string UserId, DateOnly Date, decimal Weight) : ICommand;

internal sealed class UpdateWeightDataCommandHandler(IDataRepository repository)
    : ICommandHandler<UpdateWeightDataCommand>
{
    public async Task ExecuteAsync(UpdateWeightDataCommand request, CancellationToken ct)
    {
        var (userId, date, weight) = request;
        var data = new WeightData(userId, date, weight);
        await repository.UpdateAsync(data);
    }
}
