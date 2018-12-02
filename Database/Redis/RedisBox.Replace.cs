using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Boxroom.Database
{
    public partial class RedisBox : DatabaseBox, IRedisBox
    {
        public async Task<T> Replace<T>(T item, TimeSpan? expiry)
        {
            ValidateProperties();

            EnsureConnection();
            var db = redis.GetDatabase(GetDatabaseIndex());

            var success = await Insert(item, db, expiry, When.Exists);

            // TODO: what is the expected behavior when a replace/write fails? Throw, return null, or what?
            // Also consider the case of restboxes etc.
            return (success) ? item : default(T);
        }

        public override async Task<T> Replace<T>(T item)
        {
            return await Replace(item, expiry: null);
        }
    }
}
