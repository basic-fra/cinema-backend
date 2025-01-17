using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace basic_fra_hw_02.Services
{
  public class MovieService : IMovieService
  {
     private readonly string _connectionString;
     public MovieService(IOptions<DBConfiguration> configuration)
     {
          _connectionString = configuration.Value.ConnectionString;
     }

     public async Task AddMovieAsync(Movie movie)
     {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Step 1: Fetch cinema_id based on hall_id
                var fetchCinemaIdQuery = "SELECT cinema_id FROM CINEMA_HALL WHERE hall_id = @HallId";

                string cinemaId;
                using (var fetchCommand = new SqliteCommand(fetchCinemaIdQuery, connection))
                {
                    fetchCommand.Parameters.AddWithValue("@HallId", movie.HallId);
                    cinemaId = (string)await fetchCommand.ExecuteScalarAsync();
                    if (cinemaId == null)
                    {
                        throw new UserErrorMessage("Invalid hall_id. Cinema not found for the given hall_id.");
                    }
                }

                // Step 2: Insert movie using fetched cinema_id
                var insertMovieQuery = @"
                   INSERT INTO MOVIE (movie_id, cinema_id, hall_id, title, description, duration)
                   VALUES (@MovieId, @CinemaId, @HallId, @Title, @Description, @Duration)";

                using (var insertCommand = new SqliteCommand(insertMovieQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@MovieId", movie.MovieId);
                    insertCommand.Parameters.AddWithValue("@CinemaId", cinemaId);
                    insertCommand.Parameters.AddWithValue("@HallId", movie.HallId);
                    insertCommand.Parameters.AddWithValue("@Title", movie.Title);
                    insertCommand.Parameters.AddWithValue("@Description", movie.Description);
                    insertCommand.Parameters.AddWithValue("@Duration", movie.Duration);

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

     public async Task<List<Movie>> GetAllMoviesAsync()
     {
         var movies = new List<Movie>();

         using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT movie_id, cinema_id, hall_id, title, description, duration FROM MOVIE";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            movies.Add(new Movie
                            {
                                MovieId = reader.GetString(0),
                                CinemaId = reader.GetString(1), 
                                HallId = reader.GetString(2),    
                                Title = reader.GetString(3),
                                Description = reader.GetString(4),
                                Duration = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }

         return movies;
     }

     public async Task<Movie> GetMovieByIdAsync(string movieId)
     {
         using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT movie_id, cinema_id, hall_id, title, description, duration FROM MOVIE WHERE movie_id = @MovieId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Movie
                            {
                                MovieId = reader.GetString(0),
                                CinemaId = reader.GetString(1), 
                                HallId = reader.GetString(2),    
                                Title = reader.GetString(3),
                                Description = reader.GetString(4),
                                Duration = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

         return null; // Return null if the movie is not found
     }

     public async Task DeleteMovieAsync(string movieId)
     {
         using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM MOVIE WHERE movie_id = @MovieId";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);

                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Movie not found");
                    }
                }
            }
     }
  }
}
