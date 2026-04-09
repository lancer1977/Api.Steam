using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Extensions;

namespace Api.Steam.Test;

[TestFixture]
public class SteamServiceTests
{
    [Test]
    public async Task GetGameData_ById_ParsesStorePayload()
    {
        var harness = new SteamServiceTestHarness();
        harness.Handler.WhenContains(
            "store.steampowered.com/api/appdetails?appids=346110",
            """{"346110":{"success":true,"data":{"steam_appid":346110,"name":"ARK: Survival Evolved","short_description":"Survive the island","header_image":"https://cdn.example/ark.jpg","background":"https://cdn.example/ark-bg.jpg","release_date":{"coming_soon":false,"date":"Aug 27, 2017"},"developers":["Studio Wildcard"]}}}""");

        var result = await harness.Service.GetGameData(346110);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data.SteamAppId, Is.EqualTo(346110));
        Assert.That(result.Data.Name, Is.EqualTo("ARK: Survival Evolved"));
        Assert.That(result.Data.ShortDescription, Is.EqualTo("Survive the island"));
        Assert.That(result.Data.ReleaseDate.date, Is.EqualTo("Aug 27, 2017"));
        Assert.That(harness.Handler.Requests, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetGameData_ByName_ResolvesTheAppListThenDetails()
    {
        var harness = new SteamServiceTestHarness();
        harness.Handler.WhenContains(
            "IStoreService/GetAppList/v2",
            """
            {"applist":{"apps":[
              {"appid":1353230,"name":"Bomb Rush Cyberfunk"},
              {"appid":346110,"name":"ARK: Survival Evolved"}
            ]}}
            """);
        harness.Handler.WhenContains(
            "store.steampowered.com/api/appdetails?appids=1353230",
            """{"1353230":{"success":true,"data":{"steam_appid":1353230,"name":"Bomb Rush Cyberfunk","short_description":"Inline skating chaos","header_image":"https://cdn.example/brc.jpg","background":"https://cdn.example/brc-bg.jpg","release_date":{"coming_soon":false,"date":"Aug 18, 2023"},"developers":["Team Reptile"]}}}""");

        var result = await harness.Service.GetGameData("Bomb Rush Cyberfunk");

        Assert.That(result.Data.SteamAppId, Is.EqualTo(1353230));
        Assert.That(result.Data.Name, Is.EqualTo("Bomb Rush Cyberfunk"));
        Assert.That(harness.Handler.Requests, Has.Count.EqualTo(2));
        Assert.That(harness.Handler.Requests[0].AbsoluteUri, Does.Contain("GetAppList/v2"));
        Assert.That(harness.Handler.Requests[1].AbsoluteUri, Does.Contain("appdetails?appids=1353230"));
    }

    [Test]
    public async Task GetSimpleGames_FiltersNamesAndRespectsCount()
    {
        var harness = new SteamServiceTestHarness();
        harness.Handler.WhenContains(
            "IStoreService/GetAppList/v2",
            """
            {"applist":{"apps":[
              {"appid":1353230,"name":"Bomb Rush Cyberfunk"},
              {"appid":346110,"name":"ARK: Survival Evolved"},
              {"appid":0,"name":""}
            ]}}
            """);

        var games = (await harness.Service.GetSimpleGames("ark", count: 1)).ToList();

        Assert.That(games, Has.Count.EqualTo(1));
        Assert.That(games[0].AppId, Is.EqualTo(346110));
        Assert.That(games[0].Name, Is.EqualTo("ARK: Survival Evolved"));
    }

    [Test]
    public async Task GetSimpleGamesCached_UsesInMemoryCacheAfterFirstFetch()
    {
        var harness = new SteamServiceTestHarness();
        harness.Handler.WhenContains(
            "IStoreService/GetAppList/v2",
            """
            {"applist":{"apps":[
              {"appid":1353230,"name":"Bomb Rush Cyberfunk"},
              {"appid":346110,"name":"ARK: Survival Evolved"}
            ]}}
            """);

        var first = await harness.Service.GetSimpleGamesCached();
        var second = await harness.Service.GetSimpleGamesCached();

        Assert.That(first, Has.Count.EqualTo(2));
        Assert.That(second, Has.Count.EqualTo(2));
        Assert.That(harness.Handler.Requests, Has.Count.EqualTo(1));
    }
}
