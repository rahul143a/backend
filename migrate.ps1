param (
    [string]$context = "all",
    [string]$action = "update",
    [string]$name = ""
)

$projectDir = $PSScriptRoot
$startupProject = "Web.Host"
$migrationsProject = "Migrators"

function Add-Migration {
    param (
        [string]$Context,
        [string]$Name
    )

    if ([string]::IsNullOrEmpty($Name)) {
        Write-Error "Migration name is required for add action"
        exit 1
    }

    Write-Host "Adding migration '$Name' for context '$Context'..." -ForegroundColor Yellow
    dotnet ef migrations add $Name --project $migrationsProject --startup-project $startupProject --context $Context
}

function Update-Database {
    param (
        [string]$Context
    )

    Write-Host "Updating database for context '$Context'..." -ForegroundColor Yellow
    dotnet ef database update --project $migrationsProject --startup-project $startupProject --context $Context
}

function Remove-Migration {
    param (
        [string]$Context
    )

    Write-Host "Removing last migration for context '$Context'..." -ForegroundColor Yellow
    dotnet ef migrations remove --project $migrationsProject --startup-project $startupProject --context $Context
}

function List-Migrations {
    param (
        [string]$Context
    )

    Write-Host "Listing migrations for context '$Context'..." -ForegroundColor Yellow
    dotnet ef migrations list --project $migrationsProject --startup-project $startupProject --context $Context
}

# Set working directory
Set-Location $projectDir

# Process based on context
if ($context -eq "all" -or $context -eq "ApplicationDbContext") {
    switch ($action) {
        "add" { Add-Migration -Context "Infrastructure.Database.ApplicationDbContext" -Name $name }
        "update" { Update-Database -Context "Infrastructure.Database.ApplicationDbContext" }
        "remove" { Remove-Migration -Context "Infrastructure.Database.ApplicationDbContext" }
        "list" { List-Migrations -Context "Infrastructure.Database.ApplicationDbContext" }
        default { Write-Error "Unknown action: $action" }
    }
}

if ($context -eq "all" -or $context -eq "TenantDbContext") {
    switch ($action) {
        "add" { Add-Migration -Context "Infrastructure.MultiTenancy.TenantDbContext" -Name $name }
        "update" { Update-Database -Context "Infrastructure.MultiTenancy.TenantDbContext" }
        "remove" { Remove-Migration -Context "Infrastructure.MultiTenancy.TenantDbContext" }
        "list" { List-Migrations -Context "Infrastructure.MultiTenancy.TenantDbContext" }
        default { Write-Error "Unknown action: $action" }
    }
}

Write-Host "Migration operations completed." -ForegroundColor Green
