using Forage.Cli.Commands;
using Spectre.Console.Cli;

CommandApp? app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<FindCommand>("find");
});

return app.Run(args);