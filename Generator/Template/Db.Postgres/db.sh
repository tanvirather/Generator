#!/bin/bash

################################################## parameters
DB_HOST="localhost"
DB_PORT="5432"
DB_USER="postgres"
DB_PASSWORD="P@ssw0rd"
DB_NAME="[product]"
OUTPUT_FILE="db.sql"

################################################## methods
combine_table_files() {
  for file in "$1"/*.sql; do
    [[ "$file" == *.fk.sql ]] && continue
    [[ "$file" == *.custom.sql ]] && continue
    cat $file >> $OUTPUT_FILE
  done
}

combine_fk_files() {
  for file in "$1"/*.fk.sql; do
    cat $file >> $OUTPUT_FILE
  done
}

combine_custom_files() {
  for file in "$1"/*.custom.sql; do
    cat $file >> $OUTPUT_FILE
  done
}


################################################## execute

clear
cat "schema.sql" > $OUTPUT_FILE
combine_table_files "tables/list"
combine_fk_files "tables/list"
combine_custom_files "tables/list"

export PGPASSWORD=$DB_PASSWORD
dropdb --host=$DB_HOST --port=$DB_PORT --username=$DB_USER --force --if-exists $DB_NAME
createdb --host=$DB_HOST --port=$DB_PORT --username=$DB_USER $DB_NAME
psql --host=$DB_HOST --port=$DB_PORT --username=$DB_USER -d $DB_NAME -c "create extension pgcrypto;"
psql --host=$DB_HOST --port=$DB_PORT --username=$DB_USER -d $DB_NAME -f $OUTPUT_FILE
unset PGPASSWORD
