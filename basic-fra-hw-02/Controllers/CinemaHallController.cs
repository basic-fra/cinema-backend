using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CinemaHallController : ControllerBase
{
    private readonly CinemaHallService _cinemaHallService;
    private readonly CinemaService _cinemaService;

    public CinemaHallController()
    {
        string connectionString = "Data Source=CinemaDb.sqlite;";
        _cinemaHallService = new CinemaHallService(connectionString);
        _cinemaService = new CinemaService(connectionString);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCinemaHall([FromBody] CinemaHall cinemaHall)
    {
        if (cinemaHall.HallId == Guid.Empty)
            cinemaHall.HallId = Guid.NewGuid();

        var cinemaExists = await _cinemaService.CheckIfCinemaExistsAsync(cinemaHall.CinemaId);
        if (!cinemaExists)
            return BadRequest(new { Message = "Cinema not found" });

        await _cinemaHallService.AddCinemaHallAsync(cinemaHall);
        return CreatedAtAction(nameof(GetCinemaHallById), new { id = cinemaHall.HallId }, cinemaHall);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCinemaHalls()
    {
        var cinemaHalls = await _cinemaHallService.GetAllCinemaHallsAsync();
        return Ok(cinemaHalls);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCinemaHallById(Guid id)
    {
        var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
        if (cinemaHall == null)
            return NotFound(new { Message = "CinemaHall not found" });

        return Ok(cinemaHall);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCinemaHall(Guid id)
    {
        var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
        if (cinemaHall == null)
            return NotFound(new { Message = "CinemaHall not found" });

        await _cinemaHallService.DeleteCinemaHallAsync(id);
        return Ok("CinemaHall deleted successfully");
    }
}
