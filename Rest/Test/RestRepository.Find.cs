using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataStorage.Rest;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Test
{
    public class RestRepositoryTestFind : RestRepositoryTestBase
    {

        [Test]
        public void ReplaceRepositoryPropertiesAreValidated()
        {
            RepositoryPropertiesAreValidated(async() => await Repository.Find<Class>(x => x.Name == "a name"));
        }

        [Test]
        public async Task FindSuccess()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Repository.Headers.Add("Test", "Value");
            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            var results = await Repository.Find<Class>(x => x.Name == "a name", new RestFindOptions<Class> { IfModifiedSince = DateTime.Now.Date });

            Assert.AreEqual(HttpStatusCode.OK, Repository.Response.StatusCode);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("id", results[0].Id);
        }

        [Test]
        public async Task FindFailure()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Repository.Headers.Add("Test", "Value");
            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            Repository.BaseAddress = new Uri("https://failme.com");

            var results = await Repository.Find<Class>(x => x.Name == "a name", new RestFindOptions<Class> { IfModifiedSince = DateTime.Now.Date });

            Assert.AreEqual(HttpStatusCode.NotFound, Repository.Response.StatusCode);
            Assert.IsNull(results);
        }
        protected override MockHttpMessageHandler GetMock<T>(Repository client)
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