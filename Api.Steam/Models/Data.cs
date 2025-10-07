using System.Text.Json.Serialization;

namespace PolyhydraGames.Api.Steam.Models;

public record SteamGame(
    [property: JsonPropertyName("appid")] int AppId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("playtime_forever")] int PlaytimeForever,
    [property: JsonPropertyName("playtime_2weeks")] int? Playtime2Weeks,
    [property: JsonPropertyName("img_icon_url")] string? IconUrl,
    [property: JsonPropertyName("img_logo_url")] string? LogoUrl,
    [property: JsonPropertyName("has_community_visible_stats")] bool? HasStats
);