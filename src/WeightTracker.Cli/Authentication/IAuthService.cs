namespace WeightTracker.Cli.Authentication;

public interface IAuthService
{
    Task AcquireTokenAsync();
}
