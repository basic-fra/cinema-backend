using basic_fra_hw_02.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

public class PersonService
{
    private readonly string _connectionString;

    public PersonService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddPersonAsync(Person person)
    {
        var namePattern = @"^[a-zA-Z\s]+$"; // This regex allows only letters and spaces
        var passPattern = @"^\S{8}$"; // Must be 8 chars and no spaces allowed

        if (string.IsNullOrEmpty(person.Name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (!Regex.IsMatch(person.Name, namePattern))
        {
            throw new ArgumentException("Name can only contain letters and spaces.");
        }

        if (!Regex.IsMatch(person.Password, passPattern))
        {
            throw new ArgumentException("Password must contain 8 characters.");
        }

        person.PersonId = Guid.NewGuid(); // Generate GUID here

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                INSERT INTO PERSON (person_id, name, password, role)
                VALUES (@PersonId, @Name, @Password, @Role)";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonId", person.PersonId);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Password", person.Password);
                command.Parameters.AddWithValue("@Role", person.Role);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<Person>> GetAllPersonsAsync()
    {
        var persons = new List<Person>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT person_id, name, password, role FROM PERSON";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        persons.Add(new Person
                        {
                            PersonId = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        });
                    }
                }
            }
        }

        return persons;
    }

    public async Task<Person> GetPersonByIdAsync(Guid personId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT person_id, name, password, role FROM PERSON WHERE person_id = @PersonId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonId", personId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Person
                        {
                            PersonId = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                    }
                }
            }
        }
        return null; // Return null if person not found
    }

    public async Task UpdatePersonAsync(Person person)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var namePattern = @"^[a-zA-Z\s]+$"; // This regex allows only letters and spaces
            var passPattern = @"^\S{8}$"; // Must be 8 chars and no spaces allowed

            if (string.IsNullOrEmpty(person.Name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            if (!Regex.IsMatch(person.Name, namePattern))
            {
                throw new ArgumentException("Cinema name can only contain letters and spaces.");
            }

            if (!Regex.IsMatch(person.Password, passPattern))
            {
                throw new ArgumentException("Password must contain 8 characters.");
            }
            await connection.OpenAsync();

            var query = @"
            UPDATE PERSON
            SET name = @Name, password = @Password, role = @Role
            WHERE person_id = @PersonId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonId", person.PersonId);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Password", person.Password);
                command.Parameters.AddWithValue("@Role", person.Role);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Person not found");
                }
            }
        }
    }

    public async Task DeletePersonAsync(Guid personId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "DELETE FROM PERSON WHERE person_id = @PersonId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonId", personId);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new Exception("Person not found");
                }
            }
        }
    }

    public async Task<bool> CheckIfPersonExistsAsync(Guid personId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM PERSON WHERE person_id = @PersonId";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonId", personId);

                var result = (long)await command.ExecuteScalarAsync();
                return result > 0;
            }
        }
    }

}

