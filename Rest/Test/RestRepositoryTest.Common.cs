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
    public class RestRepositoryCommonTest : RestRepositoryTestBase
    {

        [Test]
        public void Defaults()
        {
            Assert.IsNull(Repository.BaseAddress);
            Assert.IsNull(Repository.DataSources);
            Assert.IsNull(Repository.HttpClient);
            Assert.IsNull(Repository.Response);
            Assert.IsNotNull(Repository.MetaFields);
            Assert.AreEqual(0, Repository.Headers.Count);
        }

        protected override MockHttpMessageHandler GetMock<T>(Repository client)
        {
            throw new NotImplementedException();
        }

    }
}