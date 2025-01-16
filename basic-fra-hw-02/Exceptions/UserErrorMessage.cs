namespace basic_fra_hw_02.Exceptions
{
    public class UserErrorMessage : Exception
    {
        public UserErrorMessage(string? message) : base(message) { }
    }
}
