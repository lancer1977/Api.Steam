namespace PolyhydraGames.Api.Steam.Models
{
    public record PlayerAchievementsData(
        string SteamId,
        string GameName,
        int AppId,
        List<Achievement> Achievements
    );
}