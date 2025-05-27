using Infrastructure;
using Infrastructure.Database;
using Web.Host;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add configurations
builder.AddConfigurations();

// Add services to the container.
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Get tenant ID from command line arguments
var tenantId = builder.Configuration.GetValue<string>("tenant") ?? string.Empty;

// Add infrastructure services
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add infrastructure services
builder.Services.AddInfrastructure(builder.Configuration, tenantId);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Use infrastructure middleware
app.UseInfrastructure(builder.Configuration);

app.MapControllers();

// Check if we should apply migrations
if (builder.Configuration.GetSection("DatabaseSettings:EnableAutoMigration").Get<bool>())
{
    // Apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Initializing databases...");
            await databaseInitializer.InitializeDatabasesAsync();
            logger.LogInformation("Databases initialized successfully.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("PendingModelChangesWarning"))
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("There are pending model changes. Please add a new migration before updating the database.");
            logger.LogWarning("You can create a new migration using: dotnet ef migrations add <MigrationName> --project Inventory.Migrators --startup-project Inventory.Web.Host");

            // Continue running the application even with pending migrations
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while initializing the databases.");
        }
    }
}

app.Run();
