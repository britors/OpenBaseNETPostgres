#!/usr/bin/env bash
set -euo pipefail

repo_root=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)
validation_dir=$(mktemp -d)
trap 'rm -rf -- "$validation_dir"' EXIT
cd "$repo_root"

dotnet restore OpenBasePgsql.sln
dotnet build OpenBasePgsql.sln --no-restore --configuration Release
dotnet test OpenBasePgsql.sln --no-build --configuration Release

dotnet pack Template.csproj --configuration Release -p:NoDefaultExcludes=true -o "$validation_dir/packages"
package=$(find "$validation_dir/packages" -maxdepth 1 -name '*.nupkg' -print -quit)
test -n "$package"
dotnet new install "$package" --debug:custom-hive "$validation_dir/hive"
dotnet new openbasenet-pgsql --name RenamedApi --output "$validation_dir/generated" --debug:custom-hive "$validation_dir/hive"
test -f "$validation_dir/generated/RenamedApi.sln"
test -f "$validation_dir/generated/Directory.Packages.props"
if grep -R -n --include='*.cs' --include='*.csproj' --include='*.sln' 'OpenBaseNET' "$validation_dir/generated"; then
    echo 'Generated project still contains the original name.' >&2
    exit 1
fi
if find "$validation_dir/generated" -type d \( -name bin -o -name obj -o -name .git -o -name .github \) -print -quit | grep -q .; then
    echo 'Generated project contains build or repository artifacts.' >&2
    exit 1
fi
dotnet restore "$validation_dir/generated/RenamedApi.sln"
dotnet build "$validation_dir/generated/RenamedApi.sln" --no-restore --configuration Release
dotnet test "$validation_dir/generated/RenamedApi.sln" --no-build --configuration Release
mkdir -p "$repo_root/artifacts/packages"
cp -- "$package" "$repo_root/artifacts/packages/"
