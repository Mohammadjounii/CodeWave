# Safe Build Script - Stops processes before building
Write-Host "=== Safe Build Script ===" -ForegroundColor Cyan
Write-Host ""

# Step 1: Stop all running processes
Write-Host "Step 1: Stopping running processes..." -ForegroundColor Yellow
& "$PSScriptRoot\stop-app.ps1"
Write-Host ""

# Step 2: Clean build
Write-Host "Step 2: Cleaning previous build..." -ForegroundColor Yellow
cd CodeWave.Web
dotnet clean
cd ..
Write-Host "Clean completed." -ForegroundColor Green
Write-Host ""

# Step 3: Build
Write-Host "Step 3: Building project..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully!" -ForegroundColor Green
} else {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}


