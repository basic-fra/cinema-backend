using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;

    public MovieController()
    {
        string connectionString = "Data Source=CinemaDb.sqlite;";
        _movieService = new MovieService(connectionString);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
    {
        movie.MovieId = Guid.NewGuid();
        await _movieService.AddMovieAsync(movie);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.MovieId }, movie);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMovieById(Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return NotFound(new { Message = "Movie not found" });

        return Ok(movie);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return NotFound(new { Message = "Movie not found" });

        await _movieService.DeleteMovieAsync(id);
        return Ok(new { Message = "Movie deleted successfully" });
    }
}
