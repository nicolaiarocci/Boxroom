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
        public void ReplaceThrowsWhenConnectionStringIsNull()
        {
            box.ConnectionString = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await box.Delete(challenge));
        }
        [Test]
        public async Task Replace()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);
            db
                .Setup(_ => _.StringSetAsync(key, value, null, When.Exists, It.IsAny<CommandFlags>()))
                .Returns(Task.FromResult(true));
            await box.Insert(challenge);

            var returnValue = await box.Replace(challenge);

            db.Verify(db => db.StringSetAsync(key, value, null, When.Exists, It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsNotNull(returnValue);
            // TODO: might want to use DeepEqual instead.
            Assert.AreEqual(challenge.SubscriptionId, returnValue.SubscriptionId);
        }
        [Test]
        public async Task ReplaceFailure()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);

            var returnValue = await box.Replace(challenge);

            db.Verify(db => db.StringSetAsync(key, value, null, When.Exists, It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsNull(returnValue);
        }
        [Test]
        public async Task ReplaceWithExpiry()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);
            var expiry = new TimeSpan(1, 2, 3);
            db
                .Setup(_ => _.StringSetAsync(key, value, expiry, When.Exists, It.IsAny<CommandFlags>()))
                .Returns(Task.FromResult(true));
            await box.Insert(challenge);

            var returnValue = await box.Replace(challenge, expiry);

            db.Verify(db => db.StringSetAsync(key, value, expiry, When.Exists, It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsNotNull(returnValue);
            // TODO: might want to use DeepEqual instead.
            Assert.AreEqual(challenge.SubscriptionId, returnValue.SubscriptionId);
        }
        [Test]
        public async Task ReplaceWithExpiryFailure()
        {
            var key = challenge.ApiKey.ToString();
            var value = JsonConvert.SerializeObject(challenge);
            var expiry = new TimeSpan(1, 2, 3);

            var returnValue = await box.Replace(challenge, expiry);

            db.Verify(db => db.StringSetAsync(key, value, expiry, When.Exists, It.IsAny<CommandFlags>()), Times.Once());
            Assert.IsNull(returnValue);
        }
    }
}