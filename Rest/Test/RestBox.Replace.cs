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
    public class RestBoxTestReplace : RestBoxTestBase
    {

        [Test]
        public void ReplaceThrowsWhenItemIsNull()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async () => await Box.Replace<Class>(item: null));
            Assert.AreEqual("item", nullException.ParamName);
        }

        [Test]
        public void ReplacePropertiesAreValidated()
        {
            PropertiesAreValidated(async () => await Box.Replace<Class>(new Class { Name = "name" }));
        }

        [Test]
        public async Task ReplaceSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint/" } };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            var returnedItem = await Box.Replace<Class>(new Class { Name = "Item1", Id = "id" });

            Assert.AreEqual(HttpStatusCode.OK, Box.Response.StatusCode);
            Assert.AreEqual("id", returnedItem.Id);
        }

        [Test]
        public async Task ReplaceFailure()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box));

            Box.BaseAddress = new Uri("https://failme.com");

            var returnedItem = await Box.Replace<Class>(new Class { Name = "Item1", Id = "id" });

            Assert.AreEqual(HttpStatusCode.NotFound, Box.Response.StatusCode);
            Assert.IsNull(returnedItem);
        }
        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"https://testme.com/endpoint/id")
                .WithHeaders("Test", "Value")
                .WithHeaders("Content-Type", "application/json")
                .Respond(HttpStatusCode.OK,
                    "application/json", @"
                        { 'Id' : 'id', 'Name': 'Item1', }");

            return mockHttp;
        }

    }
}