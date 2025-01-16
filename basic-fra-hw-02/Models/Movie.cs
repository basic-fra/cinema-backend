using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Movie
    {
        public string MovieId { get; set; } = Guid.NewGuid().ToString();
        public string CinemaId { get; set; } = Guid.NewGuid().ToString();
        public string HallId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
    }
}
