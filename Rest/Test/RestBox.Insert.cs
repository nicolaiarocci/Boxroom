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
    public class RestBoxTestInsert : RestBoxTestBase
    {

        [Test]
        public void InsertListThrowsWhenItemIsNullOrEmpty()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async () => await Box.Insert<Class>(items: null));
            Assert.AreEqual("items", nullException.ParamName);

            var argumentException = Assert.ThrowsAsync<ArgumentException>(async () => await Box.Insert<Class>(items: new List<Class>()));
            Assert.AreEqual("items", argumentException.ParamName);
        }
        [Test]
        public void InsertItemThrowsWhenItemIsNull()
        {
            var nullException = Assert.ThrowsAsync<ArgumentNullException>(async () => await Box.Insert<Class>(item: null));
            Assert.AreEqual("item", nullException.ParamName);
        }
        [Test]
        public void InsertListPropertiesAreValidated()
        {
            PropertiesAreValidated(async () => await Box.Insert<Class>(new List<Class> { { new Class { Name = "name" } } }));
        }
        [Test]
        public void InsertItemPropertiesAreValidated()
        {
            PropertiesAreValidated(async () => await Box.Insert(new Class { Name = "name" }));
        }
        [Test]
        public async Task InsertListSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box, returnAsList: true));

            var items = new List<Class>
            { { new Class { Name = "Item1" } },
                { new Class { Name = "Item2" } },
            };
            var returnedItems = await Box.Insert<Class>(items);

            Assert.AreEqual(HttpStatusCode.Created, Box.Response.StatusCode);
            Assert.AreEqual(2, returnedItems.Count);
            Assert.AreEqual("99", returnedItems[0].Id);
            Assert.AreEqual("100", returnedItems[1].Id);
        }

        [Test]
        public async Task InsertListFailure()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Box.Headers.Add("Test", "Value");

            Box.HttpClient = new HttpClient(GetMock<Class>(Box, returnAsList: true));

            var items = new List<Class>
            { { new Class { Name = "Item1" } },
                { new Class { Name = "Item2" } },
            };

            Box.BaseAddress = new Uri("https://failme.com");

            var returnedItems = await Box.Insert<Class>(items);

            Assert.AreEqual(HttpStatusCode.NotFound, Box.Response.StatusCode);
            Assert.IsNull(returnedItems);
        }
        [Test]
        public async Task InsertItemSuccess()
        {
            Box.BaseAddress = new Uri("https://testme.com");
            Box.DataSources = new Dictionary<Type, string>
            { { typeof(Class), "endpoint" }
            };
            Box.Headers.Add("Test", "Value");
            Box.HttpClient = new HttpClient(GetMock<Class>(Box, returnAsList: false));

            var returnedItem = await Box.Insert<Class>(new Class { Name = "a name" });

            Assert.AreEqual(HttpStatusCode.Created, Box.Response.StatusCode);
            Assert.AreEqual("99", returnedItem.Id);
        }
        private MockHttpMessageHandler GetMock<T>(Boxroom client, bool returnAsList)
        {
            string returnValue;
            if (returnAsList == true)
            {
                returnValue = @"[{ 'Id' : '99', 'Name': 'Item1', }, { 'Id': 100, 'Name': 'Item2' }]";
            }
            else
            {
                returnValue = @"{ 'Id' : '99', 'Name': 'Item1', }";
            }

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{client.BaseAddress.ToString()}{client.DataSources[typeof(T)]}")
                .WithHeaders("Test", "Value")
                .WithHeaders("Content-Type", "application/json")
                .Respond(HttpStatusCode.Created,
                    "application/json", returnValue);

            return mockHttp;
        }

        protected override MockHttpMessageHandler GetMock<T>(Boxroom client)
        {
            throw new NotImplementedException();
        }
    }
}