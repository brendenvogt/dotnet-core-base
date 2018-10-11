using System;
using Microsoft.AspNetCore.Authentication;

namespace Auth
{
    public class JwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        readonly string _secretKey;
        readonly string _encryptionKey;
        readonly TokenValidator _tokenValidator;

        public JwtDataFormat(TokenValidator tokenValidator, string secretKey, string encryptionKey)
        {
            _tokenValidator = tokenValidator;
            _encryptionKey = secretKey;
            _secretKey = secretKey;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return Unprotect(protectedText, null);
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var principal = _tokenValidator.ValidateToken(protectedText,
                                                          TokenGenerator.CreateTokenValidationParameters(_secretKey, _encryptionKey), out _);
            return new AuthenticationTicket(principal, new AuthenticationProperties(), "JWT");
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }
    }
}
