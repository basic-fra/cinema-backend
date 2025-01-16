using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class NewMovieDTO
    {
        public string? HallId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }

        public Movie ToModel()
        {
            return new Movie
            {
                HallId = HallId,
                Title = Title,
                Description = Description,
                Duration = Duration
            };
        }
    }
}
