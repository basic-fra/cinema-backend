using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace basic_fra_hw_02.Services
{
    public class CinemaService
    {
        private readonly string _connectionString;
        private readonly CinemaLogic _cinemaLogic;
        public CinemaService(string connectionString)
        {
            _connectionString = connectionString;
            _cinemaLogic = new CinemaLogic();
        }

        // Add a new cinema
        public async Task AddCinemaAsync(Cinema cinema)
        {
            // Delegate validation to CinemaLogic
            _cinemaLogic.ValidateCinema(cinema);

            cinema.CinemaId = Guid.NewGuid();

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
                                CinemaId = reader.GetGuid(0),
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
        public async Task<Cinema?> GetCinemaByIdAsync(Guid cinemaId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT cinema_id, name, location FROM CINEMA WHERE cinema_id = @CinemaId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CinemaId", cinemaId);// Convert Guid to string .ToString()

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cinema
                            {
                                //CinemaId = Guid.Parse(reader.GetString(0)),  Parse string to Guid
                                CinemaId = reader.GetGuid(0),
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
        public async Task DeleteCinemaAsync(Guid cinemaId)
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
        public async Task<bool> CheckIfCinemaExistsAsync(Guid cinemaId)
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
