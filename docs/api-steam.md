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
