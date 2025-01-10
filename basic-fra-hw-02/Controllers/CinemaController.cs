using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CinemaController : ControllerBase
{
    private readonly CinemaService _cinemaService;

    public CinemaController()
    {
        string connectionString = "Data Source=CinemaDb.sqlite;";
        //string connectionString = "Server=(localdb)\\MSSQLLocalDb;database=CinemaDb_nova;Trusted_Connection=True;";
        _cinemaService = new CinemaService(connectionString);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCinema([FromBody] Cinema cinema)
    {
        cinema.CinemaId = Guid.NewGuid();
        await _cinemaService.AddCinemaAsync(cinema);
        return CreatedAtAction(nameof(GetCinemaById), new { id = cinema.CinemaId }, cinema);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCinemas()
    {
        var cinemas = await _cinemaService.GetAllCinemasAsync();
        return Ok(cinemas);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCinemaById(Guid id)
    {
        var cinema = await _cinemaService.GetCinemaByIdAsync(id);
        if (cinema == null)
            return NotFound(new { Message = "Cinema not found" });

        return Ok(cinema);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCinema(Guid id)
    {
        var cinema = await _cinemaService.GetCinemaByIdAsync(id);
        if (cinema == null)
            return NotFound(new { Message = "Cinema not found" });

        await _cinemaService.DeleteCinemaAsync(id);
        return Ok(new { Message = "Cinema deleted successfully" });
    }
}
