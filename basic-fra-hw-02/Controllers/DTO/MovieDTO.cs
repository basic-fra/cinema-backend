using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class MovieDTO
    {
        public string? MovieId { get; set; } 
        public string? CinemaId { get; set; } 
        public string? HallId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }

        public static MovieDTO FromModel(Movie model)
        {
            return new MovieDTO
            {
                MovieId = model.MovieId,
                CinemaId = model.CinemaId,
                HallId = model.HallId,
                Title = model.Title,
                Description = model.Description,
                Duration = model.Duration
            };
        }
    }
}
