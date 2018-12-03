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
        public void ExistsThrowsWhenConnectionStringIsNull()
        {
            box.ConnectionString = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Exists(challenge));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Exists("itemId"));
        }
        [Test]
        public async Task ExisstsByItemId()
        {
            var key = challenge.ApiKey.ToString();
            db.Setup(db => db.KeyExistsAsync(key, It.IsAny<CommandFlags>())).ReturnsAsync(true);
            var returnValue = await box.Exists(key);

            db.Verify(db => db.KeyExistsAsync(key, It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsTrue(returnValue);
        }
        [Test]
        public async Task ExistsByItemIdReturnsNullOnMissingKey()
        {
            db.Setup(db => db.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(false);

            var result = await box.Exists("noway");

            db.Verify(db => db.KeyExistsAsync("noway", It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsFalse(result);
        }
        [Test]
        public async Task ExistsByItemInstance()
        {
            var key = challenge.ApiKey.ToString();

            await box.Exists(challenge);

            db.Verify(db => db.KeyExistsAsync(key, It.IsAny<CommandFlags>()), Times.Once());
        }
        [Test]
        public async Task ExistsByIinstanceReturnsNullOnMissingKey()
        {
            db.Setup(db => db.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(false);
            var key = new Guid("12345678-1234-1234-1234-123456789012");

            var result = await box.Exists(new Class() { ApiKey = key });

            db.Verify(db => db.KeyExistsAsync(key.ToString(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsFalse(result);
        }
    }
}