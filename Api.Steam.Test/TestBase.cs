using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolyhydraGames.Api.Steam;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Test;
using StackExchange.Redis;

namespace Api.Steam.Test;

[TestFixture]
public class SteamServiceTests
{
    ISteamService _steamService;

#pragma warning disable NUnit1032
    private readonly IHost _host;
#pragma warning restore NUnit1032

    //[TearDown]
    //public void TearDown()
    //{
    //    _host.Dispose();
    //}
    public SteamServiceTests()
    {

        _host = TestFixtures.GetHost((ctx, services) =>
        {
            services.AddSingleton(new HttpClient());

            services.AddSingleton<IConnectionMultiplexer>((x) => ConnectionMultiplexer.Connect("redis.polyhydragames.com"));
            services.AddSingleton<ICacheService, FakeCacheService>();
            services.AddSingleton<ISteamService, SteamService>();
        });
        _steamService = _host.Services.GetService<ISteamService>();
    }

    [SetUp]
    public async Task Setup()
    {

        _steamService = _host.Services.GetRequiredService<ISteamService>();
    }

    [TestCase(1353230, ExpectedResult = "Bomb Rush Cyberfunk"),
        TestCase(346110, ExpectedResult = "ARK: Survival Evolved")]
    public async Task<string> GetBackgroundFolderRecords(int id)
    {
        var app = await _steamService.GetGameData(id);
        return app.Data.Name;

    }

    [TestCase("Bomb Rush Cyberfunk", ExpectedResult = 1353230),
     TestCase("ARK: Survival Evolved", ExpectedResult = 346110)]
    public async Task<int> GetBackgroundFolderRecords(string name)
    {
        var app = await _steamService.GetGameData(name);
        return app.Data.SteamAppId;

    }

    [Test]
    public async Task GetSimpleGames()
    {
        var app = await _steamService.GetSimpleGames();
        Assert.That(app.Any());
    }
}