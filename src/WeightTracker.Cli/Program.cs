using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<HelloCommand>("hello")
        .WithAlias("hola")
        .WithDescription("Say hello")
        .WithExample("hello", "Phil")
        .WithExample("hello", "Phil", "--count", "4");
});

return await app.RunAsync(args);

public class HelloCommand : Command<HelloCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"Hello, [blue]{settings.Name}[/]");
        return 0;
    }
    
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Name]")]
        public string Name { get; set; }
    }
}
