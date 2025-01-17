using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Logics
{
    public interface ICinemaHallLogic
    {
        Task AddCinemaHallAsync(CinemaHall cinemaHall);
        Task<List<CinemaHall>> GetAllCinemaHallsAsync();
        Task<CinemaHall?> GetCinemaHallByIdAsync(string id);
        Task DeleteCinemaHallAsync(string id);
        Task<bool> CheckIfCinemaExistsAsync(string cinemaId);
    }
}
