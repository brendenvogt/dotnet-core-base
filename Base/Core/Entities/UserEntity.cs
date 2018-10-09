using System;
namespace Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string EmailCandidate { get; set; }
        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }
        public string BearerToken { get; set; }
        public DateTimeOffset DateJoined { get; set; }
    }
}
