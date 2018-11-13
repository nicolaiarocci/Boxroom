using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataStorage.Core;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataStorage.Database.MongoDB
{
    public class MongoDBDatabaseRepository : DatabaseRepositoryBase
    {
        public override async Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, Core.FindOptions<T> options = null)
        {
            ValidateProperties();
            if (options != null)
            {
                // TODO: add support for IfModifedSince option
                throw new ArgumentException($"{nameof(options)} currently not supported");
            }

            return (await Collection<T>().FindAsync(filter)).ToList();
        }
        public override async Task<List<T>> Insert<T>(List<T> items)
        {
            ValidateProperties();
            await Collection<T>().InsertManyAsync(items);
            return items;
        }
        public override async Task<T> Replace<T>(T item)
        {
            ValidateProperties();

            // TODO: id might be mapped with a BsonId attribute, or might have
            // been mapped with a Mongo ClassMap.

            // TODO: what if both values are null?
            var (idMemberName, idMemberValue) = GetIdMemberNameAndValue<T>(item);

            var builder = Builders<T>.Filter;
            var filter = builder.Eq(idMemberName, idMemberValue);

            return await Collection<T>().FindOneAndReplaceAsync(filter, item);
        }
        public override async Task<T> Delete<T>(Expression<Func<T, bool>> filter)
        {
            ValidateProperties();
            return await Collection<T>().FindOneAndDeleteAsync(filter);
        }
        private IMongoCollection<T> Collection<T>()
        {
            // TODO make client a class-level field?
            // performance should be optimized anyway (same connection string)
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(DataBaseName).GetCollection<T>(DataSources[typeof(T)]);
        }
    }
}