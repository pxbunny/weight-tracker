namespace WeightTracker.Cli.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
    }
}
