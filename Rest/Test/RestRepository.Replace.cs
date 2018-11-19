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
    public class RestRepositoryTestReplace : RestRepositoryTestBase
    {

        [Test]
        public void ReplaceThrowsWhenItemIsNull()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await Repository.Replace<Class>(item: null));
            Assert.AreEqual("item", nullException.ParamName);
        }

        [Test]
        public void ReplaceRepositoryPropertiesAreValidated()
        {
            RepositoryPropertiesAreValidated(async() => await Repository.Replace<Class>(new Class { Name = "name" }));
        }

        [Test]
        public async Task ReplaceSuccess()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Repository.Headers.Add("Test", "Value");
            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            var returnedItem = await Repository.Replace<Class>(new Class { Name = "Item1", Id = "id" });

            Assert.AreEqual(HttpStatusCode.OK, Repository.Response.StatusCode);
            Assert.AreEqual("id", returnedItem.Id);
        }

        [Test]
        public async Task ReplaceFailure()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string> { { typeof(Class), "endpoint" } };
            Repository.Headers.Add("Test", "Value");
            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            Repository.BaseAddress = new Uri("https://failme.com");

            var returnedItem = await Repository.Replace<Class>(new Class { Name = "Item1", Id = "id" });

            Assert.AreEqual(HttpStatusCode.NotFound, Repository.Response.StatusCode);
            Assert.IsNull(returnedItem);
        }
        protected override MockHttpMessageHandler GetMock<T>(Repository client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}/id")
                .WithHeaders("Test", "Value")
                .Respond(HttpStatusCode.OK,
                    "application/json", @"
                        { 'Id' : 'id', 'Name': 'Item1', }");

            return mockHttp;
        }

    }
}