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
        public void DeleteThrowsWhenConnectionStringIsNull()
        {
            box.ConnectionString = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Delete(challenge));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Delete<Class>("itemId"));
        }
        [Test]
        public async Task DeleteByItemId()
        {
            var key = challenge.ApiKey.ToString();

            await box.Delete<Class>(key);

            db.Verify(db => db.KeyDeleteAsync(key, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task DeleteByItemInstance()
        {
            var key = challenge.ApiKey.ToString();

            await box.Get(challenge);

            db.Verify(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()), Times.Once());
        }
    }
}