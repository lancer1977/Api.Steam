using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolyhydraGames.Api.Steam;
using PolyhydraGames.Api.Steam.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Test;
using StackExchange.Redis;
using System.Diagnostics;
using PolyhydraGames.Core.Models;
using PolyhydraGames.Extensions;
using PolyhydraGames.Streaming.SQL.Models;
using PolyhydraGames.Streaming.Test;

namespace Api.Steam.Test;

[TestFixture]
public class SteamServiceTests
{
    ISteamService _steamService;

#pragma warning disable NUnit1032
    private readonly IHost _host;
#pragma warning restore NUnit1032

    private const string Endpoint = "192.168.0.21:6379";
    //"redis.polyhydragames.com"
    public SteamServiceTests()
    {

        _host = Fixture.Create((services) =>
        {
            services.AddSingleton(new HttpClient());
            services.AddSingleton<IConnectionMultiplexer>((x) => ConnectionMultiplexer.Connect(Endpoint));
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



    public async Task<SimpleSteamGame> GetGameData(string name,IEnumerable<SimpleSteamGame> result)
    {
        var game = result.OrderBy(x => x.name.LevenshteinDistance(name)).First();
        return game;
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

    public void WriteLine(string value)
    {
        Console.WriteLine(value);
        Debug.WriteLine(value);
    }
}


public static class Fixture
{
    public static IHost Create(Action<IServiceCollection> registrations)
    {
        // Arrange: Create the HostBuilder
        var hostBuilder = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                registrations.Invoke(services);
                // Register the services for testing
                //services.AddTransient<IMyService, MyService>();
                //services.AddSingleton<IOtherDependency, MockOtherDependency>();
            });
        return hostBuilder.Build();
    }

    public static async Task<IHost> Run(this IHost host, Func<IServiceProvider, Task> act)
    {
        await host.StartAsync();
        await act.Invoke(host.Services);
        return host;
    }
}