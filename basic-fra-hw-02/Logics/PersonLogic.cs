using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class PersonLogic
    {
        public void ValidatePerson(Person person)
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
        }
    }
}
