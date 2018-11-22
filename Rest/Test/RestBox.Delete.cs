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
    public class RestBoxTestDelete : RestBoxTestBase
    {

        [Test]
        public void DeleteThrowsWhenItemIdIsNullOrEmpty()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await Box.Delete<Class>(itemId: null));
            Assert.AreEqual("itemId", nullException.ParamName);

            var argumentException = Assert.ThrowsAsync<ArgumentException>(async() => await Box.Delete<Class>(itemId: string.Empty));
            Assert.AreEqual("itemId", argumentException.ParamName);
        }

        [Test]
        public void DeletePropertiesAreValidated()
        {
            PropertiesAreValidated(async() => await Box.Delete<Class>("id"));
        }

        [Test]
        public async Task DeleteSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Box.Headers.Add("Test", "Value");

            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            await Box.Delete<Class>("id");

            Assert.AreEqual(HttpStatusCode.NoContent, Box.Response.StatusCode);
        }

        [Test]
        public async Task DeleteFailure()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Box.Headers.Add("Test", "Value");

            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            Box.BaseAddress = new Uri("https://failme.com");

            await Box.Delete<Class>("id");

            Assert.AreEqual(HttpStatusCode.NotFound, Box.Response.StatusCode);
        }
        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}/id")
                .WithHeaders("Test", "Value")
                .Respond(HttpStatusCode.NoContent);

            return mockHttp;
        }

    }
}