#!/usr/bin/env bash
set -euo pipefail

rm -rf ./.nupkg

if [[ -z "${NUGET_API_KEY:-}" ]]; then
  echo "Error: NUGET_API_KEY is not set. Configure it as a repository secret and pass it to this workflow."
  exit 1
fi

projects=(
  "./dotnet/Pxl.Ui/Pxl.Ui.fsproj"
  "./dotnet/Pxl.Ui.FSharp/Pxl.Ui.FSharp.fsproj"
  "./dotnet/Pxl.Ui.CSharp/Pxl.Ui.CSharp.csproj"
  "./dotnet/Pxl/Pxl.fsproj"
)

for project in "${projects[@]}"; do
  echo "Processing $project"

  dotnet restore "$project"
  dotnet build "$project" --configuration Release
  dotnet pack "$project" --configuration Release --output ./.nupkg
done

dotnet nuget push ./.nupkg/*.nupkg -k "$NUGET_API_KEY" -s https://api.nuget.org/v3/index.json
