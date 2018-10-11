using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Auth
{
    public class TokenValidator : ISecurityTokenValidator
    {
        readonly JwtSecurityTokenHandler _tokenHandler;

        public bool CanValidateToken => true;
        public int MaximumTokenSizeInBytes { get; set; }

        public TokenValidator(JwtSecurityTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            ValidateUser(principal);
            return principal;
        }

        public void ValidateUser(ClaimsPrincipal principal)
        {
            if (Guid.TryParse(principal.Identity.Name, out Guid _)) return;
            throw new SecurityTokenValidationException("Invalid User");
        }

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }
    }
}
