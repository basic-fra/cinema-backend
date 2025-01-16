using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class TicketDTO
    {
        public string? TicketId { get; set; }
        public string? PersonId { get; set; } 
        public string? MovieId { get; set; } 
        public string? HallId { get; set; } 
        public string? CinemaId { get; set; } 
        public DateTime ShowTime { get; set; }
        public string? SeatNumber { get; set; }

        public static TicketDTO FromModel(Ticket model)
        {
            return new TicketDTO
            {
                TicketId = model.TicketId,
                PersonId = model.PersonId,
                MovieId = model.MovieId,
                HallId = model.HallId,
                CinemaId = model.CinemaId,
                ShowTime = model.ShowTime,
                SeatNumber = model.SeatNumber
            };
        }
    }
}
