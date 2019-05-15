Push-Location $PSScriptRoot;

. ".\Vsix-SetVersion.ps1"

dotnet restore ..\Neptuo.Productivity.sln
Vsix-SetVersion;

Pop-Location;