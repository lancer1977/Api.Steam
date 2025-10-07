using System.Text.Json.Serialization;

namespace PolyhydraGames.Api.Steam.Models;

public class GameResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("data")]
    public SteamGameDetail Data { get; set; }
}