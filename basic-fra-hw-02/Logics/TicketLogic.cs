using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class TicketLogic
    {
        public void ValidateTicket(Ticket ticket)
        {
            var seatNumPattern = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrEmpty(ticket.SeatNumber))
            {
                throw new ArgumentException("Seat number cannot be empty.");
            }

            if (!Regex.IsMatch(ticket.SeatNumber, seatNumPattern))
            {
                throw new ArgumentException("Seat number can only contain letters and numbers, without spaces.");
            }

        }
    }
}
