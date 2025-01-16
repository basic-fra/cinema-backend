using basic_fra_hw_02.Controllers.DTO;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[LogFilter]
public class MovieController : ControllerBase
{
    private readonly IMovieLogic _movieLogic;

    public MovieController(IMovieLogic movieLogic)
    {
        _movieLogic = movieLogic;
    }

    [HttpPost]
    public async Task<ActionResult> CreateMovie([FromBody] NewMovieDTO movie)
    {
        await _movieLogic.AddMovieAsync(movie.ToModel());
        if (movie == null)
        {
            throw new UserErrorMessage("Movie not found.");
        }
        return Ok("Movie added succesfully.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
    {
        var movies = (await _movieLogic.GetAllMoviesAsync()).Select(x => MovieDTO.FromModel(x)); ;
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovieById(string id)
    {
        var movie = await _movieLogic.GetMovieByIdAsync(id);
        return Ok(MovieDTO.FromModel(movie));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(string id)
    {
        await _movieLogic.DeleteMovieAsync(id);
        return Ok("Movie deleted succesfully.");
    }
}
