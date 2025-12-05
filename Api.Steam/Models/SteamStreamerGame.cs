using PolyhydraGames.Core.Interfaces.Gaming;

namespace PolyhydraGames.Api.Steam.Models;

public class SteamStreamerGame : IGame
{
    public string Title { get; set; }
    public string Core { get; set; }
    public string Country { get; set; }
    public string Path { get; set; }
    public string Filename { get; set; }
    public string Platform { get; set; }
    public string Description { get; set; }
    public string Developer { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
    public string BackgroundImageUrl { get; set; }
    public string Slug { get; set; }
    public string MediaUrl { get; set; }
    public IList<IFileRecord> Media { get; set; }
}