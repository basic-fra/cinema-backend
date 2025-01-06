using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Person
    {
        [Key]
        [JsonIgnore] // Hides this field from Swagger and JSON serialization
        public Guid PersonId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
