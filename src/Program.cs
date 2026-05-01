using Twitter.Console.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("twitter");

    config.AddCommand<SearchCommand>("search")
        .WithDescription("Search Twitter/X for tweets matching a query");

    config.AddCommand<ScrapeCommand>("scrape")
        .WithDescription("Scrape a Twitter/X URL (profile, tweet, or thread)");
});

return app.Run(args);
