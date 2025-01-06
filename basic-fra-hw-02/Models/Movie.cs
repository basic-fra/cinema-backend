using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Movie
    {
        [JsonIgnore]
        public Guid MovieId { get; set; }
        public Guid CinemaId { get; set; }
        public Guid HallId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
    }
}
