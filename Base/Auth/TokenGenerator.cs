using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auth
{
    public class TokenGenerator
    {
        public static string GenerateToken(Guid authenticatedUserId, string secretKey, DateTime? expiration = null, Claim[] additionalClaims = null, string payloadEncryptionKey = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticatedUserId.ToString())
            };

            if (additionalClaims != null && additionalClaims.Length > 0)
                claims.AddRange(additionalClaims);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var newExpiration = expiration ?? DateTime.Now.AddDays(7);
            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.CreateJwtSecurityToken("Api", "Api", new ClaimsIdentity(claims), DateTime.Now, newExpiration, DateTime.Now, signingCredentials, string.IsNullOrEmpty(payloadEncryptionKey) ? null : EncryptingCredentials(payloadEncryptionKey));
            var token = handler.WriteToken(jwtSecurityToken);

            return token;
        }

        static EncryptingCredentials EncryptingCredentials(string encryptionKey)
        {
            var payloadKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encryptionKey));
            return new EncryptingCredentials(payloadKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
        }

        public static TokenValidationParameters CreateTokenValidationParameters(string secretKey, string encryptionKey = null)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = "Api",

                ValidateAudience = true,
                ValidAudience = "Api",

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                TokenDecryptionKey = string.IsNullOrEmpty(encryptionKey) ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encryptionKey))
            };
        }

        public static ClaimsPrincipal GetIdFromToken(string token, string secretKey, string payloadEncryptionKey = null)
        {
            Contract.Ensures(Contract.Result<ClaimsPrincipal>() != null);
            try
            {
                var validator = new JwtSecurityTokenHandler();
                return validator.ValidateToken(token, CreateTokenValidationParameters(secretKey, payloadEncryptionKey), out _);
            }
            catch { return null; }
        }
    }
}
