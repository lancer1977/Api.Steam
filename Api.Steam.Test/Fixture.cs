using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Steam.Test
{
    public static class Fixture
    {
        public static IHost Create(Action<IServiceCollection, IConfiguration> registrations)
        {
            var config = BuildConfiguration();

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddConfiguration(config);
                })
                .ConfigureServices((context, services) =>
                {
                    registrations.Invoke(services, config);
                });

            return hostBuilder.Build();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(typeof(Fixture).Assembly, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static async Task<IHost> Run(this IHost host, Func<IServiceProvider, Task> act)
        {
            await host.StartAsync();
            await act.Invoke(host.Services);
            return host;
        }
    }
}
