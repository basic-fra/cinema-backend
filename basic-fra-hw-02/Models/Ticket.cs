using basic_fra_hw_02.Helpers;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Ticket
    {
        [JsonIgnore] // Hides this field from Swagger and JSON serialization
        public Guid TicketId { get; set; }
        public Guid PersonId { get; set; }
        public Guid MovieId { get; set; }

        [JsonIgnore]
        public Guid HallId { get; set; }

        [JsonIgnore]
        public Guid CinemaId { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ShowTime { get; set; }
        public string SeatNumber { get; set; }

    }
}
