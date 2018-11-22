using System;
using Boxroom.Rest;
using NUnit.Framework;

namespace Test
{
    public class RestFindOptionsTest
    {
        private RestFindOptions<Class> FindOptions;

        [SetUp]
        public void Setup()
        {
            FindOptions = new RestFindOptions<Class>();
        }

        [Test]
        public void Defaults()
        {
            Assert.IsNull(FindOptions.ETag);
        }
    }
}