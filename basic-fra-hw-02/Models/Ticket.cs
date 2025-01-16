using basic_fra_hw_02.Helpers;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Ticket
    {
        public string TicketId { get; set; } = Guid.NewGuid().ToString();
        public string PersonId { get; set; } = Guid.NewGuid().ToString();
        public string MovieId { get; set; } = Guid.NewGuid().ToString();    
        public string HallId { get; set; } = Guid.NewGuid().ToString();
        public string CinemaId { get; set; } = Guid.NewGuid().ToString();

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ShowTime { get; set; }
        public string SeatNumber { get; set; }

    }
}
