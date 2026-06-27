# Scripts

## Validation

Run the repo-native validation lane with:

```bash
bash scripts/validate.sh
```

The command restores, builds, and tests `PolyhydraGames.Api.Steam.sln` in
`Release` configuration. Live Steam API tests skip unless `Steam:ApiKey` is
configured.

## Coverage Template

This folder also captures a reusable shell-script baseline for .NET coverage
workflows.

Copy the script into a product repo when you want a simple local command that:

- runs the target test project or solution
- collects `XPlat Code Coverage`
- writes artifacts to a predictable `TestResults/` location
- optionally generates a human-readable HTML report when ReportGenerator is available

## Expected inputs

- `COVERAGE_TARGET` or the first positional argument: the solution or test project path
- `CONFIGURATION` (optional): defaults to `Release`
- `COVERAGE_RESULTS_DIR` (optional): defaults to `./TestResults/coverage`

## Notes

- The script is intentionally lightweight so repos can copy it and adapt the target path.
- If ReportGenerator is installed, the script will emit an HTML summary under the report directory.
- Repos with more advanced needs can wrap this command in CI or a repo-local task file.
