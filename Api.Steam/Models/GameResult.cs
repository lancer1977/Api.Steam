using System.Text.Json.Serialization;

namespace PolyhydraGames.Api.Steam.Models;

public class GameResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("data")]
    public SteamGame Data { get; set; }
}

public class SimpleSteamGame
{
    public int appid { get; set; }
    public string name { get; set; }
}