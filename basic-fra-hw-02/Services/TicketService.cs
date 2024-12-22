using basic_fra_hw_02.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace basic_fra_hw_02.Services
{
    public class TicketService
    {
        private readonly List<MovieTicket> _tickets;

        // Constructor without any parameters
        public TicketService()
        {
            _tickets = new List<MovieTicket>();
        }

        // Get all tickets
        public IEnumerable<MovieTicket> GetTickets() => _tickets;

        // Get ticket by ID
        public MovieTicket GetTicketById(Guid id) => _tickets.FirstOrDefault(t => t.Id == id);

        // Add a new ticket
        public MovieTicket AddTicket(MovieTicket ticket)
        {
            ticket.Id = Guid.NewGuid(); // Generate a new GUID for the ticket ID
            _tickets.Add(ticket);
            return ticket;
        }

        // Update an existing ticket
        public bool UpdateTicket(Guid id, MovieTicket updatedTicket)
        {
            var existingTicket = GetTicketById(id);
            if (existingTicket == null)
                return false;

            existingTicket.MovieName = updatedTicket.MovieName;
            existingTicket.CinemaName = updatedTicket.CinemaName;
            existingTicket.Time = updatedTicket.Time;
            existingTicket.Recipients = updatedTicket.Recipients;

            return true;
        }

        // Delete a ticket by ID
        public bool DeleteTicket(Guid id)
        {
            var ticket = GetTicketById(id);
            if (ticket == null)
                return false;

            _tickets.Remove(ticket);
            return true;
        }
    }
}
