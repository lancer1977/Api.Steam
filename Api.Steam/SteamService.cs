using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Extensions;
using System.Diagnostics;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PolyhydraGames.Api.Steam;
/// <summary>
/// High-level Steam service providing cached access to app lists and app details.
/// </summary>
public class SteamService : ISteamService
{
    private readonly HttpClient _client;
    private readonly ICacheService _cache;
    private readonly ISteamServiceConfiguration _config;
    private List<SimpleSteamGame>? _cachedGames;
    private string ApiKey => _config.ApiKey;
    private const string BaseUrl = "https://api.steampowered.com/";
    public SteamService(HttpClient client, ICacheService cache, ISteamServiceConfiguration config)
    {
        _client = client;
        _cache = cache;
        _config = config;
    }

    // ------------------------------------------------------------------------
    //  GetGameData (by name)
    // ------------------------------------------------------------------------
    public async Task<GameResult> GetGameData(string name, bool forceRefresh = false)
    {
        var allGames = await GetSimpleGamesCached(forceRefresh);
        var game = allGames.OrderBy(x => x.Name.LevenshteinDistance(name))
            .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (game == null)
            throw new InvalidOperationException($"Game '{name}' not found in Steam app list.");

        return await GetGameData(game.AppId, forceRefresh);
    }

    // ------------------------------------------------------------------------
    //  GetGameData (by appid)
    // ------------------------------------------------------------------------
    public async Task<GameResult> GetGameData(int id, bool forceRefresh = false)
    {
        var key = $"{nameof(GetGameData)}:{id}";
        var ttl = _config.AppDetailsTtl;

        var json = await _cache.GetString(key, async () =>
        {
            var address = $"https://store.steampowered.com/api/appdetails?appids={id}";
            var result = await _client.GetAsync(address);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            // Adjust structure to deserialize easily
            content = content.Replace("{\"" + id + "\":{\"success", "{\"GameResult\":{\"success");
            Debug.WriteLine($"Fetched app {id} from Steam API.");
            return content;
        }, ttl, forceRefresh);

        var root = JsonSerializer.Deserialize<SteamRootobject>(json);
        return root?.Game ?? throw new InvalidOperationException("Failed to deserialize Steam app details.");
    }

    // ------------------------------------------------------------------------
    //  GetSimpleGames (filtered)
    // ------------------------------------------------------------------------
    public async Task<IEnumerable<SimpleSteamGame>> GetSimpleGames(string filter, int count = 50, bool forceRefresh = false)
    {
        var allGames = await GetSimpleGamesCached(forceRefresh);

        if (string.IsNullOrWhiteSpace(filter))
            return allGames.Take(count);

        return allGames
            .Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .Take(count);
    }

    // ------------------------------------------------------------------------
    //  GetSimpleGamesCached (cached list)
    // ------------------------------------------------------------------------
    public async Task<List<SimpleSteamGame>> GetSimpleGamesCached(bool forceRefresh = false)
    {
        // Use in-memory cache first for efficiency
        if (!forceRefresh && _cachedGames != null)
            return _cachedGames;

        var key = nameof(GetSimpleGamesCached);
        var ttl = _config.AppListTtl;

        var result = await _cache.GetString(key, async () =>
        {
            var address = "https://api.steampowered.com/ISteamApps/GetAppList/v2/";
            var response = await _client.GetAsync(address);
            response.EnsureSuccessStatusCode();
            Debug.WriteLine("Fetched app list from Steam API.");
            return await response.Content.ReadAsStringAsync();
        }, ttl, forceRefresh);

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
                    AppId = appElement.GetProperty("appid").GetInt32(),
                    Name = name
                });
            }
        }

        return _cachedGames;
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        var response = await _cache.Get<HttpResponseMessage>(url, () => _client.GetAsync(url), _config.DefaultTtl);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // ------------------------------------------------------------
    //  ISteamNews
    // ------------------------------------------------------------
    public async Task<GetNewsForAppResponse?> GetNewsForAppAsync(int appId, int count = 3, int maxLength = 300)
    {
        var url = $"{BaseUrl}ISteamNews/GetNewsForApp/v0002/?appid={appId}&count={count}&maxlength={maxLength}&format=json";
        return await GetAsync<GetNewsForAppResponse>(url);
    }

    // ------------------------------------------------------------
    //  ISteamUserStats
    // ------------------------------------------------------------
    public async Task<GetGlobalAchievementPercentagesResponse?> GetGlobalAchievementPercentagesForAppAsync(int appId)
    {
        var url = $"{BaseUrl}ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid={appId}&format=json";
        return await GetAsync<GetGlobalAchievementPercentagesResponse>(url);
    }

    public async Task<GetPlayerAchievementsResponse?> GetPlayerAchievementsAsync(string steamId, int appId, string language = "en")
    {
        var url = $"{BaseUrl}ISteamUserStats/GetPlayerAchievements/v0001/?key={ApiKey}&steamid={steamId}&appid={appId}&l={language}&format=json";
        return await GetAsync<GetPlayerAchievementsResponse>(url);
    }

    public async Task<GetUserStatsForGameResponse?> GetUserStatsForGameAsync(string steamId, int appId, string language = "en")
    {
        var url = $"{BaseUrl}ISteamUserStats/GetUserStatsForGame/v0002/?key={ApiKey}&steamid={steamId}&appid={appId}&l={language}&format=json";
        return await GetAsync<GetUserStatsForGameResponse>(url);
    }

    // ------------------------------------------------------------
    //  ISteamUser
    // ------------------------------------------------------------
    public async Task<GetPlayerSummariesResponse?> GetPlayerSummariesAsync(IEnumerable<string> steamIds)
    {
        var ids = string.Join(",", steamIds);
        var url = $"{BaseUrl}ISteamUser/GetPlayerSummaries/v0002/?key={ApiKey}&steamids={ids}&format=json";
        return await GetAsync<GetPlayerSummariesResponse>(url);
    }

    public async Task<GetFriendListResponse?> GetFriendListAsync(string steamId, string relationship = "friend")
    {
        var url = $"{BaseUrl}ISteamUser/GetFriendList/v0001/?key={ApiKey}&steamid={steamId}&relationship={relationship}&format=json";
        return await GetAsync<GetFriendListResponse>(url);
    }

    // ------------------------------------------------------------
    //  IPlayerService
    // ------------------------------------------------------------
    public async Task<GetOwnedGamesResponse?> GetOwnedGamesAsync(string steamId, bool includeAppInfo = true, bool includePlayedFreeGames = false)
    {
        var url = $"{BaseUrl}IPlayerService/GetOwnedGames/v0001/?key={ApiKey}&steamid={steamId}&include_appinfo={(includeAppInfo ? 1 : 0)}&include_played_free_games={(includePlayedFreeGames ? 1 : 0)}&format=json";
        return await GetAsync<GetOwnedGamesResponse>(url);
    }

    public async Task<GetRecentlyPlayedGamesResponse?> GetRecentlyPlayedGamesAsync(string steamId, int count = 0)
    {
        var url = $"{BaseUrl}IPlayerService/GetRecentlyPlayedGames/v0001/?key={ApiKey}&steamid={steamId}&format=json";
        if (count > 0)
            url += $"&count={count}";
        return await GetAsync<GetRecentlyPlayedGamesResponse>(url);
    }

    // ------------------------------------------------------------
    //  ISteamWebAPIUtil
    // ------------------------------------------------------------
    public async Task<GetSupportedAPIListResponse?> GetSupportedAPIListAsync()
    {
        var url = $"{BaseUrl}ISteamWebAPIUtil/GetSupportedAPIList/v0001/?key={ApiKey}&format=json";
        return await GetAsync<GetSupportedAPIListResponse>(url);
    }
}
