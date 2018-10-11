using System.Threading.Tasks;
using Core.Contracts;
using Core.Entities;
using System;

namespace Infrastructure.Interfaces
{
    public interface IUserService
    {
        UserEntity AddUser(SignupUserContract user, out AuthInfo authInfo);
        Task<AuthInfo> LoginUserEmailAsync(LoginUserContract user);
        Task<UserEntity> GetUserAsync(Guid userId);
    }
}
