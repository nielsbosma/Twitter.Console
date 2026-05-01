using System.ComponentModel;
using Twitter.Console.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Twitter.Console.Commands;

public sealed class ScrapeCommand : AsyncCommand<ScrapeCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "<URL>")]
        [Description("Twitter/X URL to scrape (profile, tweet, or thread)")]
        public required string Url { get; init; }

        [CommandOption("--max <N>")]
        [Description("Maximum tweets to return")]
        [DefaultValue(20)]
        public int Max { get; init; } = 20;

        [CommandOption("--followers")]
        [Description("Include followers list (profile URLs only)")]
        public bool Followers { get; init; }

        [CommandOption("--following")]
        [Description("Include following list (profile URLs only)")]
        public bool Following { get; init; }

        [CommandOption("--retweeters")]
        [Description("Include retweeters (tweet URLs only)")]
        public bool Retweeters { get; init; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        AnsiConsole.MarkupLine("[grey]Scraping Twitter/X (this may take 30–60s)...[/]");

        using var client = settings.CreateClient();
        var doc = await client.ScrapeAsync(new ScrapeInput
        {
            StartUrls = [new { url = settings.Url }],
            MaxTweets = settings.Max,
            GetFollowers = settings.Followers,
            GetFollowing = settings.Following,
            GetRetweeters = settings.Retweeters
        });

        YamlOutput.Write(doc);
        return 0;
    }
}
