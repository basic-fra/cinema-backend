using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Services;
using basic_fra_hw_02.Configuration;
using Microsoft.Extensions.Options;

public class CinemaHallService : ICinemaHallService
{
    private readonly string _connectionString;

    public CinemaHallService(IOptions<DBConfiguration> configuration)
    {
        _connectionString = configuration.Value.ConnectionString;
    }


    // Add CinemaHall
    public async Task AddCinemaHallAsync(CinemaHall cinemaHall)
    {
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
                            HallId = reader.GetString(0),
                            CinemaId = reader.GetString(1),
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
    public async Task<CinemaHall?> GetCinemaHallByIdAsync(string hallId)
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
                            HallId = reader.GetString(0),
                            CinemaId = reader.GetString(1),
                            Name = reader.GetString(2),
                            Capacity = reader.GetInt32(3)
                        };
                    }
                }
            }
        }

        return null;
    }

    // Delete CinemaHall
    public async Task DeleteCinemaHallAsync(string hallId)
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
    public async Task<bool> CheckIfCinemaExistsAsync(string cinemaId)
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
