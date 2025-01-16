using basic_fra_hw_02.Controllers.DTO;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[LogFilter]
public class CinemaController : ControllerBase
{
    private readonly ICinemaLogic _cinemaLogic;

    public CinemaController(ICinemaLogic cinemaLogic)
    {
        _cinemaLogic = cinemaLogic;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCinema([FromBody] NewCinemaDTO cinema)
    {
        await _cinemaLogic.AddCinemaAsync(cinema.ToModel());
        return Ok("Cinema added successfully!");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CinemaDTO>>> GetAllCinemas()
    {
        var cinemas = (await _cinemaLogic.GetAllCinemasAsync())
            .Select(x => CinemaDTO.FromModel(x));
        return Ok(cinemas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CinemaDTO>> GetCinemaById(string id)
    {
        var cinema = await _cinemaLogic.GetCinemaByIdAsync(id);
        return Ok(CinemaDTO.FromModel(cinema));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCinema(string id)
    {
        await _cinemaLogic.DeleteCinemaAsync(id);
        return Ok("Cinema deleted successfully");
    }
}
