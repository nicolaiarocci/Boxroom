using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using DataStorage.Rest;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Test
{
    public abstract class RestRepositoryTestBase
    {
        protected Repository Repository;

        [SetUp]
        public void Setup()
        {
            Repository = new Repository();
        }

        protected void RepositoryPropertiesAreValidated(Func<Task> operation)
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await operation());
            Assert.AreEqual(nameof(Repository.BaseAddress), nullException.ParamName);

            Repository.BaseAddress = new Uri("https://test.com");

            nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await operation());
            Assert.AreEqual(nameof(Repository.DataSources), nullException.ParamName);

            Repository.DataSources = new Dictionary<Type, string>();
            var argumentException = Assert.ThrowsAsync<ArgumentException>(async() => await operation());
            Assert.AreEqual(nameof(Repository.DataSources), argumentException.ParamName);
        }

        protected abstract MockHttpMessageHandler GetMock<T>(Repository client);
    }
}