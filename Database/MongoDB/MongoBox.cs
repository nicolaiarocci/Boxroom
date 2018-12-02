using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Boxroom.Core;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Boxroom.Database
{
    public class MongoBox : DatabaseBox
    {
        public override async Task<List<T>> Find<T>(Expression<Func<T, bool>> filter, IFindOptions<T> options = null)
        {
            ValidateProperties();

            var filters = new List<FilterDefinition<T>>();

            filters.Add(filter == null ?
                new ExpressionFilterDefinition<T>(_ => true) :
                new ExpressionFilterDefinition<T>(filter));

            var ifModifedSinceExpression = CreateIfModifiedFilterExperssion(options);
            if (ifModifedSinceExpression != null)
            {
                filters.Add(ifModifedSinceExpression);
            }

            return (await Collection<T>().FindAsync(Builders<T>.Filter.And(filters))).ToList();
        }
        private Expression<Func<T, bool>> CreateIfModifiedFilterExperssion<T>(IFindOptions<T> options)
        {
            if (options == null || !options.IfModifiedSince.HasValue) return null;
            var entity = Expression.Parameter(typeof(T));
            var body = Expression.GreaterThan(
                Expression.Property(entity, MetaFields.LastUpdated),
                Expression.Constant(options.IfModifiedSince.GetValueOrDefault()));

            return Expression.Lambda<Func<T, bool>>(body, entity);
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