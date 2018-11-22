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
            // Perform a lookup against a database box.
            IDatabaseBox db = new MongoDatabaseBox()
            {
                ConnectionString = "mongodb://localhost:27017/my_database",
                DataSources = new Dictionary<Type, string>
                { { typeof(Customer), "customers" },
                { typeof(Invoice), "invoices" },
                }
            };
            var customers = Search<Customer>(db, c => c.Name == "John");

            // Same lookup, against a rest service box.
            IRestBox restService = new WebApiRestBox()
            {
                BaseAddress = new Uri("https://myservice.com"),
                DataSources = new Dictionary<Type, string>
                { { typeof(Customer), "/api/customers" },
                { typeof(Invoice), "/api/invoices" },
                }
            };
            // Same search but, just for the sake of it, this time only return 
            // objects that have been modified since 10 days ago.
            customers = Search<Customer>(restService, c => c.Name == "John", new FindOptions<Customer> { IfModifiedSince = DateTime.Now.AddDays(-10) });
        }
        static async Task<List<T>> Search<T>(IBox box, Expression<Func<T, bool>> filter, FindOptions<T> options = null)
        {
            return await box.Find<T>(filter, options);
        }
    }
}