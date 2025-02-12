﻿using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Logics;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

public class PersonService : IPersonService
{
    private readonly string _connectionString;

    public PersonService(IOptions<DBConfiguration> configuration)
    {
        _connectionString = configuration.Value.ConnectionString;
    }

    public async Task AddPersonAsync(Person person)
    {
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
                            PersonId = reader.GetString(0),
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

    public async Task<Person> GetPersonByIdAsync(string personId)
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
                            PersonId = reader.GetString(0),
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

    public async Task DeletePersonAsync(string personId)
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

}

