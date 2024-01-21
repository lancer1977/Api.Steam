using PolyhydraGames.Api.Steam.Models;

namespace PolyhydraGames.Api.Steam;

public interface ISteamService
{
    public Task<GameResult> GetGameData(int id);
    public Task<GameResult> GetGameData(string id);
    Task<IEnumerable<SimpleSteamGame>> GetSimpleGames();

}