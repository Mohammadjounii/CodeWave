# Run Generated PDF Path Migration Script
Write-Host "Running Generated PDF Path Migration..." -ForegroundColor Cyan

$connectionString = "Server=Haidar;Database=CodeWaveDatabase;Trusted_Connection=True;TrustServerCertificate=True;"
$sqlScript = Get-Content -Path "CodeWave.Infrastructure\Migrations\AddGeneratedPDFPath.sql" -Raw

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    
    Write-Host "Connected to database successfully." -ForegroundColor Green
    
    # Split script by GO statements
    $commands = $sqlScript -split "GO\s*\r?\n" | Where-Object { $_.Trim() -ne "" }
    
    foreach ($command in $commands) {
        if ($command.Trim() -ne "") {
            $sqlCmd = New-Object System.Data.SqlClient.SqlCommand($command, $connection)
            $sqlCmd.ExecuteNonQuery() | Out-Null
            Write-Host "Executed command block..." -ForegroundColor Yellow
        }
    }
    
    $connection.Close()
    Write-Host "Migration completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "Error running migration: $_" -ForegroundColor Red
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
    exit 1
}




