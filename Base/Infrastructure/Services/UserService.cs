using System;
using Infrastructure.Interfaces;
using System.Threading.Tasks;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Core.Entities;
using Auth;
using Core.Contracts;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        readonly IMongoRepository<UserEntity> _userRepository;
        readonly IConfiguration _configuration;
        readonly ILogger _logger;
        readonly IMapper _mapper;

        readonly double ExpirationDays = 7;

        public UserService(IConfiguration configuration, ILogger logger, IMapper mapper, IMongoRepository<UserEntity> userRepository )
        {
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserEntity> GetUserAsync(Guid userId){
            return await _userRepository.GetAsync(a => a.UserId == userId);
        }

        async Task<bool> EmailExistsAsync(string email)
        {
            var foundCount = await _userRepository.CountAsync(a => a.Email == email);
            return foundCount > 0;
        }

        public UserEntity AddUser(SignupUserContract user, out AuthInfo authInfo)
        {
            if (!string.IsNullOrEmpty(user.Email.Trim()) && EmailExistsAsync(user.Email).Result)
            {
                authInfo = null;
                return null;
            }

            var newId = Guid.NewGuid();
            var expiration = DateTime.UtcNow.AddDays(ExpirationDays);
            var bearerToken = TokenGenerator.GenerateToken(newId, _configuration["Security:SecretKey"], expiration, null, _configuration["Security:EncryptionKey"]);
            var passwordHash = CredentialUtility.HashPassword(user.Password);

            var addUser = new UserEntity
            {
                UserId = newId,
                Email = user.Email,
                EmailCandidate = user.Email,
                EmailConfirmed = false,
                PasswordHash = passwordHash,
                BearerToken = bearerToken,
                DateJoined = DateTimeOffset.UtcNow
            };

            authInfo = new AuthInfo
            {
                Token = bearerToken,
                Expiration = expiration
            };

            _userRepository.AddAsync(addUser).Wait();
            return addUser;
        }


        public async Task<AuthInfo> LoginUserEmailAsync(LoginUserContract user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Email.Trim())) return null;
            var foundUser = await _userRepository.GetAsync(a => a.Email == user.Email.Trim());
            if (foundUser == null) return null;

            if (CredentialUtility.IsValidPassword(foundUser, user.Password))
            {
                var expiration = DateTime.UtcNow.AddDays(ExpirationDays);
                var bearerToken = TokenGenerator.GenerateToken(foundUser.UserId, _configuration["Security:SecretKey"], expiration);
                foundUser.BearerToken = bearerToken;
                await _userRepository.UpdateAsync(a => a.UserId == foundUser.UserId, foundUser);
                return new AuthInfo
                {
                    Token = bearerToken,
                    Expiration = expiration
                };
            }
            return null;
        }
    }
}
