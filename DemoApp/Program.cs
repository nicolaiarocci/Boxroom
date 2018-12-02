using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Boxroom.Core;
using Boxroom.Database;
using Boxroom.Rest;

namespace DemoApp
{

    class Program
    {

        static void Main(string[] args)
        {
            DoWork().Wait();
        }
        static async Task DoWork()
        {
            // IDatabaseBox and IRestBox both inherit from IBox.
            IDatabaseBox database = MongoBox();
            IRestBox remote = WebApiBox();

            var customer = new Customer { Name = "John Doe" };
            var invoice = new Invoice { Number = "ABC/123" };

            // Insert<T> (and other CRUD methods) are all defined by IBox.
            await remote.Insert(customer);
            await database.Insert(invoice);

            // Lookups also work alike, no matter where and how data is stored.
            var brooklynCustomers = await database.Find<Customer>(c => c.Zip == "11201");
            var invoices = await remote.Find<Invoice>(inv => inv.Date >= DateTime.Now.AddDays(-10), new FindOptions<Invoice> { IfModifiedSince = DateTime.Now.Date });

            Console.WriteLine($"We got back {brooklynCustomers.Count} customers and {invoices.Count} invoices");
        }
        static IDatabaseBox MongoBox()
        {
            return new MongoBox()
            {
                ConnectionString = "mongodb://localhost:27017/my_database",
                // In the context of database boxes, DataSources maps types to tables/collections.
                DataSources = new Dictionary<Type, string> {
                { typeof (Customer), "customers" },
                { typeof (Invoice), "invoices" }
            }
            };
        }
        static IRestBox WebApiBox()
        {
            return new WebApiBox()
            {
                BaseAddress = new Uri("https://myservice.com"),
                // In the context of rest boxes, DataSources maps types to endpoints.
                DataSources = new Dictionary<Type, string> {
                { typeof (Customer), "/api/customers" },
                { typeof (Invoice), "/api/invoices" }
            }
            };
        }
    }
}