using basic_fra_hw_02.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketController()
    {
        string connectionString = "Data Source=CinemaDb.sqlite;";
        _ticketService = new TicketService(connectionString);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
        ticket.TicketId = Guid.NewGuid();
        await _ticketService.AddTicketAsync(ticket);
        return CreatedAtAction(nameof(GetTicketById), new { id = ticket.TicketId }, ticket);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound(new { Message = "Ticket not found" });

        return Ok(ticket);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        await _ticketService.DeleteTicketAsync(id);
        return Ok("Ticket deleted successfully!");
    }
}
