namespace WeightTracker.Api.Handlers;

internal sealed record GetStatus(string UserId) : ICommand<Status>;

internal sealed class GetStatusHandler(IDataRepository repository) : ICommandHandler<GetStatus, Status>
{
    public async Task<Status> ExecuteAsync(GetStatus command, CancellationToken ct)
    {
        var filter = new WeightDataFilter(command.UserId);
        var response = await repository.GetAsync(filter, ct);
        return Status.GetStatus([.. response.Data]);
    }
}
