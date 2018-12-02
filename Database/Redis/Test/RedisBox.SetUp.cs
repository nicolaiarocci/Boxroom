using System;
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
        private Class challenge;
        private RedisBox box;
        private Mock<IConnectionMultiplexer> redis;
        private Mock<IDatabase> db;

        [SetUp]
        public void Setup()
        {

            challenge = new Class
            {
                ApiKey = new Guid(),
                VatId = "IT01180680397",
                SubscriptionId = "sub_1234",
                SubscriptionIsActive = true
            };
            var challengeJson = JsonConvert.SerializeObject(challenge);

            db = new Mock<IDatabase>();

            redis = new Mock<IConnectionMultiplexer>();
            redis
                .Setup(_ => _.IsConnected).Returns(true);
            redis
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(db.Object);

            box = new RedisBox(redis.Object);
            box.MetaFields.Id = "ApiKey";
        }
        [Test]
        public void Defaults()
        {
            Assert.AreEqual(box.ConnectionString, "localhost");
            Assert.IsNull(box.DataBaseName);
            Assert.IsNull(box.DataSources);
        }
        [Test]
        public void ValidateProperties()
        {
            box.ConnectionString = null;
            Assert.Throws<ArgumentNullException>(() => box.ValidateProperties());
        }

    }
}