# Boxroom: CRUD for Humans

The Boxroom experiment is a project that aims to allow for asynchronous CRUD
operations on POCO objects to be easily executable against **any** kind of
backend storage system, no matter the underlying technology, thanks to a
uniform, yet simple interface. Currently, REST services and databases (either
SQL or NoSQL) are primary targets.

```cs
    // Perform a lookup against a database box.
    IDatabaseBox db = new MongoDatabaseBox()
    {
        ConnectionString = "mongodb://localhost:27017/my_database",
        DataSources = new Dictionary<Type, string> {
            { typeof(Customer), "customers" },
            { typeof(Invoice), "invoices" },
        };
    };
    var customers = await box.Find<Customer>(c => c.Name == "John");

    // Same lookup, against a rest service box.
    IRestBox restService = new WebApiRestBox()
    {
        BaseAddress = new Uri("https://myservice.com"),
        DataSources = new Dictionary<Type, string> {
            { typeof(Customer), "/api/customers" },
            { typeof(Invoice), "/api/invoices" },
        };
    };
    // Same search, but only return if objects have been modified since 10 days ago.
    customers = await restService.Find<Customer>(c => c.Name == "John", new FindOptions<Customer> {IfModifiedSince = DateTime.Now.AddDays(-10)});
```

Projects available:

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