using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Boxroom.Database
{
    public partial class RedisBox : DatabaseBox, IRedisBox
    {
        public async Task<bool> Exists(string itemId)
        {
            ValidateProperties();
            EnsureConnection();

            var db = Multiplexer.GetDatabase(GetDatabaseIndex());
            return await db.KeyExistsAsync(itemId);
        }

        public async Task<bool> Exists<T>(T item)
        {
            var key = typeof(T).GetProperty(MetaFields.Id).GetValue(item).ToString();
            return await Exists(key);
        }
    }
}
