# Run CodeWave Application
& "$PSScriptRoot\stop-app.ps1"

Write-Host ""
Write-Host "Starting CodeWave application..." -ForegroundColor Green
Write-Host "The application will be available at:" -ForegroundColor Yellow
Write-Host "  - https://localhost:7000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host ""

dotnet run --project "$PSScriptRoot\..\CodeWave.Web"
