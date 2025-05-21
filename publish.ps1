param(
    [Parameter(Mandatory=$true, Position=0)]
    [string]$ProjectFolder,
    
    [Parameter(Mandatory=$true, Position=1)]
    [string]$ApiKey
)

# Function to extract version from csproj file
function Get-ProjectVersion {
    param([string]$ProjectPath)
    
    $xml = [xml](Get-Content $ProjectPath)
    $versionPrefix = $xml.Project.PropertyGroup.VersionPrefix
    if ($versionPrefix) {
        return $versionPrefix
    }
    throw "Version not found in $ProjectPath"
}

# Verify and enter project folder
if (-not (Test-Path $ProjectFolder)) {
    throw "Project folder not found: $ProjectFolder"
}
Push-Location $ProjectFolder

try {
    # Find the csproj file in project directory
$projFile = Get-ChildItem -Filter "*.csproj" | Select-Object -First 1
if (-not $projFile) {
    throw "No .csproj file found in current directory"
}

# Get project name without extension for package naming
$projectName = [System.IO.Path]::GetFileNameWithoutExtension($projFile.Name)

# Get version
$version = Get-ProjectVersion -ProjectPath $projFile.Name
Write-Host "Found version: $version for project $projectName"

# Build and pack
Write-Host "Building and packing project..."
dotnet pack $projFile.Name -c Release

# Push specific version
$package = ".\bin\Release\$projectName.$version.nupkg"

if (Test-Path $package) {
    Write-Host "Pushing package version $version..."
    dotnet nuget push $package --api-key $ApiKey --source https://api.nuget.org/v3/index.json
} else {
    Write-Error "Package not found at: $package"
}
} finally {
    # Return to original directory
    Pop-Location
}
