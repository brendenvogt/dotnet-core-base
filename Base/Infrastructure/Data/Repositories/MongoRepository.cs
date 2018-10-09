using System;
using MongoDB.Driver;
using System.Threading.Tasks;

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

        public virtual async Task AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        string ToLowerFirstChar(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length == 0)
                return str;
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

    }
}