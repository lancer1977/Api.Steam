# Api.Steam Project Atlas

## Purpose

`Api.Steam` is the .NET Steam Web API wrapper used by Polyhydra projects for
Steam profile, library, achievement, stats, news, and app metadata lookups.

## Primary Surfaces

- `Api.Steam/SteamService.cs`: cache-aware service implementation.
- `Api.Steam/SteamWebApiService.cs`: Steam Web API request helpers.
- `Api.Steam/Models/`: typed Steam response models.
- `Api.Steam.Test/`: fixture-backed NUnit tests plus opt-in live integration
  tests that require `Steam:ApiKey`.

## Validation

```bash
bash scripts/validate.sh
dotnet list PolyhydraGames.Api.Steam.sln package --outdated
devstudio validate --repo /mnt/data/lancer1977/code/Api.Steam
```

## Boundaries

- Keep live Steam API calls opt-in and fixture-backed tests credential-free.
- Keep package publishing in GitHub Actions.
- Track broad repo-health follow-up in GitHub issues instead of expanding this
  atlas into a second backlog.
