# Twitter.Console

CLI for searching and scraping Twitter/X via Apify — YAML-first output optimized for LLM agent consumption.

## Install

```bash
dotnet tool install -g Twitter.Console
```

## Usage

```bash
# Set API key
export APIFY_TOKEN=your-token-here

# Search tweets
twitter search "agentic coding"
twitter search "Claude Code" --sort Top --max 10
twitter search "AI coding" --lang en --since 2025-01-01
twitter search "developer tools" --verified

# Scrape a profile
twitter scrape "https://x.com/username" --max 50

# Scrape a specific tweet/thread
twitter scrape "https://x.com/username/status/123456789"
```
