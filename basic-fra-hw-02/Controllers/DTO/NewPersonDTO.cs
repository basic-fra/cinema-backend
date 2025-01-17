using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class NewPersonDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public Person ToModel()
        {
            return new Person
            {
                Name = Name,
                Password = Password,
                Role = Role
            };
        }
    }
}
