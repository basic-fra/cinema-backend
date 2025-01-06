using basic_fra_hw_02.Models;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

public class TicketService
{
    private readonly string _connectionString;

    public TicketService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddTicketAsync(Ticket ticket)
    {
        ticket.TicketId = Guid.NewGuid(); // Generate a unique TicketId
        using (var connection = new SqliteConnection(_connectionString))
        {
            var seatNumPattern = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrEmpty(ticket.SeatNumber))
            {
                throw new ArgumentException("Seat number cannot be empty.");
            }

            if (!Regex.IsMatch(ticket.SeatNumber, seatNumPattern))
            {
                throw new ArgumentException("Seat number can only contain letters and numbers, without spaces.");
            }


            await connection.OpenAsync();

            var query = @"
                INSERT INTO TICKET (ticket_id, person_id, movie_id, hall_id, show_time, seat_number)
                VALUES (@TicketId, @PersonId, @MovieId, @HallId, @ShowTime, @SeatNumber)";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                command.Parameters.AddWithValue("@PersonId", ticket.PersonId);
                command.Parameters.AddWithValue("@MovieId", ticket.MovieId);
                command.Parameters.AddWithValue("@HallId", ticket.HallId);
                command.Parameters.AddWithValue("@ShowTime", ticket.ShowTime);
                command.Parameters.AddWithValue("@SeatNumber", ticket.SeatNumber);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        var tickets = new List<Ticket>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT ticket_id, person_id, movie_id, hall_id, show_time, seat_number FROM TICKET";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tickets.Add(new Ticket
                        {
                            TicketId = reader.GetGuid(0),
                            PersonId = reader.GetGuid(1),
                            MovieId = reader.GetGuid(2),
                            HallId = reader.GetGuid(3),
                            ShowTime = reader.GetDateTime(4),
                            SeatNumber = reader.GetString(5)
                        });
                    }
                }
            }
        }

        return tickets;
    }

    public async Task<Ticket> GetTicketByIdAsync(Guid ticketId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT ticket_id, person_id, movie_id, hall_id, show_time, seat_number FROM TICKET WHERE ticket_id = @TicketId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticketId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Ticket
                        {
                            TicketId = reader.GetGuid(0),
                            PersonId = reader.GetGuid(1),
                            MovieId = reader.GetGuid(2),
                            HallId = reader.GetGuid(3),
                            ShowTime = reader.GetDateTime(4),
                            SeatNumber = reader.GetString(5)
                        };
                    }
                }
            }
        }
        return null; // Return null if the ticket is not found
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var seatNumPattern = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrEmpty(ticket.SeatNumber))
            {
                throw new ArgumentException("Seat number cannot be empty.");
            }

            if (!Regex.IsMatch(ticket.SeatNumber, seatNumPattern))
            {
                throw new ArgumentException("Seat number can only contain letters and numbers, without spaces.");
            }

            await connection.OpenAsync();

            var query = @"
                UPDATE TICKET
                SET person_id = @PersonId, movie_id = @MovieId, hall_id = @HallId, show_time = @ShowTime, seat_number = @SeatNumber
                WHERE ticket_id = @TicketId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                command.Parameters.AddWithValue("@PersonId", ticket.PersonId);
                command.Parameters.AddWithValue("@MovieId", ticket.MovieId);
                command.Parameters.AddWithValue("@HallId", ticket.HallId);
                command.Parameters.AddWithValue("@ShowTime", ticket.ShowTime);
                command.Parameters.AddWithValue("@SeatNumber", ticket.SeatNumber);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Ticket not found");
                }
            }
        }
    }

    public async Task DeleteTicketAsync(Guid ticketId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "DELETE FROM TICKET WHERE ticket_id = @TicketId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TicketId", ticketId);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Ticket not found");
                }
            }
        }
    }
}