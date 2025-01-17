using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class NewHallDTO
    {
        public string? CinemaId { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }

        public CinemaHall ToModel()
        {
            return new CinemaHall
            {
                CinemaId = CinemaId,
                Name = Name,
                Capacity = Capacity
            };
        }
    }
}
