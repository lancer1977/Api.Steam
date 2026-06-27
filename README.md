# Steam API Integration

A .NET 10 library for interacting with the Steam Web API.

[![publish](https://github.com/PolyhydraGames/Api.Steam/actions/workflows/publish.yml/badge.svg)](https://github.com/PolyhydraGames/Api.Steam/actions/workflows/publish.yml)

## Tags

- api
- dotnet
- api-steam
- docs
- testing
- steam

## Project Structure

- `Api.Steam/`: Core library implementation including models for various Steam API responses.
- `Api.Steam.Test/`: NUnit tests for the library.
- `docs/`: Detailed documentation and design decisions.

## Getting Started

### Prerequisites
- .NET 10.0 SDK

## Testing
The project uses NUnit for testing. To run tests:
```bash
bash scripts/validate.sh
```

Live Steam Web API tests skip unless `Steam:ApiKey` is configured through .NET
configuration, user secrets, or environment variables.

## Packaging
NuGet packages are published from GitHub Actions to GitHub Packages.

## Documentation
Additional documentation can be found in the [docs](docs/) directory.

## Docs

- [Docs Index](./docs/README.md)
- [Feature Index](./docs/features/README.md)
- [Roadmap Index](./docs/roadmaps/README.md)
- [Health Baseline](./code_health.md)
