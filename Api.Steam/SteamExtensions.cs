using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Interfaces.Gaming;

namespace PolyhydraGames.Api.Steam;

public static class SteamExtensions
{
    public static IGame ToGame(this SteamGameDetail result)
    {
        return new SteamStreamerGame()
        {
            // = result.steam_appid, 
            Title = result.Name,
            Description = result.ShortDescription,
            ImageUrl = result.HeaderImage,
            BackgroundImageUrl = result.Background,
            Platform = "pc",
            Year = DateTime.Parse(result.ReleaseDate.date).Year,
            Developer = result.Developers.FirstOrDefault(),

        };
    }
}