using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class CinemaLogic
    {
        public void ValidateCinema(Cinema cinema)
        {
            if (cinema == null)
            {
                throw new ArgumentNullException(nameof(cinema), "Cinema object cannot be null.");
            }

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
        }
        public void ValidateCinemaId(Cinema cinema)
        {
            if (cinema == null)
            {
                throw new ArgumentException("Cinema with the specified ID does not exist.");
            }
        }
    }
}
