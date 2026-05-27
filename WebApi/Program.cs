using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// DatabaseSettings registrieren
var movieDatabaseConfigSection =
    builder.Configuration.GetSection("DatabaseSettings");

builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

var app = builder.Build();

// Root-Endpoint
app.MapGet("/", () => "Minimal API Version 1.0");

// Check-Endpoint mit Injection
app.MapGet("/check",
    (Microsoft.Extensions.Options.IOptions<DatabaseSettings> options) =>
{
    try
    {
        var mongoDbConnectionString = options.Value.ConnectionString;

        var client = new MongoClient(mongoDbConnectionString);

        var databases = client.ListDatabaseNames().ToList();

        return $"Zugriff auf MongoDB ok. Vorhandene DBs: {string.Join(",", databases)}";
    }
    catch (Exception ex)
    {
        return $"Fehler: {ex.Message}";
    }
});

app.Run();