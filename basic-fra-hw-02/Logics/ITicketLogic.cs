using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Logics
{
    public interface ITicketLogic
    {
        Task AddTicketAsync(Ticket ticket);
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(string id);
        Task DeleteTicketAsync(string id);
    }
}
