using System.Net;
using System.Text;
using PolyhydraGames.Api.Steam;
using PolyhydraGames.Core.Interfaces;

namespace Api.Steam.Test;

public sealed class PassThroughCacheService : ICacheService
{
    public Task<string> GetString(string key, Func<Task<string>> func) => func();

    public Task<T> Get<T>(string key, Func<Task<T>> func) => func();

    public Task<string> GetString(string key, Func<Task<string>> func, TimeSpan? ttl, bool forceRefresh = false) => func();

    public Task<T> Get<T>(string key, Func<Task<T>> func, TimeSpan? ttl, bool forceRefresh = false) => func();

    public Task Clear(string key) => Task.CompletedTask;
}

public sealed class RecordingHttpMessageHandler : HttpMessageHandler
{
    private readonly List<(Func<Uri, bool> Match, string Body, HttpStatusCode Status)> _responses = new();

    public List<Uri> Requests { get; } = new();

    public void WhenContains(string fragment, string body, HttpStatusCode status = HttpStatusCode.OK)
    {
        _responses.Add((uri => uri.AbsoluteUri.Contains(fragment, StringComparison.OrdinalIgnoreCase), body, status));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = request.RequestUri ?? throw new InvalidOperationException("Request URI was null.");
        Requests.Add(uri);

        var response = _responses.FirstOrDefault(x => x.Match(uri));
        if (response.Match is null)
        {
            throw new InvalidOperationException($"No canned response configured for {uri}.");
        }

        return Task.FromResult(new HttpResponseMessage(response.Status)
        {
            Content = new StringContent(response.Body, Encoding.UTF8, "application/json")
        });
    }
}

public sealed class SteamServiceTestHarness
{
    public SteamServiceTestHarness(string apiKey = "test-api-key")
    {
        Handler = new RecordingHttpMessageHandler();
        Cache = new PassThroughCacheService();
        Service = new SteamService(new HttpClient(Handler), Cache, new DefaultSteamServiceConfiguration
        {
            ApiKey = apiKey
        });
    }

    public RecordingHttpMessageHandler Handler { get; }

    public PassThroughCacheService Cache { get; }

    public SteamService Service { get; }
}
