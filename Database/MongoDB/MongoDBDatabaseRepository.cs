using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataStorage.Database.MongoDB
{
    public class MongoDBDatabaseRepository : BaseDatabaseRepository
    {
        public override async Task<List<T>> Get<T>(Expression<Func<T, bool>> filter)
        {
            ValidateProperties();
            return (await Collection<T>().FindAsync(filter)).ToList();
        }
        public override async Task Insert<T>(List<T> items)
        {
            ValidateProperties();
            await Collection<T>().InsertManyAsync(items);
        }
        public override async Task<T> Delete<T>(Expression<Func<T, bool>> filter)
        {
            ValidateProperties();
            return await Collection<T>().FindOneAndDeleteAsync(filter);
        }
        private IMongoCollection<T> Collection<T>()
        {
            return GetDatabase().GetCollection<T>(DataSources[typeof(T)]);
        }
        private IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(DataBaseName);
        }
    }
}