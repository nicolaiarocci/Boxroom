using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Boxroom.Core;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Boxroom.Database
{
    public partial class RedisBox : DatabaseBox, IRedisBox
    {
        public override async Task Delete<T>(string itemId)
        {
            ValidateProperties();

            EnsureConnection();
            var db = redis.GetDatabase(GetDatabaseIndex());
            await db.KeyDeleteAsync(itemId);
        }
        public override async Task Delete<T>(T item)
        {
            ValidateProperties();

            EnsureConnection();
            var db = redis.GetDatabase(GetDatabaseIndex());
            var key = item.GetType().GetProperty(MetaFields.Id).GetValue(item).ToString();
            await db.KeyDeleteAsync(key);
        }
    }
}
