using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Cinema
    {
        public string CinemaId { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Location { get; set; }
    }
}
