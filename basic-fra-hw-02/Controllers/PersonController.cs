using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonController()
    {
        string connectionString = "Data Source=CinemaDb.sqlite;";
        _personService = new PersonService(connectionString);
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] Person person)
    {
        await _personService.AddPersonAsync(person);
        return Ok("Person added successfully!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return Ok(persons);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPersonById(Guid id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
        if (person == null)
            return NotFound(new { Message = "Person not found" });

        return Ok(person);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await _personService.DeletePersonAsync(id);
        return Ok("Person deleted successfully!");
    }
}
