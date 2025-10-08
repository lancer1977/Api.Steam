using Microsoft.Extensions.Configuration;
using PolyhydraGames.Api.Steam; 

namespace Api.Steam.Test
{
    [TestFixture]
    [Category("Integration")]
    public class SteamWebApiServiceIntegrationTests
    {
        private SteamService _service = null!;
        private string _steamApiKey = null!;
        private const string PublicSteamId = "76561197962914477"; // Valve public account (Gabe Newell)
        private const int TestAppId = 531510; // Team Fortress 2
        [OneTimeSetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<SteamWebApiServiceIntegrationTests>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            _steamApiKey = config["Steam:ApiKey"];

            if (string.IsNullOrWhiteSpace(_steamApiKey))
            {
                Assert.Ignore("Steam API key not set — skipping live integration tests.");
            }

            //_service = new SteamWebApiService(new HttpClient(), _);
        }

        // ------------------------------------------------------------
        // ISteamUser
        // ------------------------------------------------------------
        [Test]
        public async Task GetPlayerSummaries_ShouldReturnPublicProfileData()
        {
            var result = await _service.GetPlayerSummariesAsync(new[] { PublicSteamId });
            Assert.That(result, Is.Not.Null, "No data returned from API.");
            Assert.That(result!.Response.Players, Is.Not.Empty, "Player list should not be empty.");

            var player = result.Response.Players.First();
            TestContext.WriteLine($"Persona: {player.PersonaName}, URL: {player.ProfileUrl}");
            Assert.Multiple(() =>
            {
                Assert.That(player.SteamId, Is.EqualTo(PublicSteamId));
                Assert.That(player.PersonaName, Is.Not.Null.And.Not.Empty);
                Assert.That(player.ProfileUrl, Does.StartWith("http"));
            });
        }

        [Test]
        public async Task GetFriendList_ShouldReturnEmptyOrFriends()
        {
            var result = await _service.GetFriendListAsync(PublicSteamId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FriendsList.Friends, Is.Not.Null);
            TestContext.WriteLine($"Friend count: {result.FriendsList.Friends.Count}");
        }

        // ------------------------------------------------------------
        // IPlayerService
        // ------------------------------------------------------------
        [Test]
        public async Task GetOwnedGames_ShouldReturnAtLeastOneGame()
        {
            var result = await _service.GetOwnedGamesAsync(PublicSteamId, includeAppInfo: true);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Response.Game_Count, Is.GreaterThan(0), "Expected at least one owned game.");

            var firstGame = result.Response.Games.First();
            TestContext.WriteLine($"First Game: {firstGame.Name} ({firstGame.AppId})");
            Assert.Multiple(() =>
            {
                Assert.That(firstGame.AppId, Is.GreaterThan(0));
                Assert.That(firstGame.Name, Is.Not.Null.And.Not.Empty);
            });
        }

        [Test]
        public async Task GetRecentlyPlayedGames_ShouldReturnValidResult()
        {
            var result = await _service.GetRecentlyPlayedGamesAsync(PublicSteamId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Response.Total_Count, Is.GreaterThanOrEqualTo(0));
            TestContext.WriteLine($"Recently played count: {result.Response.Total_Count}");
        }

        // ------------------------------------------------------------
        // ISteamUserStats
        // ------------------------------------------------------------
        [Test]
        public async Task GetPlayerAchievements_ShouldReturnData()
        {
            var result = await _service.GetPlayerAchievementsAsync(PublicSteamId, TestAppId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PlayerStats.AppId, Is.EqualTo(TestAppId));
            Assert.That(result.PlayerStats.Achievements, Is.Not.Null);

            var first = result.PlayerStats.Achievements.FirstOrDefault();
            if (first != null)
                TestContext.WriteLine($"Achievement: {first.Name} - Achieved: {first.Achieved}");
        }

        [Test]
        public async Task GetUserStatsForGame_ShouldReturnStats()
        {
            var result = await _service.GetUserStatsForGameAsync(PublicSteamId, TestAppId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PlayerStats.AppId, Is.EqualTo(TestAppId));
            TestContext.WriteLine($"Stat count: {result.PlayerStats.Stats?.Count ?? 0}");
        }

        [Test]
        public async Task GetGlobalAchievementPercentagesForApp_ShouldReturnAchievements()
        {
            var result = await _service.GetGlobalAchievementPercentagesForAppAsync(TestAppId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.AchievementPercentages.Achievements, Is.Not.Empty);

            var first = result.AchievementPercentages.Achievements.First();
            TestContext.WriteLine($"Achievement: {first.Name}, {first.Percent}% of players");
        }

        // ------------------------------------------------------------
        // ISteamNews
        // ------------------------------------------------------------
        [Test]
        public async Task GetNewsForApp_ShouldReturnArticles()
        {
            var result = await _service.GetNewsForAppAsync(TestAppId, 3, 300);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.AppNews.NewsItems, Is.Not.Empty);

            var first = result.AppNews.NewsItems.First();
            TestContext.WriteLine($"News: {first.Title} ({first.Url})");
        }

        // ------------------------------------------------------------
        // ISteamWebAPIUtil
        // ------------------------------------------------------------
        [Test]
        public async Task GetSupportedAPIList_ShouldReturnInterfaces()
        {
            var result = await _service.GetSupportedAPIListAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.apilist.interfaces, Is.Not.Empty, "Expected list of supported interfaces.");

            var firstInterface = result.apilist.interfaces.First();
            TestContext.WriteLine($"First interface: {firstInterface.name}");
            TestContext.WriteLine($"Methods: {firstInterface.methods.Count}");
        }
    }
}