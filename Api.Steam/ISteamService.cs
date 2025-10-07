using PolyhydraGames.Api.Steam.Models;

namespace PolyhydraGames.Api.Steam;

public interface ISteamService
{
    public Task<GameResult> GetGameData(int id, bool forceRefresh = false);
    public Task<GameResult> GetGameData(string id,bool forceRefresh = false); 
    Task<IEnumerable<SimpleSteamGame>> GetSimpleGames(string filter, int count = 50, bool forceRefresh = false);

}