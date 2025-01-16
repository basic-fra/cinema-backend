using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Person
    {
        public string PersonId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
