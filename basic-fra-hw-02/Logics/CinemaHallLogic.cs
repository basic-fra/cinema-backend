using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class CinemaHallLogic : ICinemaHallLogic
    {
        private readonly ICinemaHallService _cinemaHallService;
        private readonly ValidationConfiguration _validationConfiguration;

        public CinemaHallLogic(ICinemaHallService cinemaHallService, IOptions<ValidationConfiguration> configuration)
        {
            _cinemaHallService = cinemaHallService;
            _validationConfiguration = configuration.Value;
        }

        public async Task AddCinemaHallAsync(CinemaHall cinemaHall)
        {
            if (cinemaHall is null)
            {
                throw new UserErrorMessage("Cannot create a new cinema hall. No cinema hall specified.");
            }
            ValidateNameField(cinemaHall.Name);
            ValidateCapacityField(cinemaHall.Capacity);
            await _cinemaHallService.AddCinemaHallAsync(cinemaHall);
        }

        public async Task<List<CinemaHall>> GetAllCinemaHallsAsync()
        {
            return await _cinemaHallService.GetAllCinemaHallsAsync();
        }

        public async Task<CinemaHall?> GetCinemaHallByIdAsync(string id)
        {
            var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
            return cinemaHall == null
                ? throw new UserErrorMessage("Cinema hall not found.")
                : await _cinemaHallService.GetCinemaHallByIdAsync(id);
        }

        public async Task DeleteCinemaHallAsync(string id)
        {
            var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
            if (cinemaHall == null)
            {
                throw new UserErrorMessage("Cinema hall not found.");
            }

            await _cinemaHallService.DeleteCinemaHallAsync(id);
        }

        public async Task<bool> CheckIfCinemaExistsAsync(string cinemaId)
        {
            return await _cinemaHallService.CheckIfCinemaExistsAsync(cinemaId);
        }

        private void ValidateNameField(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new UserErrorMessage("Name field cannot be empty.");
            }

            if (name.Length > _validationConfiguration.HallMaxCharacters)
            {
                throw new UserErrorMessage($"Name field too long. Exceeded {_validationConfiguration.HallMaxCharacters} characters");
            }

            if (!Regex.IsMatch(name, _validationConfiguration.HallNameRegex))
            {
                throw new UserErrorMessage("Name can only contain letters,numbers and spaces.");
            }
        }

        private void ValidateCapacityField(int capacity)
        {
            if (capacity <= 0)
            {
                throw new UserErrorMessage("Capacity must be a positive number.");
            }
        }
    }
}
