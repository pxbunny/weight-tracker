namespace WeightTracker.Cli.Extensions;

public static class ConfiguratorExtensions
{
    public static void RegisterCommands(this IConfigurator config, Type t, string name)
    {
        var addCommand = config.GetType()
            .GetMethod(nameof(config.AddCommand))
            ?.MakeGenericMethod(t);
        addCommand?.Invoke(config, new object[] { name });
    }
}
