using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Twitter.Console.Infrastructure;

public sealed class ApifyClient : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _http;
    private readonly string _token;

    public ApifyClient(string token)
    {
        _token = token;
        _http = new HttpClient
        {
            BaseAddress = new Uri("https://api.apify.com/v2/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
    }

    public async Task<JsonDocument> SearchAsync(SearchInput input)
    {
        var endpoint = $"acts/apidojo~tweet-scraper/run-sync-get-dataset-items?token={_token}";
        return await PostAndReadAsync(endpoint, input);
    }

    public async Task<JsonDocument> ScrapeAsync(ScrapeInput input)
    {
        var endpoint = $"acts/apidojo~tweet-scraper/run-sync-get-dataset-items?token={_token}";
        return await PostAndReadAsync(endpoint, input);
    }

    private async Task<JsonDocument> PostAndReadAsync(string endpoint, object body)
    {
        var response = await _http.PostAsJsonAsync(endpoint, body, JsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Apify API error: {response.StatusCode} — {error}");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }

    public void Dispose() => _http.Dispose();
}

public sealed class SearchInput
{
    public string[]? SearchTerms { get; init; }
    public int MaxItems { get; init; } = 20;
    public string? Sort { get; init; }
    public string? TweetLanguage { get; init; }
    public bool OnlyVerifiedUsers { get; init; }
    public bool OnlyImage { get; init; }
    public bool OnlyVideo { get; init; }
    public bool OnlyQuote { get; init; }
    public string? Start { get; init; }
    public string? End { get; init; }
}

public sealed class ScrapeInput
{
    public string[]? StartUrls { get; init; }
    public string[]? TwitterHandles { get; init; }
    public int MaxItems { get; init; } = 20;
}
