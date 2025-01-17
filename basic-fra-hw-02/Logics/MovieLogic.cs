using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;

namespace basic_fra_hw_02.Logics
{
    public class MovieLogic : IMovieLogic
    {
        private readonly IMovieService _movieService;
        private readonly ValidationConfiguration _validationConfiguration;

        public MovieLogic(IMovieService movieService, IOptions<ValidationConfiguration> configuration)
        {
            _movieService = movieService;
            _validationConfiguration = configuration.Value;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            ValidateTitleField(movie.Title);
            ValidateDescriptionField(movie.Description);
            ValidateDurationField(movie.Duration);
            await _movieService.AddMovieAsync(movie);
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _movieService.GetAllMoviesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(string id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new UserErrorMessage("Movie not found.");
            }
            return await _movieService.GetMovieByIdAsync(id);
        }

        public async Task DeleteMovieAsync(string id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new UserErrorMessage("Movie not found.");
            }

            await _movieService.DeleteMovieAsync(id);
        }

        private void ValidateTitleField(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new UserErrorMessage("Title cannot be empty.");
            }

            if (title.Length > _validationConfiguration.MovieMaxCharacters)
            {
                throw new UserErrorMessage($"Title field too long. Exceeded {_validationConfiguration.MovieMaxCharacters} characters");
            }
        }

        private void ValidateDescriptionField(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new UserErrorMessage("Description cannot be empty.");
            }
        }

        private void ValidateDurationField(int duration)
        {
            if(duration <= 0)
            {
                throw new UserErrorMessage("Duration must be positive number");
            }
        }
    }
}
