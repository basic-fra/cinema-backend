using basic_fra_hw_02.Models;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class PersonDTO
    {
        public string PersonId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static PersonDTO FromModel(Person model)
        {
            return new PersonDTO
            {
                PersonId = model.PersonId,
                Name = model.Name,
                Password = model.Password,
                Role = model.Role
            };
        }
    }
}
