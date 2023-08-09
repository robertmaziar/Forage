using Spectre.Console.Cli;

namespace Forage.Cli.Settings
{
    public class FindSettings : CommandSettings
    {

        [CommandArgument(0, "<SEARCH_TERMS>")]
        public string[] SearchTerms { get; set; }

        [CommandOption("-p|--path")]
        public string? SearchPath { get; init; }

        [CommandOption("-c|--copy")]
        public bool CopyToOutputDir { get; init; }
    }
}
