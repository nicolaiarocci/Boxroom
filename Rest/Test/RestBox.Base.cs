using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Boxroom.Rest;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Test
{
    public abstract class RestBoxTestBase
    {
        protected Boxroom Box;

        [SetUp]
        public void Setup()
        {
            Box = new Boxroom();
        }

        protected void PropertiesAreValidated(Func<Task> operation)
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async () => await operation());
            Assert.AreEqual(nameof(Box.BaseAddress), nullException.ParamName);

            Box.BaseAddress = new Uri("https://test.com");

            nullException = Assert.ThrowsAsync<ArgumentNullException>(async () => await operation());
            Assert.AreEqual(nameof(Box.DataSources), nullException.ParamName);

            Box.DataSources = new Dictionary<Type, string>();
            var argumentException = Assert.ThrowsAsync<ArgumentException>(async () => await operation());
            Assert.AreEqual(nameof(Box.DataSources), argumentException.ParamName);
        }

        protected abstract MockHttpMessageHandler GetMock<T>(Boxroom client);
    }
}