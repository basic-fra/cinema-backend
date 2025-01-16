using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Logics
{
    public interface IMovieLogic
    {
        Task AddMovieAsync(Movie movie);
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(string id);
        Task DeleteMovieAsync(string id);
    }
}
