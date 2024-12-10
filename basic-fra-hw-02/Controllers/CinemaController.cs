using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace basic_fra_hw_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly TicketService _ticketService;

        // The DI container will inject TicketService into the controller
        public CinemaController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Endpoint to create a new ticket
        [HttpPost("ticket")]
        public IActionResult CreateTicket([FromBody] MovieTicket ticket)
        {
            if (ticket == null || ticket.Recipients == null || !ticket.Recipients.Any())
            {
                return BadRequest("A movie ticket must have at least one recipient.");
            }

            // Add the new ticket via TicketService
            var createdTicket = _ticketService.AddTicket(ticket);
            return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, createdTicket);
        }

        // Endpoint to get all tickets
        [HttpGet("tickets")]
        public IActionResult GetTickets()
        {
            // Return all tickets from the service
            return Ok(_ticketService.GetTickets());
        }

        // Endpoint to get a ticket by GUID
        [HttpGet("ticket/{id}")]
        public IActionResult GetTicketById(Guid id)
        {
            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
                return NotFound();  // Return 404 if ticket is not found

            return Ok(ticket);  // Return the ticket if found
        }

        // Endpoint to update a ticket by GUID
        [HttpPut("ticket/{id}")]
        public IActionResult UpdateTicket(Guid id, [FromBody] MovieTicket ticket)
        {
            if (ticket == null || ticket.Recipients == null || !ticket.Recipients.Any())
            {
                return BadRequest("A movie ticket must have at least one recipient.");
            }

            // Try to update the ticket via TicketService
            if (!_ticketService.UpdateTicket(id, ticket))
                return NotFound();  // Return 404 if ticket with given ID is not found

            return NoContent();  // Return 204 No Content if updated successfully
        }

        // Endpoint to delete a ticket by GUID
        [HttpDelete("ticket/{id}")]
        public IActionResult DeleteTicket(Guid id)
        {
            if (!_ticketService.DeleteTicket(id))
                return NotFound();  // Return 404 if ticket with given ID is not found

            return NoContent();  // Return 204 No Content if deleted successfully
        }
    }
}
