# Menu API

A list of dishes available in a restaurant. Including items, categories and pictures.

## Status

[![CircleCI](https://circleci.com/gh/storefront-community/menu-api.svg?style=shield)](https://circleci.com/gh/storefront-community/menu-api)
[![codecov](https://codecov.io/gh/storefront-community/menu-api/branch/master/graph/badge.svg)](https://codecov.io/gh/storefront-community/menu-api)

## Documentation

API documentation written with Swagger and available at the root URL (no route prefix).

## Debug locally

Before you start:

- Install [.NET Core SDK](https://dotnet.microsoft.com/)
- Install [PostgreSQL](https://www.postgresql.org/) or
  [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads/)
- Install [VS Code](https://code.visualstudio.com/) (or your preferred editor)

To create/drop a PotgreSQL database on Linux:

```bash
# Create database:
bash .sh/db/postgresql/create.sh /path/to/file.conf

# Drop database:
bash .sh/db/postgresql/drop.sh /path/to/file.conf
```

```bash
#file.conf

PGHOST=""
PGPORT=""
PGUSER=""
PGPASSWORD=""
DBNAME=""
```

## Bugs and features

Please, fell free to [open a new issue](https://github.com/storefront-community/menu-api/issues) on GitHub.

## License

Code released under the [Apache License 2.0](https://github.com/storefront-community/menu-api/blob/master/LICENSE).  

Copyright (c) 2019-present, [Marx J. Moura](https://github.com/marxjmoura)
