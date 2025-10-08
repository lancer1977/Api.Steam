using PolyhydraGames.Api.Steam.Models;

namespace PolyhydraGames.Api.Steam;

public interface ISteamService
{

    // ------------------------------------------------------------------------
    //  Game Data
    // ------------------------------------------------------------------------
    Task<GameResult> GetGameData(string name, bool forceRefresh = false);
    Task<GameResult> GetGameData(int id, bool forceRefresh = false);

    // ------------------------------------------------------------------------
    //  App List
    // ------------------------------------------------------------------------
    Task<IEnumerable<SimpleSteamGame>> GetSimpleGames(string filter, int count = 50, bool forceRefresh = false);
    Task<List<SimpleSteamGame>> GetSimpleGamesCached(bool forceRefresh = false);

    // ------------------------------------------------------------------------
    //  ISteamNews
    // ------------------------------------------------------------------------
    Task<GetNewsForAppResponse?> GetNewsForAppAsync(int appId, int count = 3, int maxLength = 300);

    // ------------------------------------------------------------------------
    //  ISteamUserStats
    // ------------------------------------------------------------------------
    Task<GetGlobalAchievementPercentagesResponse?> GetGlobalAchievementPercentagesForAppAsync(int appId);
    Task<GetPlayerAchievementsResponse?> GetPlayerAchievementsAsync(string steamId, int appId, string language = "en");
    Task<GetUserStatsForGameResponse?> GetUserStatsForGameAsync(string steamId, int appId, string language = "en");

    // ------------------------------------------------------------------------
    //  ISteamUser
    // ------------------------------------------------------------------------
    Task<GetPlayerSummariesResponse?> GetPlayerSummariesAsync(IEnumerable<string> steamIds);
    Task<GetFriendListResponse?> GetFriendListAsync(string steamId, string relationship = "friend");

    // ------------------------------------------------------------------------
    //  IPlayerService
    // ------------------------------------------------------------------------
    Task<GetOwnedGamesResponse?> GetOwnedGamesAsync(string steamId, bool includeAppInfo = true, bool includePlayedFreeGames = false);
    Task<GetRecentlyPlayedGamesResponse?> GetRecentlyPlayedGamesAsync(string steamId, int count = 0);

    // ------------------------------------------------------------------------
    //  ISteamWebAPIUtil
    // ------------------------------------------------------------------------
    Task<GetSupportedAPIListResponse?> GetSupportedAPIListAsync();
}

