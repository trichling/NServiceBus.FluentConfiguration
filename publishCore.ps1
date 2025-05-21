param(   
    [Parameter(Mandatory=$true, Position=1)]
    [string]$ApiKey
)

# Call publish.ps1 with the Core project folder
$scriptPath = Join-Path $PSScriptRoot "publish.ps1"
& $scriptPath "src\NServiceBus.FluentConfiguration.Core" $ApiKey
