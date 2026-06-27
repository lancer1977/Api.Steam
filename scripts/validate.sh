#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SOLUTION_FILE="${SOLUTION_FILE:-PolyhydraGames.Api.Steam.sln}"
CONFIGURATION="${CONFIGURATION:-Release}"

cd "$ROOT_DIR"

echo "1/3 restore"
dotnet restore "$SOLUTION_FILE"

echo "2/3 build"
dotnet build "$SOLUTION_FILE" --configuration "$CONFIGURATION" --no-restore

echo "3/3 tests"
dotnet test "$SOLUTION_FILE" --configuration "$CONFIGURATION" --no-restore --no-build --verbosity normal

echo "Validation complete."
