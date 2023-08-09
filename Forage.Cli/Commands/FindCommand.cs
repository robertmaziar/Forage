using Forage.Cli.Settings;
using Forage.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Forage.Cli.Commands
{
    public class FindCommand : Command<FindSettings>
    {

        public override int Execute(CommandContext context, FindSettings settings)
        {
            string consoleFriendlySearchTerms = string.Join(", ", settings.SearchTerms.Select(searchTerm => searchTerm));

            string searchPath = settings.SearchPath ?? Directory.GetCurrentDirectory();
            AnsiConsole.MarkupLine("Foraging through \"[yellow]{0}[/]\"...", searchPath);

            DirectoryInfo dir = new DirectoryInfo(searchPath);

            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            var queryMatchingFiles = fileList
                .Select(o => new {
                    file = o,
                    fileText = Helpers.Files.ReadAllText(o.FullName)
                })
                .Where(o =>
                    o.file.Extension == ".txt"
                    && settings.SearchTerms.All(searchTerm => o.fileText.Contains(searchTerm))
                )
                .Select(o => new { FullName = o.file.FullName, FileName = o.file.Name }).ToList();

            if (queryMatchingFiles.Count > 0)
            {
                AnsiConsole.MarkupLine("The term(s) \"[blue]{0}[/]\" were found in \"[yellow]{1}[/]\" ([green]{2}[/]):", consoleFriendlySearchTerms, searchPath, queryMatchingFiles.Count);
                foreach (var file in queryMatchingFiles)
                {
                    if (settings.CopyToOutputDir)
                    {
                        string version = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string destinationDirectory = string.Format(@"{0}\found_files\{1}\", Directory.GetCurrentDirectory(), version);

                        AnsiConsole.MarkupLine("Copying [green]{0}[/] to [yellow]{1}[/]", file.FileName, destinationDirectory);

                        if (!Directory.Exists(destinationDirectory))
                            Directory.CreateDirectory(destinationDirectory);

                        File.Copy(file.FullName, destinationDirectory + Path.GetFileName(file.FullName));
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[green]{0}[/]", file.FullName);
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("The term(s) \"[red]{0}[/]\" were not found in \"[yellow]{1}[/]\" ([red]{2}[/]):", consoleFriendlySearchTerms, searchPath, queryMatchingFiles.Count);
            }

            return 0;
        }
    }
}
