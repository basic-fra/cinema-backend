using basic_fra_hw_02.Models;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class CinemaDTO
    {
        public string? CinemaId { get; set; } 
        public string? Name { get; set; }
        public string? Location { get; set; }

        public static CinemaDTO FromModel(Cinema model) 
        {
            return new CinemaDTO
            {
                CinemaId = model.CinemaId,
                Name = model.Name,
                Location = model.Location
            };
        }
    }
}
