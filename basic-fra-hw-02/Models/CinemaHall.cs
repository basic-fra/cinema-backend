using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class CinemaHall
    {
        public string HallId { get; set; } = Guid.NewGuid().ToString(); 
        public string CinemaId { get; set; } 
        public string Name { get; set; }
        public int Capacity { get; set; }

    }
}
