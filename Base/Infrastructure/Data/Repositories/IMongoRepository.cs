using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public interface IMongoRepository<T> where T : class
    {
        Task AddAsync(T entity);
    }
}
