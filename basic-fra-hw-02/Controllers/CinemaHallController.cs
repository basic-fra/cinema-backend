using basic_fra_hw_02.Controllers.DTO;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[LogFilter]
public class CinemaHallController : ControllerBase
{
    private readonly ICinemaHallLogic _cinemaHallLogic;

    public CinemaHallController(ICinemaHallLogic cinemaHallLogic)
    {
        _cinemaHallLogic = cinemaHallLogic;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCinemaHall([FromBody] NewHallDTO cinemaHall)
    {
        await _cinemaHallLogic.AddCinemaHallAsync(cinemaHall.ToModel());
        return Ok("Cinema hall added successfully");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallDTO>>> GetAllCinemaHalls()
    {
        var cinemaHalls = (await _cinemaHallLogic.GetAllCinemaHallsAsync()).Select(x => HallDTO.FromModel(x)); 
        return Ok(cinemaHalls);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HallDTO>> GetCinemaHallById(string id)
    {
        var cinemaHall = await _cinemaHallLogic.GetCinemaHallByIdAsync(id);
        return Ok(HallDTO.FromModel(cinemaHall));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCinemaHall(string id)
    {
        await _cinemaHallLogic.DeleteCinemaHallAsync(id);
        return Ok("CinemaHall deleted successfully");
    }
}
