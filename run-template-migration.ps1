# PowerShell script to run Template column migration
$connectionString = "Server=localhost;Database=CodeWaveDatabase;Integrated Security=True;TrustServerCertificate=True;"

Write-Host "Running Template Column Migration..." -ForegroundColor Cyan

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    Write-Host "Connected to database successfully." -ForegroundColor Green

    $sql = Get-Content -Path "CodeWave.Infrastructure\Migrations\AddTemplateColumn.sql" -Raw
    $command = New-Object System.Data.SqlClient.SqlCommand($sql, $connection)
    $command.ExecuteNonQuery()
    
    Write-Host "Migration completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "Error running migration: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}




