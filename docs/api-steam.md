# Api.Steam

**Location:** `~/code/APIs/Api.Steam`

**Purpose:** Steam Web API integration (profiles, games, owned library, etc.).

**Assembly:** `PolyhydraGames.Api.Steam`

## Key Folders

- `Models/` — Steam DTOs
- `Enums/` — Steam enums
- `data/` — local data assets (if any)

## Key Types

| Type | Purpose |
|------|---------|
| `ISteamService` | Main service contract |
| `ISteamServiceConfiguration` | Config contract |
| `DefaultSteamServiceConfiguration` | Default config implementation |
| `SteamExtensions` | Helpers |

## Dependencies

- `PolyhydraGames.Core.Interfaces`
- `PolyhydraGames.Core.Interfaces.Gaming`
- `PolyhydraGames.Extensions`

## Config

Has config (`hasConfig: true`) — likely requires Steam API key + base URL.

## Status

✅ **Working** (library + tests present)

## Coverage Notes

- [x] `SteamPlayer` deserializes Steam `gameid` as a string
- [x] `SteamService` has deterministic unit coverage for app list, app details, and Steam Web API request shapes
- [x] Live integration tests initialize `SteamService` correctly when a Steam API key is present
- [ ] Add broader contract fixtures for rare or nested Steam payloads if new endpoints are introduced
