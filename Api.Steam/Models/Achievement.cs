namespace PolyhydraGames.Api.Steam.Models
{
    public record Achievement(string ApiName, int Achieved, long UnlockTime, string? Name, string? Description);
}