using PolyhydraGames.Api.Steam.Models;

namespace Api.Steam.Test;

[TestFixture]
public class SteamServiceTests_new
{
    private static SteamServiceTestHarness CreateHarness() => new();

    [Test]
    public async Task GetPlayerSummariesAsync_ReturnsProfilesAndUsesSteamIdsQuery()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamUser/GetPlayerSummaries/v0002",
            """
            {"response":{"players":[
              {"steamid":"76561197962914477","personaname":"Gabe Newell","profileurl":"https://steamcommunity.com/id/gaben","avatar":"https://avatars.example/avatar.jpg","gameid":"531510","gameextrainfo":"Team Fortress 2"}
            ]}}
            """);

        var result = await harness.Service.GetPlayerSummariesAsync(new[] { "76561197962914477" });

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Response.Players, Has.Count.EqualTo(1));
        Assert.That(result.Response.Players[0].SteamId, Is.EqualTo("76561197962914477"));
        Assert.That(result.Response.Players[0].PersonaName, Is.EqualTo("Gabe Newell"));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("steamids=76561197962914477"));
    }

    [Test]
    public async Task GetFriendListAsync_ReturnsFriendEntries()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamUser/GetFriendList/v0001",
            """
            {"friendslist":{"friends":[
              {"steamid":"111","relationship":"friend","friend_since":1710000000}
            ]}}
            """);

        var result = await harness.Service.GetFriendListAsync("76561197962914477");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.FriendsList.Friends, Has.Count.EqualTo(1));
        Assert.That(result.FriendsList.Friends[0].SteamId, Is.EqualTo("111"));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("relationship=friend"));
    }

    [Test]
    public async Task GetOwnedGamesAsync_ReturnsLibraryAndAppInfoQuery()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "IPlayerService/GetOwnedGames/v0001",
            """
            {"response":{"game_count":1,"games":[
              {"appid":346110,"name":"ARK: Survival Evolved","playtime_forever":123,"playtime_2weeks":2,"img_icon_url":"icon","img_logo_url":"logo","has_community_visible_stats":true}
            ]}}
            """);

        var result = await harness.Service.GetOwnedGamesAsync("76561197962914477", includeAppInfo: true, includePlayedFreeGames: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Response.Game_Count, Is.EqualTo(1));
        Assert.That(result.Response.Games[0].AppId, Is.EqualTo(346110));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("include_appinfo=1"));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("include_played_free_games=1"));
    }

    [Test]
    public async Task GetRecentlyPlayedGamesAsync_UsesCountWhenProvided()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "IPlayerService/GetRecentlyPlayedGames/v0001",
            """
            {"response":{"total_count":1,"games":[
              {"appid":346110,"name":"ARK: Survival Evolved","playtime_forever":456,"playtime_2weeks":10,"img_icon_url":"icon","img_logo_url":"logo","has_community_visible_stats":true}
            ]}}
            """);

        var result = await harness.Service.GetRecentlyPlayedGamesAsync("76561197962914477", count: 3);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Response.Total_Count, Is.EqualTo(1));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("count=3"));
    }

    [Test]
    public async Task GetPlayerAchievementsAsync_ReturnsAchievementPayload()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamUserStats/GetPlayerAchievements/v0001",
            """
            {"playerstats":{"steamid":"76561197962914477","gamename":"Team Fortress 2","appid":531510,"achievements":[
              {"apiname":"TF2_WIN","achieved":1,"unlocktime":1710000000,"name":"Win a Match","description":"Win a match"}
            ]}}
            """);

        var result = await harness.Service.GetPlayerAchievementsAsync("76561197962914477", 531510);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.PlayerStats.AppId, Is.EqualTo(531510));
        Assert.That(result.PlayerStats.Achievements, Has.Count.EqualTo(1));
        Assert.That(result.PlayerStats.Achievements[0].ApiName, Is.EqualTo("TF2_WIN"));
    }

    [Test]
    public async Task GetUserStatsForGameAsync_ReturnsStatsPayload()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamUserStats/GetUserStatsForGame/v0002",
            """
            {"playerstats":{"steamid":"76561197962914477","gamename":"Team Fortress 2","appid":531510,"stats":[
              {"name":"kills","value":12}
            ],"achievements":[
              {"apiname":"TF2_WIN","achieved":1,"unlocktime":1710000000,"name":"Win a Match","description":"Win a match"}
            ]}}
            """);

        var result = await harness.Service.GetUserStatsForGameAsync("76561197962914477", 531510);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.PlayerStats.AppId, Is.EqualTo(531510));
        Assert.That(result.PlayerStats.Stats, Has.Count.EqualTo(1));
        Assert.That(result.PlayerStats.Stats[0].Name, Is.EqualTo("kills"));
    }

    [Test]
    public async Task GetGlobalAchievementPercentagesForAppAsync_ReturnsPercentages()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002",
            """
            {"achievementpercentages":{"achievements":[
              {"name":"TF2_WIN","percent":12.5}
            ]}}
            """);

        var result = await harness.Service.GetGlobalAchievementPercentagesForAppAsync(531510);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.AchievementPercentages.Achievements, Has.Count.EqualTo(1));
        Assert.That(result.AchievementPercentages.Achievements[0].Name, Is.EqualTo("TF2_WIN"));
        Assert.That(harness.Handler.Requests.Single().AbsoluteUri, Does.Contain("gameid=531510"));
    }

    [Test]
    public async Task GetNewsForAppAsync_ReturnsArticles()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamNews/GetNewsForApp/v0002",
            """
            {"appnews":{"appid":531510,"newsitems":[
              {"gid":"1","title":"Patch Notes","url":"https://example.com","contents":"Updated","feedlabel":"Steam","date":1710000000}
            ]}}
            """);

        var result = await harness.Service.GetNewsForAppAsync(531510, 3, 300);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.AppNews.AppId, Is.EqualTo(531510));
        Assert.That(result.AppNews.NewsItems, Has.Count.EqualTo(1));
        Assert.That(result.AppNews.NewsItems[0].Title, Is.EqualTo("Patch Notes"));
    }

    [Test]
    public async Task GetSupportedAPIListAsync_ReturnsInterfaces()
    {
        var harness = CreateHarness();
        harness.Handler.WhenContains(
            "ISteamWebAPIUtil/GetSupportedAPIList/v0001",
            """
            {"apilist":{"interfaces":[
              {"name":"ISteamUser","methods":[{"name":"GetPlayerSummaries","version":2,"httpmethod":"GET","description":"Player summaries"}]}
            ]}}
            """);

        var result = await harness.Service.GetSupportedAPIListAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.apilist.interfaces, Has.Count.EqualTo(1));
        Assert.That(result.apilist.interfaces[0].name, Is.EqualTo("ISteamUser"));
        Assert.That(result.apilist.interfaces[0].methods[0].name, Is.EqualTo("GetPlayerSummaries"));
    }
}
