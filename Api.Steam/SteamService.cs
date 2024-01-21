using System.Diagnostics;
using System.Text.Json;  
using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Extensions;

namespace PolyhydraGames.Api.Steam;

public class SteamService : ISteamService
{
    private readonly HttpClient _client;
    private readonly ICacheService _redis;

    public SteamService(HttpClient client, ICacheService redis)
    {
        _client = client;
        _redis = redis;
    }

    public async Task<GameResult> GetGameData(string name)
    {
        var result = await GetSimpleGames();
        var game = result.OrderBy(x => x.name.LevenshteinDistance(name)).First(x => x.name == name);
        return await GetGameData(game.appid);

    }

    public async Task<GameResult> GetGameData(int id)
    {
        var key = nameof(GetGameData) + id;

        var result = await _redis.GetString(key, async () =>
        {
            try
            {
                var address = $"https://store.steampowered.com/api/appdetails?appids={id}";
                var result = await _client.GetAsync(address);
                var content = await result.Content.ReadAsStringAsync();
                content = content.Replace("{\"" + id + "\":{\"success", "{\"GameResult\":{\"success");
                Debug.WriteLine(content);
                return content;
            }
            catch (Exception e)
            {
                throw e;
            }
        });
        var gameResult = JsonSerializer.Deserialize<SteamRootobject>(result);
        return gameResult.Game;
    }
    public async Task<IEnumerable<SimpleSteamGame>> GetSimpleGames()
    {
        var key = nameof(GetSimpleGames);

        var result = await _redis.GetString(key, async () =>
        {
            try
            {
                var address = "https://api.steampowered.com/ISteamApps/GetAppList/v2/";
                var result = await _client.GetAsync(address);
                var content = await result.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                throw e;
            }
        });
        var jsonObject = JsonDocument.Parse(result).RootElement;
        var apps = new List<SimpleSteamGame>();
        var appsArray = jsonObject.GetProperty("applist").GetProperty("apps");
        foreach (var appElement in appsArray.EnumerateArray())
        {
            var app = new SimpleSteamGame
            {
                appid = appElement.GetProperty("appid").GetInt32(),
                name = appElement.GetProperty("name").GetString()
            };
            apps.Add(app);
        }
        //var gameResult = JsonSerializer.Deserialize<List<SimpleSteamGame>>(result);
        return apps;
    }

    //public static IGame ToGame(SteamGame result)
    //{
    //    return new Game()
    //    {
    //        // = result.steam_appid, 
    //        Title = result.Name,
    //        Description = result.ShortDescription,
    //        ImageUrl = result.HeaderImage,
    //        BackgroundImageUrl = result.Background,
    //        Platform = "pc",
    //        Year = DateTime.Parse(result.ReleaseDate.date).Year,
    //        Developer = result.Developers.FirstOrDefault(),
          
    //    };
    //}
}

