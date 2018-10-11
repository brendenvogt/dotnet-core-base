using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Infrastructure.Data.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        public IMongoCollection<T> Collection { get; }

        public MongoRepository(IMongoDatabase database)
        {
            var collectionName = ToLowerFirstChar(typeof(T).Name);
            Collection = database.GetCollection<T>(collectionName);
        }

        //create
        public virtual async Task AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        //read
        public virtual IEnumerable<T> All()
        {
            return Collection.AsQueryable();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            var where = Collection.AsQueryable().Where(expression);
            return await where.FirstOrDefaultAsync();
        }

        public virtual async Task<List<T>> ListAsync(Expression<Func<T, bool>> expression)
        {
            var where = Collection.AsQueryable().Where(expression);
            return await where.ToListAsync();
        }

        //update
        public virtual async Task UpdateAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await Collection.ReplaceOneAsync(expression, entity);
        }

        public virtual async Task UpsertAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await Collection.ReplaceOneAsync(expression, entity, new UpdateOptions { IsUpsert = true });
        }

        //delete
        public async Task DeleteAsync(Expression<Func<T, bool>> expression)
        {
            await Collection.DeleteOneAsync(expression);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            await Collection.DeleteManyAsync(expression);
        }

        //utility
        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await Collection.CountDocumentsAsync(expression);
        }

        //helper
        string ToLowerFirstChar(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length == 0)
                return str;
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

    }
}