# Boxroom: CRUD for Humans

The Boxroom experiment is a project that aims to allow for asynchronous CRUD
operations on POCO objects to be easily executable against **any** kind of
backend storage system, no matter the underlying technology, thanks to a
uniform, yet simple interface. Currently, REST services and databases (either
SQL or NoSQL) are primary targets.

```cs
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
        return new MongoDatabaseBox()
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
        return new WebApiRestBox()
        {
            BaseAddress = new Uri("https://myservice.com"),
            // In the context of rest boxes, DataSources maps types to endpoints.
            DataSources = new Dictionary<Type, string> {
                { typeof (Customer), "/api/customers" },
                { typeof (Invoice), "/api/invoices" }
            }
        };
    }
```

Available projects:

- `Boxroom.Core`: core classes and interfaces
- `Boxroom.Database`: Database base classes and inerfaces
- `Boxroom.Database.MongoDB`: Mongo client
- `Boxroom.Rest`: REST base classes and interfaces
- `Boxroom.Rest.WebApi`: WebApi REST client

At this time all projects reside together but are likely to be split into
separate repositories when they are mature enough. They all conform to
NetStandard 2.0 and are under development. They are probably not ready for
production use.

Boxes (drivers, or clients) provide concrete interface implementations:

- `MongoDatabaseBox` is a Mongo client;
- `WebApiRestBox` is a WebApi client;

Both are in active development.

## Documentation

Documentation is planned.

## License

Boxroom is a [Nicola Iarocci](https://nicolaiarocci.com) open source
project distributed under the
[BSD](https://raw.githubusercontent.com/nicolaiarocci/Boxroom/master/LICENSE) license.