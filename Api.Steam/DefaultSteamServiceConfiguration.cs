namespace PolyhydraGames.Api.Steam
{
    /// <summary>
    /// Default implementation of <see cref="ISteamServiceConfiguration"/> with 1 hour lifetimes.
    /// </summary>
    public class DefaultSteamServiceConfiguration : ISteamServiceConfiguration
    {
        public string ApiKey { get; set; }
        public TimeSpan AppListTtl => TimeSpan.FromHours(24);
        public TimeSpan AppDetailsTtl => TimeSpan.FromHours(24);
        public TimeSpan DefaultTtl => TimeSpan.FromHours(1);
    }
}