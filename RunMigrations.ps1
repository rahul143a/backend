# Script to run migrations directly using EF Core's Database.Migrate() method

# Set working directory
$projectDir = $PSScriptRoot
Set-Location $projectDir

# Build the projects
Write-Host "Building projects..." -ForegroundColor Yellow
dotnet build Web.Host
dotnet build Infrastructure
dotnet build Migrators

# Run the API with a special command-line argument to run migrations
Write-Host "Running migrations..." -ForegroundColor Yellow
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:DatabaseSettings__EnableAutoMigration = "true"
dotnet run --project Web.Host --no-build

Write-Host "Migration operations completed." -ForegroundColor Green
