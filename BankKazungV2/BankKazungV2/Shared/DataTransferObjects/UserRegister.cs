namespace BankKazungV2.Shared.DataTransferObjects
{
    public class UserRegister
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Password { get; set; } = string.Empty;

    }
}
