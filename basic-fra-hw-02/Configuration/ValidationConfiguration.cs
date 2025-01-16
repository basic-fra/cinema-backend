namespace basic_fra_hw_02.Configuration
{
    public class ValidationConfiguration
    {
        public int CinemaMaxCharacters { get; set; }
        public int HallMaxCharacters { get; set; }
        public int MovieMaxCharacters { get; set; }
        public int PersonMaxCharacters { get; set; }
        public string CinemaLocationRegex { get; set; } 
        public string HallNameRegex { get; set; }
        public string PersonNameRegex { get; set; }
        public string PersonPasswordRegex { get; set; }
        public string PersonRoleRegex { get; set; }
        public string TicketSeatNumberRegex { get; set; }
    }
}
