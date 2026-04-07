using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class PostgresGenerate(SettingModel settingModel) : IGenerate
{
    public void Generate()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(settingModel.InputPath, "Table.csv"));
        GenerateSchema(tableList);
        foreach (var table in tableList)
        {
            var csvPathColumn = Path.Combine(settingModel.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = CsvLoader.LoadColumns(csvPathColumn, table.BaseSchema, table.BaseTable);
            GenerateTable(table, columns);
            GenerateForeignKeys(table, columns);
        }
    }

    /// <summary>
    /// Generate a sql script to create schema in the Output f/Db.Postgres folder
    /// </summary>
    /// <param name="tableList"></param>
    private void GenerateSchema(List<TableModel> tableList)
    {
        var filePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "schema.sql");
        var schemas = tableList.Select(x => x.Schema).Distinct().ToList();
        var sql = string.Join("\n", schemas.Select(schema => $"create schema if not exists {schema.ToSnakeCase()};\n"));
        new FileInfo(filePath).WriteAllText(sql);
    }

    private void GenerateTable(TableModel table, List<ColumnModel> columns)
    {
        var filePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "tables", table.Schema.ToSnakeCase(), $"{table.Table.ToSnakeCase()}.sql");
        var columnDefinitions = columns.Select(column =>
        {
            var definition = $"    {column.Column.ToSnakeCase()} {column.Datatype}";
            if (column.Length > 0)
            {
                definition += $"({column.Length})";
            }
            else if (column.Precision > 0)
            {
                definition += $"({column.Precision})";
            }

            if (column.Required)
            {
                definition += " not null";
            }

            if (!string.IsNullOrWhiteSpace(column.Default))
            {
                definition += $" default {column.Default}";
            }

            if (column.Unique == 1)
            {
                definition += " unique";
            }
            return definition;
        });

        var sql = $"create table if not exists {table.Schema.ToSnakeCase()}.{table.Table.ToSnakeCase()} (\n{string.Join(",\n", columnDefinitions)}\n);\n";
        new FileInfo(filePath).WriteAllText(sql);
    }

    private void GenerateForeignKeys(TableModel table, List<ColumnModel> columns)
    {
        var filePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "tables", table.Schema.ToSnakeCase(), $"{table.Table.ToSnakeCase()}.fk.sql");
        var fkStatements = columns
            .Where(column => !string.IsNullOrWhiteSpace(column.FkSchema) && !string.IsNullOrWhiteSpace(column.FkTable) && !string.IsNullOrWhiteSpace(column.FkColumn))
            .Select(column =>
            {
                var fkName = $"fk_{table.Table.ToSnakeCase()}_{column.Column.ToSnakeCase()}";
                var statement = $"alter table {table.Schema.ToSnakeCase()}.{table.Table.ToSnakeCase()} " +
                                $"add constraint {fkName} " +
                                $"foreign key ({column.Column.ToSnakeCase()}) " +
                                $"references {column.FkSchema.ToSnakeCase()}.{column.FkTable.ToSnakeCase()}({column.FkColumn.ToSnakeCase()});";
                return statement;
            }).ToList();

        if (fkStatements.Count != 0)
        {
            new FileInfo(filePath).WriteAllText(string.Join("\n", fkStatements) + "\n");
        }
    }
}

