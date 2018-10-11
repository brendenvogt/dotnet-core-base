using MongoDB.Driver;
using Core.Entities;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : MongoRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
