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
        public override async Task<T> Insert<T>(T item)
        {
            return await Insert(item, expiry: null);
        }
        public async Task<T> Insert<T>(T item, TimeSpan? expiry)
        {
            ValidateProperties();

            EnsureConnection();
            var db = Multiplexer.GetDatabase(GetDatabaseIndex());

            await Insert(item, db, expiry, when: When.Always);

            return item;
        }
        public override async Task<List<T>> Insert<T>(List<T> items)
        {
            return await Insert(items, expiry: null);
        }
        public async Task<List<T>> Insert<T>(List<T> items, TimeSpan? expiry)
        {
            ValidateProperties();

            EnsureConnection();
            var db = Multiplexer.GetDatabase(GetDatabaseIndex());
            foreach (var item in items)
            {
                await Insert(item, db, expiry, When.Always);
            }
            return items;
        }
        private async Task<bool> Insert<T>(T item, IDatabase db, TimeSpan? expiry = null, When when = When.Always)
        {
            var obj = JsonConvert.SerializeObject(item);
            var key = item.GetType().GetProperty(MetaFields.Id).GetValue(item).ToString();
            return await db.StringSetAsync(key, obj, expiry, when);
        }
    }
}
