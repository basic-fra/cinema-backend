using basic_fra_hw_02.Controllers.DTO;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[LogFilter]
public class TicketController : ControllerBase
{
    private readonly ITicketLogic _ticketLogic;

    public TicketController(ITicketLogic ticketLogic)
    {
        _ticketLogic = ticketLogic;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTicket([FromBody] NewTicketDTO ticket)
    {
        await _ticketLogic.AddTicketAsync(ticket.ToModel());
        return Ok("The movie ticket has been purchased.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAllTickets()
    {
        var tickets = (await _ticketLogic.GetAllTicketsAsync())
            .Select(x => TicketDTO.FromModel(x)); ;
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTicketById(string id)
    {
        var ticket = await _ticketLogic.GetTicketByIdAsync(id);
        return Ok(TicketDTO.FromModel(ticket));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(string id)
    {
        await _ticketLogic.DeleteTicketAsync(id);
        return Ok("Ticket deleted successfully!");
    }
}
