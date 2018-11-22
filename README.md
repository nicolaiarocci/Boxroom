# Boxroom Storage System

## CRUD for Humans

The Boxroom experiment aims to allow for asynchronous CRUD operations on POCO
objects to be executable against **any** kind of backend storage system, no
matter the underlying technology, with a uniform yet simple interface.

REST services and NoSQL/SQL databases are the current main targets.

All projects conform to NetStandard 2.0 and are currently under heavy
development. They are probably not ready for production use.

Boxes (drivers) provide concrete interface implementations, such as:

- MongoDB: `MongoDatabaseBox`
- WebApi: `WebApiRestBox`

Both of the above boxes are currently in development.

## Documentation

Documentation is still missing.

## License

Boxroom is a [Nicola Iarocci](https://nicolaiarocci.com) open source
project, distributed under the
[MIT](https://raw.githubusercontent.com/nicolaiarocci/Boxroom/master/LICENSE)`license.