using basic_fra_hw_02.Models;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Services
{
    public class CinemaService
    {
        private readonly string _connectionString;

        public CinemaService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new cinema
        public async Task AddCinemaAsync(Cinema cinema)

        {
            // Validate the Name field using regex
            var namePattern = @"^[a-zA-Z\s]+$"; // This regex allows only letters and spaces

            if (string.IsNullOrEmpty(cinema.Name) || string.IsNullOrEmpty(cinema.Location))
            {
                throw new ArgumentException("Cinema name and location cannot be empty.");
            }

            if (!Regex.IsMatch(cinema.Name, namePattern))
            {
                throw new ArgumentException("Cinema name can only contain letters and spaces.");
            }

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

                    await command.ExecuteNonQueryAsync();
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
                    command.Parameters.AddWithValue("@CinemaId", cinemaId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cinema
                            {
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

        //Update cinema
        public async Task UpdateCinemaAsync(Cinema cinema)
        {
            var namePattern = @"^[a-zA-Z\s]+$"; // This regex allows only letters and spaces

            if (string.IsNullOrEmpty(cinema.Name) || string.IsNullOrEmpty(cinema.Location))
            {
                throw new ArgumentException("Cinema name and location cannot be empty.");
            }

            if (!Regex.IsMatch(cinema.Name, namePattern))
            {
                throw new ArgumentException("Cinema name can only contain letters and spaces.");
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                UPDATE CINEMA
                SET name = @Name, location = @Location
                WHERE cinema_id = @CinemaId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CinemaId", cinema.CinemaId);
                    command.Parameters.AddWithValue("@Name", cinema.Name);
                    command.Parameters.AddWithValue("@Location", cinema.Location);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Cinema not found");
                    }
                }
            }
        }

        //Delete cinema by ID
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
