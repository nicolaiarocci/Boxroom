# Boxroom: CRUD for Humans

The Boxroom experiment is a project that aims to allow for asynchronous CRUD
operations on POCO objects to be executable against **any** kind of backend
storage system, no matter the underlying technology, with a uniform yet
simple interface. Currently, REST services and SQL/NoSQL databases are
primary targets.

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