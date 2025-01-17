using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class NewTicketDTO
    {
        public string? PersonId { get; set; }
        public string? MovieId { get; set; }
        public DateTime ShowTime { get; set; }
        public string? SeatNumber { get; set; }

        public Ticket ToModel()
        {
            return new Ticket
            {
                PersonId = PersonId,
                MovieId = MovieId,
                ShowTime = ShowTime,
                SeatNumber = SeatNumber
            };
        }
    }
}
