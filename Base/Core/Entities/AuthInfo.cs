using System;
namespace Core.Entities
{
    public class AuthInfo
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
