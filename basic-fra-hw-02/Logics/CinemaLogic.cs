using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class CinemaLogic : ICinemaLogic
    {
        private readonly ICinemaService _cinemaService;
        private readonly ValidationConfiguration _validationConfiguration;

        public CinemaLogic(ICinemaService cinemaService, IOptions<ValidationConfiguration> configuration)
        {
            _cinemaService = cinemaService;
            _validationConfiguration = configuration.Value;
        }

        public async Task AddCinemaAsync(Cinema cinema)
        {
            if(cinema is null)
            {
                throw new UserErrorMessage("Cannot create a new cinema. No cinema specified.");
            }
            ValidateNameField(cinema.Name);
            ValidateLocationField(cinema.Location);
            await _cinemaService.AddCinemaAsync(cinema);
        }

        public async Task<List<Cinema>> GetAllCinemasAsync()
        {
            return await _cinemaService.GetAllCinemasAsync();
        }

        public async Task<Cinema?> GetCinemaByIdAsync(string id)
        {
            var cinema = await _cinemaService.GetCinemaByIdAsync(id);
            if (cinema == null)
            {
                throw new UserErrorMessage("Cinema not found.");
            }
            return cinema;
        }

        public async Task DeleteCinemaAsync(string id)
        {
            var cinema = await _cinemaService.GetCinemaByIdAsync(id);
            if (cinema == null)
            {
                throw new UserErrorMessage("Cinema not found.");
            }

            await _cinemaService.DeleteCinemaAsync(id);
        }

        private void ValidateNameField(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new UserErrorMessage("Cinema name cannot be empty.");
            }

            if (name.Length > _validationConfiguration.CinemaMaxCharacters)
            {
                throw new UserErrorMessage($"Cinema name is too long. Exceeded {_validationConfiguration.CinemaMaxCharacters} characters");
            }
        }

        private void ValidateLocationField(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new UserErrorMessage("Location cannot be empty.");
            }

            if (!Regex.IsMatch(location, _validationConfiguration.CinemaLocationRegex))
            {
                throw new UserErrorMessage($"Location can only contain letters,numbers and spaces");
            }
        }

    }
}
