using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;

public class CinemaHallService
{
    private readonly string _connectionString;

    public CinemaHallService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Add CinemaHall
    public async Task AddCinemaHallAsync(CinemaHall cinemaHall)
    {
        var namePattern = @"^[a-zA-Z0-9\s]+$";
        var capPattern = @"^\d+$"; //regex only numbers

        if (string.IsNullOrEmpty(cinemaHall.Name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (!Regex.IsMatch(cinemaHall.Name, namePattern))
        {
            throw new ArgumentException("Name can only contain letters, spaces and numbers.");
        }

        if (!Regex.IsMatch(cinemaHall.Capacity.ToString(), capPattern))
        {
            throw new ArgumentException("Capacity can only contain numbers.");
        }

        // Check if the CinemaId exists in the Cinema table
        var cinemaExists = await CheckIfCinemaExistsAsync(cinemaHall.CinemaId);
        if (!cinemaExists)
        {
            throw new Exception("Cinema does not exist");
        }

        // Proceed to add the CinemaHall
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                INSERT INTO CINEMA_HALL (hall_id, cinema_id, name, capacity)
                VALUES (@HallId, @CinemaId, @Name, @Capacity)";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HallId", cinemaHall.HallId);
                command.Parameters.AddWithValue("@CinemaId", cinemaHall.CinemaId);
                command.Parameters.AddWithValue("@Name", cinemaHall.Name);
                command.Parameters.AddWithValue("@Capacity", cinemaHall.Capacity);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    // Get all CinemaHalls
    public async Task<List<CinemaHall>> GetAllCinemaHallsAsync()
    {
        var cinemaHalls = new List<CinemaHall>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT hall_id, cinema_id, name, capacity FROM CINEMA_HALL ";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cinemaHalls.Add(new CinemaHall
                        {
                            HallId = reader.GetGuid(0),
                            CinemaId = reader.GetGuid(1),
                            Name = reader.GetString(2),
                            Capacity = reader.GetInt32(3)
                        });
                    }
                }
            }
        }

        return cinemaHalls;
    }

    // Get CinemaHall by Id
    public async Task<CinemaHall> GetCinemaHallByIdAsync(Guid hallId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT hall_id, cinema_id, name, capacity FROM CINEMA_HALL WHERE hall_id = @HallId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HallId", hallId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new CinemaHall
                        {
                            HallId = reader.GetGuid(0),
                            CinemaId = reader.GetGuid(1),
                            Name = reader.GetString(2),
                            Capacity = reader.GetInt32(3)
                        };
                    }
                }
            }
        }

        return null;
    }

    // Update CinemaHall
    public async Task UpdateCinemaHallAsync(CinemaHall cinemaHall)
    {
        var namePattern = @"^[a-zA-Z0-9\s]+$";
        var capPattern = @"^\d+$"; //regex only numbers

        if (string.IsNullOrEmpty(cinemaHall.Name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (!Regex.IsMatch(cinemaHall.Name, namePattern))
        {
            throw new ArgumentException("Name can only contain letters, spaces and numbers.");
        }

        if (!Regex.IsMatch(cinemaHall.Capacity.ToString(), capPattern))
        {
            throw new ArgumentException("Capacity can only contain numbers.");
        }
        // Check if the CinemaId exists in the Cinema table
        var cinemaExists = await CheckIfCinemaExistsAsync(cinemaHall.CinemaId);
        if (!cinemaExists)
        {
            throw new Exception("Cinema does not exist");
        }

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            UPDATE CINEMA_HALL
            SET name = @Name, cinema_id = @CinemaId, capacity = @Capacity 
            WHERE hall_id = @HallId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HallId", cinemaHall.HallId);
                command.Parameters.AddWithValue("@CinemaId", cinemaHall.CinemaId);
                command.Parameters.AddWithValue("@Name", cinemaHall.Name);
                command.Parameters.AddWithValue("@Capacity", cinemaHall.Capacity);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("CinemaHall not found");
                }
            }
        }
    }

    // Delete CinemaHall
    public async Task DeleteCinemaHallAsync(Guid hallId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "DELETE FROM CINEMA_HALL WHERE hall_id = @HallId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HallId", hallId);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new Exception("CinemaHall not found");
                }
            }
        }
    }

    // Check if Cinema exists by CinemaId
    private async Task<bool> CheckIfCinemaExistsAsync(Guid cinemaId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM CINEMA WHERE cinema_id = @CinemaId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CinemaId", cinemaId);

                var result = (long)await command.ExecuteScalarAsync();
                return result > 0;
            }
        }
    }
}
