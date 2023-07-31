using System.Reflection;

namespace WeightTracker.Cli.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetCommandTypesFromAssembly(this Assembly assembly)
    {
        return assembly.GetTypes().Where(t =>
            t.IsAssignableTo(typeof(ICommand)) &&
            t.IsClass &&
            !t.IsAbstract);
    }
}
