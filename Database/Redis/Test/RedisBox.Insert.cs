using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boxroom.Database;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;

namespace Tests
{
    public partial class Tests
    {
        [Test]
        public void InsertThrowsWhenConnectionStringIsNull()
        {
            box.ConnectionString = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Insert(challenge));
        }
        [Test]
        public async Task InsertItem()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);

            await box.Insert(challenge);

            db.Verify(db => db.StringSetAsync(key, value, null, When.Always, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task InsertItemWithExpiry()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);
            var expiry = new TimeSpan(1, 2, 3);

            await box.Insert(challenge, expiry);

            db.Verify(db => db.StringSetAsync(key, value, expiry, When.Always, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task InsertItems()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);

            var challenges = new List<Class> { challenge, challenge };

            await box.Insert(challenges);

            db.Verify(db => db.StringSetAsync(key, value, null, When.Always, It.IsAny<CommandFlags>()), Times.Exactly(2));
        }
        [Test]
        public async Task InsertItemsWithExpiry()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);
            var expiry = new TimeSpan(1, 2, 3);

            var challenges = new List<Class> { challenge, challenge };

            await box.Insert(challenges, expiry);

            db.Verify(db => db.StringSetAsync(key, value, expiry, When.Always, It.IsAny<CommandFlags>()), Times.Exactly(2));
        }
    }
}