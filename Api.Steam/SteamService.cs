using System.Diagnostics;
using System.Text.Json;
using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Extensions;

namespace PolyhydraGames.Api.Steam;

public class SteamService : ISteamService
{
    private readonly HttpClient _client;
    private readonly ICacheService _redis;
    private List<SimpleSteamGame> _cachedGames;
    public SteamService(HttpClient client, ICacheService redis)
    {
        _client = client;
        _redis = redis;
    }

    public async Task<GameResult> GetGameData(string name)
    {
        var result = await GetSimpleGamesCached();
        var game = result.OrderBy(x => x.name.LevenshteinDistance(name)).First(x => x.name == name);
        return await GetGameData(game.appid);

    }

    public async Task<GameResult> GetGameData(int id)
    {
        var key = nameof(GetGameData) + id;

        var result = await _redis.GetString(key, async () =>
        {
            try
            {
                var address = $"https://store.steampowered.com/api/appdetails?appids={id}";
                var result = await _client.GetAsync(address);
                var content = await result.Content.ReadAsStringAsync();
                content = content.Replace("{\"" + id + "\":{\"success", "{\"GameResult\":{\"success");
                Debug.WriteLine(content);
                return content;
            }
            catch (Exception e)
            {
                throw e;
            }
        });
        var gameResult = JsonSerializer.Deserialize<SteamRootobject>(result);
        return gameResult.Game;
    }

    public async Task<IEnumerable<SimpleSteamGame>> GetSimpleGames(string filter, int count = 50)
    {
        // Ensure games are cached in memory
        var allGames = await GetSimpleGamesCached();

        if (string.IsNullOrWhiteSpace(filter))
            return allGames.Take(count);

        return allGames
            .Where(x => x.name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .Take(count);
    }

    public async Task<List<SimpleSteamGame>> GetSimpleGamesCached()
    {
        if (_cachedGames != null)
            return _cachedGames;

        var key = nameof(GetSimpleGames);
        var result = await _redis.GetString(key, async () =>
        {
            try
            {
                var address = "https://api.steampowered.com/ISteamApps/GetAppList/v2/";
                var response = await _client.GetAsync(address);
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                throw;
            }
        });

        var jsonObject = JsonDocument.Parse(result).RootElement;
        var appsArray = jsonObject.GetProperty("applist").GetProperty("apps");

        _cachedGames = new List<SimpleSteamGame>(appsArray.GetArrayLength());
        foreach (var appElement in appsArray.EnumerateArray())
        {
            var name = appElement.GetProperty("name").GetString();
            if (!string.IsNullOrWhiteSpace(name))
            {
                _cachedGames.Add(new SimpleSteamGame
                {
                    appid = appElement.GetProperty("appid").GetInt32(),
                    name = name
                });
            }
        }

        return _cachedGames;
    }


}