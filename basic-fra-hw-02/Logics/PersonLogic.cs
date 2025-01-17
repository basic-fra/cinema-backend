using basic_fra_hw_02.Configuration;
using basic_fra_hw_02.Exceptions;
using basic_fra_hw_02.Models;
using basic_fra_hw_02.Services;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class PersonLogic : IPersonLogic
    {
        private readonly IPersonService _personService;
        private readonly ValidationConfiguration _validationConfiguration;

        public PersonLogic(IPersonService personService, IOptions<ValidationConfiguration> configuration)
        {
            _personService = personService;
            _validationConfiguration = configuration.Value;
        }

        public async Task AddPersonAsync(Person person)
        {
            ValidateNameField(person.Name);
            ValidatePasswordField(person.Password);
            ValidateRoleField(person.Role);
            await _personService.AddPersonAsync(person);
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _personService.GetAllPersonsAsync();
        }

        public async Task<Person> GetPersonByIdAsync(string id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                throw new UserErrorMessage("Person not found.");
            }
            return await _personService.GetPersonByIdAsync(id);
        }

        public async Task DeletePersonAsync(string id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                throw new UserErrorMessage("Person not found.");
            }

            await _personService.DeletePersonAsync(id);
        }

        private void ValidateNameField(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new UserErrorMessage("Name cannot be empty.");
            }

            if (name.Length > _validationConfiguration.PersonMaxCharacters)
            {
                throw new UserErrorMessage($"Name field too long. Exceeded {_validationConfiguration.PersonMaxCharacters} characters");
            }

            if (!Regex.IsMatch(name, _validationConfiguration.PersonNameRegex))
            {
                throw new UserErrorMessage($"Name must contain only letters and spaces.");
            }
        }

        private void ValidatePasswordField(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new UserErrorMessage("Password cannot be empty.");
            }

            if (!Regex.IsMatch(password, _validationConfiguration.PersonPasswordRegex))
            {
                throw new UserErrorMessage("Password invalid. Must contain 8 chars without spaces.");
            }
        }

        private void ValidateRoleField(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new UserErrorMessage("Role cannot be empty.");
            }

            if (!Regex.IsMatch(role, _validationConfiguration.PersonRoleRegex))
            {
                throw new UserErrorMessage("Role invalid. Must be 'user' or 'admin'.");
            }
        }
    }
}
