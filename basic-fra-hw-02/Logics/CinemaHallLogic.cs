using basic_fra_hw_02.Models;
using System.Text.RegularExpressions;

namespace basic_fra_hw_02.Logics
{
    public class CinemaHallLogic
    {
        public void ValidateHall(CinemaHall cinemaHall)
        {
            var namePattern = @"^[a-zA-Z0-9\s]+$";
            var capPattern = @"^\d+$"; //regex only numbers

            if (string.IsNullOrEmpty(cinemaHall.Name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            if (!Regex.IsMatch(cinemaHall.Name, namePattern))
            {
                throw new ArgumentException("Name can only contain letters, spaces and numbers.");
            }

            if (!Regex.IsMatch(cinemaHall.Capacity.ToString(), capPattern))
            {
                throw new ArgumentException("Capacity can only contain numbers.");
            }
        }
    }
}
