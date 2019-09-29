#!/bin/bash

set -e

set -a
source $1
set +a

sqldir="$(dirname $0)/sql"

create_database_sql=$(<$sqldir//create_database.sql)
create_extensions_sql=$(<$sqldir//create_extensions.sql)
create_functions_sql=$(<$sqldir//create_functions.sql)

docker exec -it postgres psql -U $PGUSER -c "${create_database_sql//p_dbname/$DBNAME}"
docker exec -it postgres psql -U $PGUSER -c "$create_extensions_sql" -d $DBNAME
docker exec -it postgres psql -U $PGUSER -c "$create_functions_sql"  -d $DBNAME
