# Script to add Quiz tables to the database
$connectionString = "Server=Haidar;Database=CodeWaveDatabase;Trusted_Connection=True;TrustServerCertificate=True;"
$sqlFile = "CodeWave.Infrastructure\Migrations\AddQuizTables.sql"

Write-Host "Adding Quiz tables to database..." -ForegroundColor Yellow

# Try using sqlcmd if available
$sqlcmdPath = "sqlcmd"
if (Get-Command sqlcmd -ErrorAction SilentlyContinue) {
    $sqlFileFullPath = Resolve-Path $sqlFile
    sqlcmd -S "Haidar" -d "CodeWaveDatabase" -E -i $sqlFileFullPath
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Quiz tables added successfully!" -ForegroundColor Green
    } else {
        Write-Host "Error executing SQL script. Exit code: $LASTEXITCODE" -ForegroundColor Red
        Write-Host "Please run the SQL script manually:" -ForegroundColor Yellow
        Write-Host "  File: $sqlFileFullPath" -ForegroundColor Cyan
    }
} else {
    Write-Host "sqlcmd not found. Please run the SQL script manually:" -ForegroundColor Yellow
    Write-Host "  File: $(Resolve-Path $sqlFile)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or install SQL Server Command Line Utilities and run:" -ForegroundColor Yellow
    Write-Host "  sqlcmd -S Haidar -d CodeWaveDatabase -E -i `"$(Resolve-Path $sqlFile)`"" -ForegroundColor Cyan
}

