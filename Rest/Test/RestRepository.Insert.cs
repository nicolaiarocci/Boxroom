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
    public class RestRepositoryTestInsert : RestRepositoryTestBase
    {

        [Test]
        public void InsertThrowsWhenItemIsNullOrEmpty()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async() => await Repository.Insert<Class>(items: null));
            Assert.AreEqual("items", nullException.ParamName);

            var argumentException = Assert.ThrowsAsync<ArgumentException>(async() => await Repository.Insert<Class>(items: new List<Class>()));
            Assert.AreEqual("items", argumentException.ParamName);
        }

        [Test]
        public void InsertRepositoryPropertiesAreValidated()
        {
            RepositoryPropertiesAreValidated(async() => await Repository.Insert<Class>(new List<Class> { { new Class { Name = "name" } } }));
        }

        [Test]
        public async Task InsertSuccess()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Repository.Headers.Add("Test", "Value");
            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            var items = new List<Class>
            { { new Class { Name = "Item1" } },
                { new Class { Name = "Item2" } },
            };
            var returnedItems = await Repository.Insert<Class>(items);

            Assert.AreEqual(HttpStatusCode.Created, Repository.Response.StatusCode);
            Assert.AreEqual(2, returnedItems.Count);
            Assert.AreEqual("99", returnedItems[0].Id);
            Assert.AreEqual("100", returnedItems[1].Id);
        }

        [Test]
        public async Task InsertFailure()
        {
            Repository.BaseAddress = new Uri("https://testme.com");
            Repository.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Repository.Headers.Add("Test", "Value");

            Repository.HttpClient = new HttpClient(GetMock<Class>(Repository));

            var items = new List<Class>
            { { new Class { Name = "Item1" } },
                { new Class { Name = "Item2" } },
            };

            Repository.BaseAddress = new Uri("https://failme.com");

            var returnedItems = await Repository.Insert<Class>(items);

            Assert.AreEqual(HttpStatusCode.NotFound, Repository.Response.StatusCode);
            Assert.IsNull(returnedItems);
        }
        protected override MockHttpMessageHandler GetMock<T>(Repository client)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}")
                .WithHeaders("Test", "Value")
                .Respond(HttpStatusCode.Created,
                    "application/json", @"[
                        { 'Id' : '99', 'Name': 'Item1', },
                        { 'Id': 100, 'Name': 'Item2' }]");

            return mockHttp;
        }

    }
}