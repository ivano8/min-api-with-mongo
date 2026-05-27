var builder = WebApplication.CreateBuilder(args);

// DatabaseSettings registrieren
var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

// MovieService als Singleton registrieren
builder.Services.AddSingleton<IMovieService, MongoMovieService>();

var app = builder.Build();

// Root-Endpoint
app.MapGet("/", () => "Minimal API Version 1.0");

// Check-Endpoint
app.MapGet("/check", (IMovieService movieService) =>
{
    return movieService.Check();
});

// Insert Movie
app.MapPost("/api/movies", (IMovieService movieService, Movie movie) =>
{
    try
    {
        movieService.Create(movie);
        return Results.Created($"/api/movies/{movie.Id}", movie);
    }
    catch
    {
        return Results.Conflict();
    }
});

// Get all Movies
app.MapGet("/api/movies", (IMovieService movieService) =>
{
    return Results.Ok(movieService.Get());
});

// Get Movie by id
app.MapGet("/api/movies/{id}", (IMovieService movieService, string id) =>
{
    var movie = movieService.Get(id);
    return movie is not null ? Results.Ok(movie) : Results.NotFound();
});

// Update Movie
app.MapPut("/api/movies/{id}", (IMovieService movieService, string id, Movie movie) =>
{
    var updated = movieService.Update(id, movie);
    return updated ? Results.Ok(movie) : Results.NotFound();
});

// Delete Movie
app.MapDelete("/api/movies/{id}", (IMovieService movieService, string id) =>
{
    var removed = movieService.Remove(id);
    return removed ? Results.NoContent() : Results.NotFound();
});

app.Run();