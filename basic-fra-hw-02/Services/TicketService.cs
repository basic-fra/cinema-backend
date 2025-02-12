﻿using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

public class TicketService : ITicketService
{
    private readonly string _connectionString;

    public TicketService(IOptions<DBConfiguration> configuration)
    {
        _connectionString = configuration.Value.ConnectionString;
    }

    public async Task AddTicketAsync(Ticket ticket)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Step 1: Fetch hall_id and cinema_id based on movie_id
            var fetchQuery = @"
            SELECT hall_id, cinema_id 
            FROM MOVIE
            WHERE movie_id = @MovieId";

            string hallId, cinemaId;
            using (var fetchCommand = new SqliteCommand(fetchQuery, connection))
            {
                fetchCommand.Parameters.AddWithValue("@MovieId", ticket.MovieId);

                using (var reader = await fetchCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        hallId = reader["hall_id"].ToString();
                        cinemaId = reader["cinema_id"].ToString();
                    }
                    else
                    {
                        throw new ArgumentException("Invalid movie_id. No movie found for the given movie_id.");
                    }
                }
            }

            // Step 2: Insert ticket with fetched hall_id and cinema_id
            var insertQuery = @"
            INSERT INTO TICKET (ticket_id, person_id, movie_id, hall_id, cinema_id, show_time, seat_number)
            VALUES (@TicketId, @PersonId, @MovieId, @HallId, @CinemaId, @ShowTime, @SeatNumber)";

            using (var insertCommand = new SqliteCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                insertCommand.Parameters.AddWithValue("@PersonId", ticket.PersonId);
                insertCommand.Parameters.AddWithValue("@MovieId", ticket.MovieId);
                insertCommand.Parameters.AddWithValue("@HallId", hallId);
                insertCommand.Parameters.AddWithValue("@CinemaId", cinemaId);
                insertCommand.Parameters.AddWithValue("@ShowTime", ticket.ShowTime);
                insertCommand.Parameters.AddWithValue("@SeatNumber", ticket.SeatNumber);

                await insertCommand.ExecuteNonQueryAsync();
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
                            TicketId = reader.GetString(0),
                            PersonId = reader.GetString(1),
                            MovieId = reader.GetString(2),
                            HallId = reader.GetString(3),
                            ShowTime = reader.GetDateTime(4),
                            SeatNumber = reader.GetString(5)
                        });
                    }
                }
            }
        }

        return tickets;
    }

    public async Task<Ticket?> GetTicketByIdAsync(string ticketId)
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
                            TicketId = reader.GetString(0),
                            PersonId = reader.GetString(1),
                            MovieId = reader.GetString(2),
                            HallId = reader.GetString(3),
                            ShowTime = reader.GetDateTime(4),
                            SeatNumber = reader.GetString(5)
                        };
                    }
                }
            }
        }
        return null; // Return null if the ticket is not found
    }

    public async Task DeleteTicketAsync(string ticketId)
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