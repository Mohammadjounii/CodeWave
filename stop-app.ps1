# Stop CodeWave Application
Write-Host "Stopping all CodeWave and dotnet processes..." -ForegroundColor Yellow

# Stop processes by name
$processes = Get-Process -Name "CodeWave.Web","dotnet" -ErrorAction SilentlyContinue | Where-Object { 
    $_.Path -like "*CWAVE*" -or 
    $_.MainWindowTitle -like "*CodeWave*" -or
    (Get-WmiObject Win32_Process -Filter "ProcessId = $($_.Id)" -ErrorAction SilentlyContinue | Select-Object -ExpandProperty CommandLine) -like "*CodeWave*"
}

if ($processes) {
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    Write-Host "Stopped $($processes.Count) process(es)." -ForegroundColor Green
    Start-Sleep -Seconds 2
} else {
    Write-Host "No running CodeWave processes found." -ForegroundColor Green
}

# Also check for processes using port 7000
Write-Host "Checking for processes using port 7000..." -ForegroundColor Yellow
$portProcesses = Get-NetTCPConnection -LocalPort 7000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess -Unique
if ($portProcesses) {
    foreach ($pid in $portProcesses) {
        try {
            Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
            Write-Host "Stopped process $pid using port 7000." -ForegroundColor Green
        } catch {
            # Process might already be stopped
        }
    }
    Start-Sleep -Seconds 2
}

Write-Host "All processes stopped. Safe to build now." -ForegroundColor Green


