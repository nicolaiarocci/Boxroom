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
        public void GetThrowsWhenConnectionStringIsNull()
        {
            box.ConnectionString = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Get(challenge));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Get<Class>("itemId"));
        }
        [Test]
        public async Task GetByItemId()
        {
            var key = challenge.ApiKey.ToString();

            await box.Get<Class>(key);

            db.Verify(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task GetByItemIdReturnsNullOnMissingKey()
        {
            var result = await box.Get<Class>("noway");
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetByItemInstance()
        {
            var key = challenge.ApiKey.ToString();

            await box.Get(challenge);

            db.Verify(db => db.StringGetAsync(key, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task GetByIinstanceReturnsNullOnMissingKey()
        {
            var result = await box.Get<Class>(new Class() { ApiKey = new Guid("12345678-1234-1234-1234-123456789012") });
            Assert.IsNull(result);
        }
    }
}