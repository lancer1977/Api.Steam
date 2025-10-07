namespace PolyhydraGames.Api.Steam
{
    /// <summary>
    /// Defines cache lifetime and service configuration for Steam API calls.
    /// </summary>
    public interface ISteamServiceConfiguration
    {
        /// <summary>How long to cache Steam app list results.</summary>
        TimeSpan AppListTtl { get; }

        /// <summary>How long to cache app details (store metadata).</summary>
        TimeSpan AppDetailsTtl { get; }

        /// <summary>How long to cache other Steam-related data by default.</summary>
        TimeSpan DefaultTtl { get; }
    }
}