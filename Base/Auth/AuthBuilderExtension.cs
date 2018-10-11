using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Auth
{
    public static class AuthBuilderExtension
    {
        public static IServiceCollection UseTrdAuthentication(this IServiceCollection services, AuthOptions tokenOptions)
        {
            var jwtTokenValidator = new JwtSecurityTokenHandler();
            var validator = new TokenValidator(jwtTokenValidator);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenGenerator.CreateTokenValidationParameters(tokenOptions.SecretKey, tokenOptions.EncryptionKey);
                options.RequireHttpsMetadata = false;
            });

            return services;
        }
    }
}
