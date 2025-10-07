using System.Text.Json.Serialization;

namespace PolyhydraGames.Api.Steam.Models;

public class SteamRootobject
{
    [JsonPropertyName("GameResult")]
    public GameResult Game { get; set; }
}