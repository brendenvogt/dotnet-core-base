namespace Core.Contracts
{
    public class SignupUserContract
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
