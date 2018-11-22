using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Boxroom.Rest;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Test
{
    public class RestBoxTestFind : RestBoxTestBase
    {

        [Test]
        public void ReplacePropertiesAreValidated()
        {
            PropertiesAreValidated(async() => await Box.Find<Class>(x => x.Name == "a name"));
        }

        [Test]
        public async Task FindSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            var results = await Box.Find<Class>(x => x.Name == "a name", new RestFindOptions<Class> { IfModifiedSince = DateTime.Now.Date });

            Assert.AreEqual(HttpStatusCode.OK, Box.Response.StatusCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("id", results[0].Id);
        }

        [Test]
        public async Task FindFailure()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            Box.BaseAddress = new Uri("https://failme.com");

            var results = await Box.Find<Class>(x => x.Name == "a name", new RestFindOptions<Class> { IfModifiedSince = DateTime.Now.Date });

            Assert.AreEqual(HttpStatusCode.NotFound, Box.Response.StatusCode);
            Assert.IsNull(results);
        }
        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}?test=me")
                .WithHeaders("Test", "Value")
                .WithHeaders("If-Modified-Since", DateTime.Now.Date.ToString("r"))
                .Respond(HttpStatusCode.OK,
                    "application/json", @"[{ 'Id' : 'id', 'Name': 'Item1', }]");

            return mockHttp;
        }

    }
}