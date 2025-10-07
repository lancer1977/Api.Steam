namespace PolyhydraGames.Api.Steam.Models
{
    public record ResponseData(int Game_Count, List<SteamGame> Games);
}