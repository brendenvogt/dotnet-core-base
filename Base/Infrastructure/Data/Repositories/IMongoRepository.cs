using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories
{
    public interface IMongoRepository<T> where T : class
    {
        //create
        Task AddAsync(T entity);
        //read
        IEnumerable<T> All();
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> expression);
        //update
        Task UpdateAsync(Expression<Func<T, bool>> expression, T entity);
        Task UpsertAsync(Expression<Func<T, bool>> expression, T entity);
        //delete
        Task DeleteAsync(Expression<Func<T, bool>> expression);
        Task DeleteManyAsync(Expression<Func<T, bool>> expression);
        //utility
        Task<long> CountAsync(Expression<Func<T, bool>> expression);
    }
}
