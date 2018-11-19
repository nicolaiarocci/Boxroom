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
    public class RestRepositoryTestDelete : RestRepositoryTestBase
    {

        [Test]
        public void DeleteThrowsWhenItemIdIsNullOrEmpty()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await Repository.Delete<Class>(itemId: null));
            Assert.AreEqual("itemId", nullException.ParamName);

            var argumentException = Assert.ThrowsAsync<ArgumentException>(async() => await Repository.Delete<Class>(itemId: string.Empty));
            Assert.AreEqual("itemId", argumentException.ParamName);
        }

        [Test]
        public void DeleteRepositoryPropertiesAreValidated()
        {
            RepositoryPropertiesAreValidated(async() => await Repository.Delete<Class>("id"));
        }

        [Test]
        public async Task DeleteSuccess()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Repository.Headers.Add("Test", "Value");

            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            await Repository.Delete<Class>("id");

            Assert.AreEqual(HttpStatusCode.NoContent, Repository.Response.StatusCode);
        }

        [Test]
        public async Task DeleteFailure()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Repository.Headers.Add("Test", "Value");

            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            Repository.BaseAddress = new Uri("https://failme.com");

            await Repository.Delete<Class>("id");

            Assert.AreEqual(HttpStatusCode.NotFound, Repository.Response.StatusCode);
        }
        protected override MockHttpMessageHandler GetMock<T>(Repository client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}/id")
                .WithHeaders("Test", "Value")
                .Respond(HttpStatusCode.NoContent);

            return mockHttp;
        }

    }
}