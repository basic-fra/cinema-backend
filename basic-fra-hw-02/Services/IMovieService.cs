using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Services
{
    public interface IMovieService
    {
        Task AddMovieAsync(Movie movie);
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(string id);
        Task DeleteMovieAsync(string id);
    }
}
