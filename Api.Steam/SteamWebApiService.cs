using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using System.Text.Json;

namespace PolyhydraGames.Api.Steam;

public class SteamWebApiService
{
    private readonly ICacheService _cache;
    private readonly ISteamServiceConfiguration _config;
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.steampowered.com/";

    public SteamWebApiService(HttpClient client, ICacheService cache, ISteamServiceConfiguration config)
    {
        _cache = cache;
        _config = config;
        _apiKey = config.ApiKey;
        _http = client ?? new HttpClient();
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        var response = await _cache.Get<HttpResponseMessage>(url, () => _http.GetAsync(url), _config.DefaultTtl);
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
        var url = $"{BaseUrl}ISteamUserStats/GetPlayerAchievements/v0001/?key={_apiKey}&steamid={steamId}&appid={appId}&l={language}&format=json";
        return await GetAsync<GetPlayerAchievementsResponse>(url);
    }

    public async Task<GetUserStatsForGameResponse?> GetUserStatsForGameAsync(string steamId, int appId, string language = "en")
    {
        var url = $"{BaseUrl}ISteamUserStats/GetUserStatsForGame/v0002/?key={_apiKey}&steamid={steamId}&appid={appId}&l={language}&format=json";
        return await GetAsync<GetUserStatsForGameResponse>(url);
    }

    // ------------------------------------------------------------
    //  ISteamUser
    // ------------------------------------------------------------
    public async Task<GetPlayerSummariesResponse?> GetPlayerSummariesAsync(IEnumerable<string> steamIds)
    {
        var ids = string.Join(",", steamIds);
        var url = $"{BaseUrl}ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey}&steamids={ids}&format=json";
        return await GetAsync<GetPlayerSummariesResponse>(url);
    }

    public async Task<GetFriendListResponse?> GetFriendListAsync(string steamId, string relationship = "friend")
    {
        var url = $"{BaseUrl}ISteamUser/GetFriendList/v0001/?key={_apiKey}&steamid={steamId}&relationship={relationship}&format=json";
        return await GetAsync<GetFriendListResponse>(url);
    }

    // ------------------------------------------------------------
    //  IPlayerService
    // ------------------------------------------------------------
    public async Task<GetOwnedGamesResponse?> GetOwnedGamesAsync(string steamId, bool includeAppInfo = true, bool includePlayedFreeGames = false)
    {
        var url = $"{BaseUrl}IPlayerService/GetOwnedGames/v0001/?key={_apiKey}&steamid={steamId}&include_appinfo={(includeAppInfo ? 1 : 0)}&include_played_free_games={(includePlayedFreeGames ? 1 : 0)}&format=json";
        return await GetAsync<GetOwnedGamesResponse>(url);
    }

    public async Task<GetRecentlyPlayedGamesResponse?> GetRecentlyPlayedGamesAsync(string steamId, int count = 0)
    {
        var url = $"{BaseUrl}IPlayerService/GetRecentlyPlayedGames/v0001/?key={_apiKey}&steamid={steamId}&format=json";
        if (count > 0)
            url += $"&count={count}";
        return await GetAsync<GetRecentlyPlayedGamesResponse>(url);
    }

    // ------------------------------------------------------------
    //  ISteamWebAPIUtil
    // ------------------------------------------------------------
    public async Task<GetSupportedAPIListResponse?> GetSupportedAPIListAsync()
    {
        var url = $"{BaseUrl}ISteamWebAPIUtil/GetSupportedAPIList/v0001/?key={_apiKey}&format=json";
        return await GetAsync<GetSupportedAPIListResponse>(url);
    }
}
