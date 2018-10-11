namespace Auth
{
    public class AuthOptions
    {
        public string SecretKey { get; set; }
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string EncryptionKey { get; set; }
    }
}
