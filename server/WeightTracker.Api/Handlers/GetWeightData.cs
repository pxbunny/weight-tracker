namespace WeightTracker.Api.Handlers;

public sealed record GetWeightData(string UserId, DateOnly DateFrom, DateOnly DateTo)
    : ICommand<WeightDataGroup>;

internal sealed class GetWeightDataHandler(IDataRepository repository)
    : ICommandHandler<GetWeightData, WeightDataGroup>
{
    public async Task<WeightDataGroup> ExecuteAsync(GetWeightData command, CancellationToken ct)
    {
        var (userId, dateFrom, dateTo) = command;
        var filter = new WeightDataFilter(userId, dateFrom, dateTo);
        return await repository.GetAsync(filter);
    }
}
