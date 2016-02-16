cd $PSScriptRoot

$ErrorActionPreference="Stop"

throw "Boom"

if(!(Get-Command dotnet -ErrorAction SilentlyContinue))
{
	Write-Host "Installing dotnet-cli"
	$dotnetInstall = "$PSScriptRoot\dotnet_install.ps1"
	iwr https://github.com/dotnet/cli/blob/rel/1.0.0/scripts/obtain/install.ps1 -OutFile $dotnetInstall
	& $dotnetInstall -Channel beta
}

Write-Host "Restoring packages for build tasks"
dotnet restore
if($LASTEXITCODE -ne 0) { throw "Build failed" }

Write-Host "Building build tasks"
del -rec -for "$PSScriptRoot\tasks\bin\app"
dotnet build --framework dnxcore50 "$PSScriptRoot\tasks" -o "$PSScriptRoot\tasks\bin\app"
if($LASTEXITCODE -ne 0) { throw "Build failed" }

Write-Host "Running build tasks"
& "$PSScriptRoot\tasks\bin\app\tasks.exe" @args
if($LASTEXITCODE -ne 0) { throw "Build failed" }

Write-Host "Build completed"