using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class CinemaHall
    {
        [Key]
        [JsonIgnore] // Hides this field from Swagger and JSON serialization
        public Guid HallId { get; set; }  // This will be the primary key
        public Guid CinemaId { get; set; } // Foreign key to the Cinema table
        public string Name { get; set; }
        public int Capacity { get; set; }

    }
}
