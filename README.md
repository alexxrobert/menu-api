# Menu API

A list of dishes available in a restaurant. Including items, categories and pictures.

## Status

[![CircleCI](https://circleci.com/gh/storefront-community/menu-api.svg?style=shield)](https://circleci.com/gh/storefront-community/menu-api)
[![codecov](https://codecov.io/gh/storefront-community/menu-api/branch/master/graph/badge.svg)](https://codecov.io/gh/storefront-community/menu-api)

## Debug locally

Before you start:

- Install [.NET Core SDK](https://dotnet.microsoft.com/)
- Install [PostgreSQL](https://www.postgresql.org/)
- Install [VS Code](https://code.visualstudio.com/) (or your preferred editor)

Create the database (Linux):

```bash
bash .sh/db/create.sh /path/to/file.conf
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
