using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace basic_fra_hw_02.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly string _connectionString;

        public CinemaService(IOptions<DBConfiguration> configuration)
        {
            _connectionString = configuration.Value.ConnectionString;
        }

        // Add a new cinema
        public async Task AddCinemaAsync(Cinema cinema)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO CINEMA (cinema_id, name, location)
                    VALUES (@CinemaId, @Name, @Location)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CinemaId", cinema.CinemaId);
                    command.Parameters.AddWithValue("@Name", cinema.Name);
                    command.Parameters.AddWithValue("@Location", cinema.Location);

                    await command.ExecuteNonQueryAsync(); //Used for commands that do not return results
                }
            }
        }

        // Get all cinemas
        public async Task<List<Cinema>> GetAllCinemasAsync()
        {
            var cinemas = new List<Cinema>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT cinema_id, name, location FROM CINEMA";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cinemas.Add(new Cinema
                            {
                                CinemaId = reader.GetString(0),
                                Name = reader.GetString(1),
                                Location = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return cinemas;
        }

        // Get cinema by ID
        public async Task<Cinema?> GetCinemaByIdAsync(string cinemaId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT cinema_id, name, location FROM CINEMA WHERE cinema_id = @CinemaId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CinemaId", cinemaId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cinema
                            {
                                CinemaId = reader.GetString(0),
                                Name = reader.GetString(1),
                                Location = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null; // Return null if cinema not found
        }

        // Delete cinema by ID
        public async Task DeleteCinemaAsync(string cinemaId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM CINEMA WHERE cinema_id = @CinemaId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CinemaId", cinemaId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Cinema not found");
                    }
                }
            }
        }

        // Check if cinema exists
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
}
