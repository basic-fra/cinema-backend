using System.Text.Json.Serialization;
using basic_fra_hw_02.Helpers;
using System.Collections.Generic;

namespace basic_fra_hw_02.Models
{
    public class MovieTicket
    {
        public Guid Id { get; set; }  // Unique identifier using GUID
        public string MovieName { get; set; }  // Name of the movie
        public string CinemaName { get; set; }  // Name of the cinema

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Time { get; set; }  // Show time of the movie

        public List<Recipient> Recipients { get; set; } = new List<Recipient>(); // List of recipients
    }

    // New Recipient class to hold recipient details
    public class Recipient
    {
        public string Name { get; set; } // Name of the recipient
        public int SeatNumber { get; set; } // Optional: seat number assigned to the recipient
    }
}
