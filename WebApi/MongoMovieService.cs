using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoMovieService : IMovieService
{
    private readonly IMongoCollection<Movie> _movies;

    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("gbs");
        _movies = database.GetCollection<Movie>("movies");
    }

    public string Check()
    {
        try
        {
            _movies.Database.Client.ListDatabaseNames();
            return "Verbindung zur MongoDB erfolgreich.";
        }
        catch (Exception ex)
        {
            return $"Verbindung zur MongoDB fehlgeschlagen: {ex.Message}";
        }
    }

    public void Create(Movie movie)
    {
        _movies.InsertOne(movie);
    }

    public IEnumerable<Movie> Get()
    {
        return _movies.Find(_ => true).ToList();
    }

    public Movie? Get(string id)
    {
        return _movies.Find(m => m.Id == id).FirstOrDefault();
    }

    public bool Update(string id, Movie movie)
    {
        var result = _movies.ReplaceOne(m => m.Id == id, movie);
        return result.ModifiedCount > 0;
    }

    public bool Remove(string id)
    {
        var result = _movies.DeleteOne(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}