using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class TicketLogic : ITicketLogic
    {
        private readonly ITicketService _ticketService;
        private readonly ValidationConfiguration _validationConfiguration;

        public TicketLogic(ITicketService ticketService, IOptions<ValidationConfiguration> configuration)
        {
            _ticketService = ticketService;
            _validationConfiguration = configuration.Value;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            ValidateSeatNumberField(ticket.SeatNumber);
            await _ticketService.AddTicketAsync(ticket);
        }
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketService.GetAllTicketsAsync();   
        }
        public async Task<Ticket?> GetTicketByIdAsync(string id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                throw new UserErrorMessage("Ticket not found.");
            }
            return (ticket);
        }
        public async Task DeleteTicketAsync(string id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if(ticket == null)
            {
                throw new UserErrorMessage("Ticket not found.");
            }

            await _ticketService.DeleteTicketAsync(id);
        }
        private void ValidateSeatNumberField(string seatNumber)
        {
            if (string.IsNullOrEmpty(seatNumber))
            {
                throw new UserErrorMessage("Seat number cannot be empty.");
            }

            if (!Regex.IsMatch(seatNumber, _validationConfiguration.TicketSeatNumberRegex))
            {
                throw new UserErrorMessage("Seat number can only contain letters and numbers, without spaces (ex 13A).");
            }
        }
    }
}
