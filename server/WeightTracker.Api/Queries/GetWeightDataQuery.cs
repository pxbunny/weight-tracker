namespace WeightTracker.Api.Queries;

public sealed record GetWeightDataQuery(string UserId, DateOnly DateFrom, DateOnly DateTo)
    : ICommand<WeightDataGroup>;

internal sealed class GetWeightDataQueryHandler(IDataRepository repository)
    : ICommandHandler<GetWeightDataQuery, WeightDataGroup>
{
    public async Task<WeightDataGroup> ExecuteAsync(GetWeightDataQuery request, CancellationToken ct)
    {
        var (userId, dateFrom, dateTo) = request;
        var filter = new WeightDataFilter(userId, dateFrom, dateTo);
        return await repository.GetAsync(filter);
    }
}
