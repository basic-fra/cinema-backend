using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class HallDTO
    {
        public string? HallId { get; set; } 
        public string? CinemaId { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }

        public static HallDTO FromModel(CinemaHall model)
        {
            return new HallDTO
            {
                HallId = model.HallId,
                CinemaId = model.CinemaId,
                Name = model.Name,
                Capacity = model.Capacity
            };
        }
    }
}
