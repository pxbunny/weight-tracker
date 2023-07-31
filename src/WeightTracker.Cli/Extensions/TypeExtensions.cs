namespace WeightTracker.Cli.Extensions;

public static class TypeExtensions
{
    public static string GetCommandName(this Type type, string? commandPostfix = null)
    {
        var name = type.Name;
        
        if (!string.IsNullOrEmpty(commandPostfix))
        {
            name = name.Replace(commandPostfix, "");
        }

        return name.ToKebabCase();
    }
}
