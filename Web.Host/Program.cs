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

// Initialize databases following leadrat-black pattern
await app.Services.InitializeDatabasesAsync();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Use infrastructure middleware
app.UseInfrastructure(builder.Configuration);

app.MapControllers();

app.Run();
