# Api.Steam Health Baseline

Last reviewed: 2026-06-27
Tracking: GitHub issue [#3](https://github.com/lancer1977/Api.Steam/issues/3)
Hermes Kanban: pending promotion

## Purpose

`Api.Steam` is a .NET 10 Steam Web API wrapper. The library owns typed Steam
request/response models, cache-aware service methods, and fixture-backed tests
for primary client flows. Live Steam calls stay opt-in because they require a
Steam API key and may be rate limited.

## Native Validation

Credential-free validation:

```bash
bash scripts/validate.sh
```

Equivalent expanded commands:

```bash
dotnet restore PolyhydraGames.Api.Steam.sln
dotnet build PolyhydraGames.Api.Steam.sln --configuration Release --no-restore
dotnet test PolyhydraGames.Api.Steam.sln --configuration Release --no-restore --no-build --verbosity normal
```

Live integration tests are marked `Category("Integration")` and skip when
`Steam:ApiKey` is not configured. Configure the key through user secrets,
environment variables, or another .NET configuration provider before running
live Steam API checks.

Repo-health checks:

```bash
dotnet list PolyhydraGames.Api.Steam.sln package --outdated
devstudio validate --repo /mnt/data/lancer1977/code/Api.Steam
```

## Current Audit

- Native validation command is now automated by `scripts/validate.sh`.
- CI already restores, builds, and tests `PolyhydraGames.Api.Steam.sln`.
- `.devstudio/runtime/` is generated DevStudio runtime output and is ignored so
  it stays uncommitted.
- Dependency audit on 2026-06-27 found no updates for
  `PolyhydraGames.Api.Steam`; `Api.Steam.Test` can update
  `StackExchange.Redis` from `3.0.0` to `3.0.7` in a separate dependency slice.
- No central package management file is present. Adopt central package
  management only if the API-family repos standardize that pattern together.

## Follow-Up Slices

- Add contract-level smoke tests for Steam service methods that currently rely
  on live calls, using fixture HTTP responses for rate-limited endpoints.
- Add a small diagnostics helper or documented health check that verifies
  configuration readiness without calling destructive or account-specific APIs.
- Add one end-to-end README example that lists required secrets, local command,
  sample request, and expected response shape.
- Review request/response model naming and error mapping against current
  Polyhydra API wrapper conventions.
- Handle the `StackExchange.Redis` test dependency update after a clean test
  run, or track why the repo should remain pinned.
