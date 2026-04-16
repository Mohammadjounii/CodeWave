# Run CodeWave Application
Write-Host "Stopping any running CodeWave processes..." -ForegroundColor Yellow
Get-Process -Name "CodeWave.Web","dotnet" -ErrorAction SilentlyContinue | Where-Object { $_.Path -like "*CWAVE*" -or $_.CommandLine -like "*CodeWave*" } | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2
Write-Host "Processes stopped." -ForegroundColor Green
Write-Host ""

Write-Host "Starting CodeWave application..." -ForegroundColor Green
Write-Host "The application will be available at:" -ForegroundColor Yellow
Write-Host "  - http://localhost:5023" -ForegroundColor Cyan
Write-Host "  - https://localhost:7000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host ""

cd CodeWave.Web
dotnet run





