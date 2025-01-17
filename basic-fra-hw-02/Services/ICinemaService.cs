using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Services
{
    public interface ICinemaService
    {
        Task AddCinemaAsync(Cinema cinema);
        Task<List<Cinema>> GetAllCinemasAsync();
        Task<Cinema?> GetCinemaByIdAsync(string id);
        Task DeleteCinemaAsync(string id);
    }
}
