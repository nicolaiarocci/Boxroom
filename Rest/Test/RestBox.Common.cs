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
    public class RestBoxCommonTest : RestBoxTestBase
    {

        [Test]
        public void Defaults()
        {
            Assert.IsNull(Box.BaseAddress);
            Assert.IsNull(Box.DataSources);
            Assert.IsNull(Box.HttpClient);
            Assert.IsNull(Box.Response);
            Assert.IsNull(Box.Authentication);
            Assert.IsNotNull(Box.MetaFields);
            Assert.AreEqual(0, Box.Headers.Count);
        }

        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            throw new NotImplementedException();
        }

    }
}