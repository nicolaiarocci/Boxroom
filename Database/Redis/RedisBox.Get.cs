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
        public override async Task<T> Get<T>(string itemId)
        {
            ValidateProperties();

            EnsureConnection();
            var db = Multiplexer.GetDatabase(GetDatabaseIndex());
            var result = await db.StringGetAsync(itemId);
            if (!result.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(result.ToString());
        }
        public override async Task<T> Get<T>(T item)
        {
            var key = typeof(T).GetProperty(MetaFields.Id).GetValue(item).ToString();
            return await Get<T>(key);
        }
    }
}
