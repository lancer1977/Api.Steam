namespace PolyhydraGames.Api.Steam.Models
{
    public record SteamPlayer(
        string SteamId,
        string PersonaName,
        string ProfileUrl,
        string Avatar,
        string AvatarMedium,
        string AvatarFull,
        int PersonaState,
        int CommunityVisibilityState,
        int ProfileState,
        long LastLogoff,
        int? CommentPermission,
        string? RealName,
        string? PrimaryClanId,
        long? TimeCreated,
        int? GameId,
        string? GameExtraInfo,
        string? LocCountryCode,
        string? LocStateCode,
        int? LocCityId
    );
}