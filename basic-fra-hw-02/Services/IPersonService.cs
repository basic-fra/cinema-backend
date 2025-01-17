using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Services
{
    public interface IPersonService
    {
        Task AddPersonAsync(Person person);
        Task<List<Person>> GetAllPersonsAsync();
        Task<Person> GetPersonByIdAsync(string id);
        Task DeletePersonAsync(string id);
    }
}
