using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolyhydraGames.Api.Steam;
using PolyhydraGames.Core.Interfaces;
using StackExchange.Redis;

namespace Api.Steam.Test
{
    [TestFixture]
    public class SteamServiceTests_new
    {
#pragma warning disable NUnit1032
        private IHost? _host = null!;
#pragma warning restore NUnit1032
        private ISteamService _steamService = null!;
        private const string Endpoint = "192.168.0.21:6379"; // Your Redis instance

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            _host = Fixture.Create((services, config) =>
            {
                services.AddSingleton(new HttpClient());
                services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(Endpoint));
                services.AddSingleton<ICacheService, FakeCacheService>();
                services.AddSingleton<ISteamService, SteamService>();
            });

            _steamService = _host.Services.GetRequiredService<ISteamService>();
            await Task.CompletedTask;
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            if (_host is not null)
                await _host.StopAsync();
        }

        [TestCase(1353230, ExpectedResult = "Bomb Rush Cyberfunk")]
        [TestCase(346110, ExpectedResult = "ARK: Survival Evolved")]
        public async Task<string> GetBackgroundFolderRecords_ById_ShouldReturnExpectedName(int id)
        {
            var app = await _steamService.GetGameData(id);
            Assert.That(app.Data.Name, Is.Not.Null.And.Not.Empty);
            return app.Data.Name;
        }

        [TestCase("Bomb Rush Cyberfunk", ExpectedResult = 1353230)]
        [TestCase("ARK: Survival Evolved", ExpectedResult = 346110)]
        public async Task<int> GetBackgroundFolderRecords_ByName_ShouldReturnExpectedId(string name)
        {
            var app = await _steamService.GetGameData(name);
            Assert.That(app.Data.AppId, Is.GreaterThan(0));
            return app.Data.AppId;
        }

        [Test]
        public async Task GetSimpleGames_ShouldReturnList()
        {
            var app = await _steamService.GetSimpleGames("");
            Assert.That(app, Is.Not.Null);
            Assert.That(app.Any(), Is.True);
        }

        private static void WriteLine(string value)
        {
            Console.WriteLine(value);
            Debug.WriteLine(value);
        }
    }
}