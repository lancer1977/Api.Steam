using System.Text.Json.Serialization;

namespace PolyhydraGames.Api.Steam.Models;

public class SteamGame
{
    [JsonPropertyName("header_image")]
    public string HeaderImage { get; set; }

    [JsonPropertyName("capsule_image")]
    public string CapsuleImage { get; set; }

    [JsonPropertyName("capsule_imagev5")]
    public string CapsuleImageV5 { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("steam_appid")]
    public int SteamAppId { get; set; }

    [JsonPropertyName("required_age")]
    public string? RequiredAge { get; set; }

    [JsonPropertyName("is_free")]
    public bool IsFree { get; set; }

    [JsonPropertyName("controller_support")]
    public string ControllerSupport { get; set; }

    [JsonPropertyName("dlc")]
    public int[] Dlc { get; set; }

    [JsonPropertyName("detailed_description")]
    public string DetailedDescription { get; set; }

    [JsonPropertyName("about_the_game")]
    public string AboutTheGame { get; set; }

    [JsonPropertyName("short_description")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("supported_languages")]
    public string SupportedLanguages { get; set; }



    [JsonPropertyName("website")]
    public string Website { get; set; }

    [JsonPropertyName("pc_requirements")]
    public Requirements PcRequirements { get; set; }

    [JsonPropertyName("mac_requirements")]
    public dynamic MacRequirements { get; set; }

    [JsonPropertyName("linux_requirements")]
    public dynamic LinuxRequirements { get; set; }

    [JsonPropertyName("developers")]
    public string[] Developers { get; set; }

    [JsonPropertyName("publishers")]
    public string[] Publishers { get; set; }

    [JsonPropertyName("price_overview")]
    public Price_Overview PriceOverview { get; set; }

    [JsonPropertyName("packages")]
    public int[] Packages { get; set; }

    [JsonPropertyName("package_groups")]
    public Package_Groups[] PackageGroups { get; set; }

    [JsonPropertyName("platforms")]
    public Platforms Platforms { get; set; }

    [JsonPropertyName("metacritic")]
    public Metacritic Metacritic { get; set; }

    [JsonPropertyName("categories")]
    public Category[] Categories { get; set; }

    [JsonPropertyName("genres")]
    public Genre[] Genres { get; set; }

    [JsonPropertyName("screenshots")]
    public Screenshot[] Screenshots { get; set; }

    [JsonPropertyName("movies")]
    public Movie[] Movies { get; set; }

    [JsonPropertyName("recommendations")]
    public Recommendations Recommendations { get; set; }

    [JsonPropertyName("achievements")]
    public Achievements Achievements { get; set; }

    [JsonPropertyName("release_date")]
    public Release_Date ReleaseDate { get; set; }

    [JsonPropertyName("support_info")]
    public Support_Info SupportInfo { get; set; }

    [JsonPropertyName("background")]
    public string Background { get; set; }

    [JsonPropertyName("background_raw")]
    public string BackgroundRaw { get; set; }

    [JsonPropertyName("content_descriptors")]
    public Content_Descriptors ContentDescriptors { get; set; }
}