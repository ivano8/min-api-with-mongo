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

// Insert Movie
// Wenn Objekt eingefügt werden konnte,
// wird es mit Statuscode 201 (Created) zurückgegeben.
// Bei Fehler wird Statuscode 409 (Conflict) zurückgegeben.
app.MapPost("/api/movies", (Movie movie) =>
{
    throw new NotImplementedException();
});

// Get all Movies
// Gibt alle vorhandenen Movie-Objekte mit Statuscode 200 Ok zurück.
app.MapGet("/api/movies", () =>
{
    throw new NotImplementedException();
});

// Get Movie by id
// Gibt das gewünschte Movie-Objekt mit Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapGet("/api/movies/{id}", (string id) =>
{
    throw new NotImplementedException();
});

// Update Movie
// Gibt das aktualisierte Movie-Objekt zurück.
// Der erfolgreiche Update wird mit Statuscode 200 OK quittiert.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben
app.MapPut("/api/movies/{id}", (string id, Movie movie) =>
{
    throw new NotImplementedException();
});

// Delete Movie
// Gibt bei erfolgreicher Löschung Statuscode 204 NoContent zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapDelete("/api/movies/{id}", (string id) =>
{
    throw new NotImplementedException();
});

app.Run();