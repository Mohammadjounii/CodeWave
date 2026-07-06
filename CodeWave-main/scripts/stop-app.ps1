# Stop CodeWave Application
# Stops any running CodeWave instance so a build/run doesn't hit "file is locked" errors.
# Matches by the ports the app listens on (reliable) and by process path under this project
# folder (catches a built CodeWave.Web.exe that hasn't bound the port yet).

Write-Host "Stopping CodeWave processes..." -ForegroundColor Yellow

$projectRoot = Split-Path -Parent $PSScriptRoot
$stopped = 0

foreach ($port in 7000) {
    Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue |
        Select-Object -ExpandProperty OwningProcess -Unique |
        ForEach-Object {
            Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue
            $stopped++
        }
}

Get-Process -Name "CodeWave.Web", "dotnet" -ErrorAction SilentlyContinue |
    Where-Object { $_.Path -like "$projectRoot*" } |
    ForEach-Object {
        Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
        $stopped++
    }

if ($stopped -gt 0) {
    Write-Host "Stopped $stopped process(es). Waiting for file handles to release..." -ForegroundColor Green
    Start-Sleep -Seconds 2
} else {
    Write-Host "No running CodeWave processes found." -ForegroundColor Green
}
