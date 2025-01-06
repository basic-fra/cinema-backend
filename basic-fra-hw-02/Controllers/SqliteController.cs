using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SqliteController : ControllerBase
{
    private readonly CinemaService _cinemaService;
    private readonly CinemaHallService _cinemaHallService;
    private readonly PersonService _personService;
    private readonly MovieService _movieService;
    private readonly TicketService _ticketService;

    public SqliteController()
    {
        // Set the SQLite connection string
        string connectionString = "Data Source=CinemaDb.sqlite;";
        _cinemaService = new CinemaService(connectionString);
        _cinemaHallService = new CinemaHallService(connectionString);
        _personService = new PersonService(connectionString);
        _movieService = new MovieService(connectionString);
        _ticketService = new TicketService(connectionString);
    }

    [HttpPost("cinema")]
    public async Task<IActionResult> CreateCinema([FromBody] Cinema cinema)
    {
        cinema.CinemaId = Guid.NewGuid();
        await _cinemaService.AddCinemaAsync(cinema);
        return CreatedAtAction(nameof(GetCinemaById), new { id = cinema.CinemaId }, cinema);
    }

    [HttpGet("cinema")]
    public async Task<IActionResult> GetAllCinemas()
    {
        var cinemas = await _cinemaService.GetAllCinemasAsync();
        return Ok(cinemas);
    }

    [HttpGet("cinema/{id:guid}")]
    public async Task<IActionResult> GetCinemaById(Guid id)
    {
        var cinema = await _cinemaService.GetCinemaByIdAsync(id);
        if (cinema == null)
            return NotFound(new { Message = "Cinema not found" });

        return Ok(cinema);
    }

    [HttpPut("cinema/{id:guid}")]
    public async Task<IActionResult> UpdateCinema(Guid id, [FromBody] Cinema cinema)
    {
        // Ensure the ID from the route is applied to the Cinema object
        cinema.CinemaId = id;

        try
        {
            // Step 1: Check if the cinema exists in the database
            var cinemaExists = await _cinemaService.CheckIfCinemaExistsAsync(id);
            if (!cinemaExists)
            {
                return NotFound(new { Message = "Cinema not found" });
            }

            // Step 2: Proceed with the update
            await _cinemaService.UpdateCinemaAsync(cinema);
            return Ok("Cinema updated successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("cinema/{id:guid}")]
    public async Task<IActionResult> DeleteCinema(Guid id)
    {
        var cinema = await _cinemaService.GetCinemaByIdAsync(id);
        if (cinema == null)
            return NotFound(new { Message = "Cinema not found" });

        await _cinemaService.DeleteCinemaAsync(id);
        return Ok(new { Message = "Cinema deleted successfully" });
    }

    [HttpPost("cinemahall")]
    public async Task<IActionResult> CreateCinemaHall([FromBody] CinemaHall cinemaHall)
    {
        try
        {
            if (cinemaHall.HallId == Guid.Empty)
            {
                cinemaHall.HallId = Guid.NewGuid(); // Generate new HallId if not provided
            }

            // Check if the CinemaId exists
            var cinemaExists = await _cinemaService.CheckIfCinemaExistsAsync(cinemaHall.CinemaId);
            if (!cinemaExists)
            {
                return BadRequest(new { Message = "Cinema not found" });
            }

            // Add the CinemaHall
            await _cinemaHallService.AddCinemaHallAsync(cinemaHall);
            return CreatedAtAction(nameof(GetCinemaHallById), new { id = cinemaHall.HallId }, cinemaHall);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("cinemahall")]
    public async Task<IActionResult> GetAllCinemaHalls()
    {
        try
        {
            var cinemaHalls = await _cinemaHallService.GetAllCinemaHallsAsync();
            return Ok(cinemaHalls);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("cinemahall/{id:guid}")]
    public async Task<IActionResult> GetCinemaHallById(Guid id)
    {
        try
        {
            var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
            if (cinemaHall == null)
            {
                return NotFound(new { Message = "CinemaHall not found" });
            }
            return Ok(cinemaHall);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut("cinemahall/{id:guid}")]
    public async Task<IActionResult> UpdateCinemaHall(Guid id, [FromBody] CinemaHall cinemaHall)
    {
        // Ensure the ID from the route is applied to the CinemaHall object
        cinemaHall.HallId = id;

        try
        {
            // Step 1: Check if the CinemaHall exists in the database
            var existingCinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
            if (existingCinemaHall == null)
            {
                return NotFound(new { Message = "CinemaHall not found" });
            }

            // Step 2: Check if the CinemaId exists in the Cinema table
            var cinemaExists = await _cinemaService.CheckIfCinemaExistsAsync(cinemaHall.CinemaId);
            if (!cinemaExists)
            {
                return BadRequest(new { Message = "Cinema not found" });
            }

            // Step 3: Proceed with the update if both CinemaHall and Cinema are valid
            await _cinemaHallService.UpdateCinemaHallAsync(cinemaHall);
            return Ok(new { Message = "CinemaHall updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
    
    [HttpDelete("cinemahall/{id:guid}")]
    public async Task<IActionResult> DeleteCinemaHall(Guid id)
    {
        try
        {
            var cinemaHall = await _cinemaHallService.GetCinemaHallByIdAsync(id);
            if (cinemaHall == null)
            {
                return NotFound(new { Message = "CinemaHall not found" });
            }

            await _cinemaHallService.DeleteCinemaHallAsync(id);
            return Ok("CinemaHall deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPost("movie")]
    public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
    {
        movie.MovieId = Guid.NewGuid();
        await _movieService.AddMovieAsync(movie);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.MovieId }, movie);
    }

    [HttpGet("movie")]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [HttpGet("movie/{id:guid}")]
    public async Task<IActionResult> GetMovieById(Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
        {
            return NotFound(new { Message = "Movie not found" });
        }

        return Ok(movie);
    }

    [HttpPut("movie/{id:guid}")]
    public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] Movie movie)
    {
        // Ensure the ID from the route is applied to the Movie object
        movie.MovieId = id;

        try
        {
            // Step 1: Check if the movie exists in the database
            var movieExists = await _movieService.CheckIfMovieExistsAsync(id);
            if (!movieExists)
            {
                return NotFound(new { Message = "Movie not found" });
            }

            // Step 2: Proceed with the update
            await _movieService.UpdateMovieAsync(movie);
            return Ok(new { Message = "Movie updated successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("movie/{id:guid}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        try
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound(new { Message = "Movie not found" });
            }

            await _movieService.DeleteMovieAsync(id);
            return Ok(new { Message = "Movie deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPost("person")]
    public async Task<IActionResult> AddPerson([FromBody] Person person)
    {
        await _personService.AddPersonAsync(person);
        return Ok("Person added successfully!");
    }

    [HttpGet("person")]
    public async Task<IActionResult> GetAllPersons()
    {
        try
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("person/{id:guid}")]
    public async Task<IActionResult> GetPersonById(Guid id)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound(new { Message = "Person not found" });
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut("person/{id:guid}")]
    public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] Person person)
    {
        // Ensure the ID from the route is applied to the Person object
        person.PersonId = id;

        try
        {
            // Step 1: Check if the person exists in the database
            var personExists = await _personService.CheckIfPersonExistsAsync(id);
            if (!personExists)
            {
                return NotFound(new { Message = "Person not found" });
            }

            // Step 2: Proceed with the update
            await _personService.UpdatePersonAsync(person);
            return Ok("Person updated successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("person/{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        try
        {
            await _personService.DeletePersonAsync(id);
            return Ok("Person deleted successfully!");
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., person not found)
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPost("ticket")]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
        ticket.TicketId = Guid.NewGuid(); // Generate a new TicketId
        await _ticketService.AddTicketAsync(ticket);
        return CreatedAtAction(nameof(GetTicketById), new { id = ticket.TicketId }, ticket);
    }

    [HttpGet("ticket")]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("ticket/{id:guid}")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound(new { Message = "Ticket not found" });

        return Ok(ticket);
    }

    [HttpPut("ticket/{id:guid}")]
    public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] Ticket ticket)
    {
        ticket.TicketId = id;

        try
        {
            await _ticketService.UpdateTicketAsync(ticket);
            return Ok("Ticket updated successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("ticket/{id:guid}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        try
        {
            await _ticketService.DeleteTicketAsync(id);
            return Ok("Ticket deleted successfully!");
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
