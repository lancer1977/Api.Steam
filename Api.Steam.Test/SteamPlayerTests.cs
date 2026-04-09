using System.Text.Json;
using PolyhydraGames.Api.Steam.Models;

namespace Api.Steam.Test;

[TestFixture]
public class SteamPlayerTests
{
    [Test]
    public void GetPlayerSummariesResponse_Deserializes_StringGameId()
    {
        const string json = """
        {
          "response": {
            "players": [
              {
                "steamid": "76561197962914477",
                "personaname": "Gabe Newell",
                "profileurl": "https://steamcommunity.com/id/gaben",
                "avatar": "https://avatars.example/avatar.jpg",
                "avatarmedium": "https://avatars.example/avatar-medium.jpg",
                "avatarfull": "https://avatars.example/avatar-full.jpg",
                "personastate": 1,
                "communityvisibilitystate": 3,
                "profilestate": 1,
                "lastlogoff": 1710000000,
                "commentpermission": 1,
                "realname": "Gabe Newell",
                "primaryclanid": "103582791429521412",
                "timecreated": 1710000000,
                "gameid": "531510",
                "gameextrainfo": "Team Fortress 2",
                "loccountrycode": "US"
              }
            ]
          }
        }
        """;

        var result = JsonSerializer.Deserialize<GetPlayerSummariesResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Response.Players, Has.Count.EqualTo(1));
        var player = result.Response.Players[0];
        Assert.That(player.GameId, Is.EqualTo("531510"));
        Assert.That(player.GameExtraInfo, Is.EqualTo("Team Fortress 2"));
    }
}
