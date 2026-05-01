using System.ComponentModel;
using Twitter.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Twitter.Console.Commands;

public sealed class SearchCommand : AsyncCommand<SearchCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<QUERY>")]
        [Description("Search query (keywords, hashtags, or @mentions)")]
        public required string Query { get; init; }

        [CommandOption("--max <N>")]
        [Description("Maximum tweets to return")]
        [DefaultValue(20)]
        public int Max { get; init; } = 20;

        [CommandOption("--sort <SORT>")]
        [Description("Sort by: Latest, Top")]
        [DefaultValue("Latest")]
        public string Sort { get; init; } = "Latest";

        [CommandOption("--lang <LANG>")]
        [Description("Filter by tweet language (e.g. en, sv, de)")]
        public string? Lang { get; init; }

        [CommandOption("--verified")]
        [Description("Only tweets from verified users")]
        public bool Verified { get; init; }

        [CommandOption("--images")]
        [Description("Only tweets with images")]
        public bool Images { get; init; }

        [CommandOption("--videos")]
        [Description("Only tweets with videos")]
        public bool Videos { get; init; }

        [CommandOption("--since <DATE>")]
        [Description("Tweets after this date (YYYY-MM-DD)")]
        public string? Since { get; init; }

        [CommandOption("--until <DATE>")]
        [Description("Tweets before this date (YYYY-MM-DD)")]
        public string? Until { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        AnsiConsole.MarkupLine("[grey]Searching Twitter/X (this may take 30–60s)...[/]");

        using var client = settings.CreateClient();
        var doc = await client.SearchAsync(new SearchInput
        {
            SearchTerms = [settings.Query],
            MaxItems = settings.Max,
            Sort = settings.Sort,
            TweetLanguage = settings.Lang,
            OnlyVerifiedUsers = settings.Verified,
            OnlyImage = settings.Images,
            OnlyVideo = settings.Videos,
            Start = settings.Since,
            End = settings.Until
        });

        YamlOutput.Write(doc);
        return 0;
    }
}
