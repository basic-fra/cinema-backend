using basic_fra_hw_02.Controllers.DTO;
using basic_fra_hw_02.Filters;
using basic_fra_hw_02.Logics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
//[LogFilter]
public class PersonController : ControllerBase
{
    private readonly IPersonLogic _personLogic;

    public PersonController(IPersonLogic personLogic)
    {
        _personLogic = personLogic;
    }

    [HttpPost]
    public async Task<ActionResult> AddPerson([FromBody] NewPersonDTO person)
    {
        await _personLogic.AddPersonAsync(person.ToModel());
        return Ok("Person added successfully!");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons()
    {
        var persons = (await _personLogic.GetAllPersonsAsync()).Select(x => PersonDTO.FromModel(x)); ;
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDTO>> GetPersonById(string id)
    {
        var person = await _personLogic.GetPersonByIdAsync(id);
        return Ok(PersonDTO.FromModel(person));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
        await _personLogic.DeletePersonAsync(id);
        return Ok("Person deleted successfully!");
    }
}
