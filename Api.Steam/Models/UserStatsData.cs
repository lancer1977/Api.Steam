namespace PolyhydraGames.Api.Steam.Models
{
    public record UserStatsData(
        string SteamId,
        string GameName,
        int AppId,
        List<Stat> Stats,
        List<Achievement> Achievements
    );
}