public interface IMovieService
{
    string Check();
    void Create(Movie movie);
    IEnumerable<Movie> Get();
    Movie? Get(string id);
    bool Update(string id, Movie movie);
    bool Remove(string id);
}