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
    public class RestBoxTestGet : RestBoxTestBase
    {

        [Test]
        public void GetPropertiesAreValidated()
        {
            PropertiesAreValidated(async () => await Box.Get<Class>("123"));
        }

        [Test]
        public async Task GetByIdSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Box.Authentication = new BasicAuthentication { Username = "user", Password = "pw" };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            var item = await Box.Get<Class>("123");

            Assert.AreEqual(HttpStatusCode.OK, Box.Response.StatusCode);
            Assert.AreEqual("123", item.Id);
            Assert.AreEqual("Item1", item.Name);
        }

        [Test]
        public async Task GetByIdFailure()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Box.Authentication = new BasicAuthentication { Username = "user", Password = "pw" };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            Box.BaseAddress = new Uri("https://failme.com");

            var item = await Box.Get<Class>("123");

            Assert.AreEqual(HttpStatusCode.NotFound, Box.Response.StatusCode);
            Assert.IsNull(item);
        }
        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}/123")
                .WithHeaders("Test", "Value")
                .WithHeaders("Authorization", "Basic dXNlcjpwdw==")
                .Respond(HttpStatusCode.OK,
                    "application/json", @"{ 'Id' : '123', 'Name': 'Item1', }");

            return mockHttp;
        }

    }
}