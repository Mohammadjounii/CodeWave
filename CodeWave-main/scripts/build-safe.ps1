# Safe Build Script - stops any running instance, cleans, then builds.
Write-Host "=== Safe Build ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "Step 1: Stopping running processes..." -ForegroundColor Yellow
& "$PSScriptRoot\stop-app.ps1"
Write-Host ""

$project = "$PSScriptRoot\..\CodeWave.Web\CodeWave.Web.csproj"

Write-Host "Step 2: Cleaning previous build..." -ForegroundColor Yellow
dotnet clean "$project"
Write-Host "Clean completed." -ForegroundColor Green
Write-Host ""

Write-Host "Step 3: Building project..." -ForegroundColor Yellow
dotnet build "$project"
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully!" -ForegroundColor Green
} else {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}
