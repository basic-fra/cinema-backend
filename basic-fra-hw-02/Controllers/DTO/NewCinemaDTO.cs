using basic_fra_hw_02.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Controllers.DTO
{
    public class NewCinemaDTO
    {
        public string? Name { get; set; }
        public string? Location { get; set; }

        public Cinema ToModel()
        {
            return new Cinema
            {
                Name = Name,
                Location = Location
            };
        }
    }
}
