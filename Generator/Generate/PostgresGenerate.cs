using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class PostgresGenerate(SettingModel settingModel, IFileService fileService) : IGenerate
{
    public void Generate()
    {
        var tableList = fileService.LoadTables(Path.Combine(settingModel.InputPath, "Table.csv"));
        fileService.WriteAllText(GenerateSchema(tableList));
        foreach (var table in tableList)
        {
            var csvPathColumn = Path.Combine(settingModel.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = fileService.LoadColumns(settingModel.InputPath, csvPathColumn, table.BaseSchema, table.BaseTable);

            fileService.WriteAllText(GenerateTable(table, columns));
            fileService.WriteAllText(GenerateForeignKeys(table, columns));
        }
    }

    internal FileModel GenerateSchema(List<TableModel> tableList)
    {
        var schemas = tableList
            .Select(x => x.Schema)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => $"create schema if not exists {x.ToSnakeCase()};")
            .ToList();
        return new FileModel {
            FilePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "schema.sql"),
            Content = string.Join("\n", schemas) + "\n"
        };
    }

    internal FileModel GenerateTable(TableModel table, List<ColumnModel> columns)
    {
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

        var content = $"create table if not exists {table.Schema.ToSnakeCase()}.{table.Table.ToSnakeCase()} (\n{string.Join(",\n", columnDefinitions)}\n);\n";
        return new FileModel
        {
            FilePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "tables", table.Schema.ToSnakeCase(), $"{table.Table.ToSnakeCase()}.sql"),
            Content = content
        };
    }

    internal FileModel GenerateForeignKeys(TableModel table, List<ColumnModel> columns)
    {
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
        var content = fkStatements.Count != 0 ? string.Join("\n", fkStatements) + "\n" : string.Empty;
        return new FileModel
        {
            FilePath = Path.Combine(settingModel.OutputPath, "Db.Postgres", "tables", table.Schema.ToSnakeCase(), $"{table.Table.ToSnakeCase()}.fk.sql"),
            Content = content
        };
    }
}

