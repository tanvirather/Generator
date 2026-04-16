using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpConfigGenerate(SettingModel setting, IFileService fileService) : IGenerate
{
    public void Generate()
    {
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList.Where(t => t.BaseSchema == "Config"))
        {
            var csvPathColumn = Path.Combine(setting.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = fileService.LoadColumns(setting.InputPath, csvPathColumn);
            var columnsWithBase = fileService.LoadColumns(setting.InputPath, csvPathColumn, table.BaseSchema, table.BaseTable);

            fileService.WriteAllText(GenerateEntities(table, columns, columnsWithBase, tableList));
            fileService.WriteAllText(GenerateModels(table, columns));
            fileService.WriteAllText(GenerateRepositories(table, columnsWithBase));
            fileService.WriteAllText(GenerateControllers(table));
        }
    }

    internal FileModel GenerateEntities(TableModel table, List<ColumnModel> columns,  List<ColumnModel> columnsWithBase, List<TableModel> allTables)
    {
        var hasEnum = columns.Any(t => t.FkSchema == "List");
        var navigationNamespaces = columns
            .Where(col => !string.IsNullOrWhiteSpace(col.FkTable) && !string.IsNullOrWhiteSpace(col.FkSchema) && col.FkSchema != table.Schema)
            .Select(col => col.FkSchema)
            .Distinct()
            .Select(fkSchema => $"using {setting.Company}.{setting.Product}.Entities.{fkSchema};")
            .Append($"using Zuhid.Base;")
            .Append($"using System.ComponentModel.DataAnnotations.Schema;")
            .Order()
            .ToList();
        if(columns.Any(t => t.FkSchema == "List"))
        {
            navigationNamespaces.Insert(0, $"using {setting.Company}.{setting.Product}.Enums.List;");
        }

        var content = $"""
{string.Join("\n", navigationNamespaces)}

namespace {setting.Company}.{setting.Product}.Entities.{table.Schema};

public class {table.Table}Entity : BaseEntity
{"{"}
""";

        foreach (var col in columns)
        {
            var isFk = !string.IsNullOrWhiteSpace(col.FkTable);
            // var fkTable = allTables.FirstOrDefault(t => t.Table == col.FkTable && t.Schema == col.FkSchema);
            var csharpType = col.FkSchema == "List" ? $"{col.FkTable}Enum{(col.Required ? "" : "?")}" : PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            if (isFk)
            {
                content += $"\n    [ForeignKey(nameof({col.FkTable}))]";
            }
            content += $"\n    public {csharpType} {col.Column} {"{"} get; set; {"}"}";

            if (isFk)
            {
                content += $"\n    public virtual {col.FkTable}Entity{(col.Required ? "" : "?")} {col.FkTable} {"{"} get; set; {"}"}{(col.Required ? "" : " = null!;")}";
            }
        }
        content += "\n}\n";
        return new FileModel { FilePath = Path.Combine(setting.OutputPath, setting.Product, "Entities", table.Schema, $"{table.Table}Entity.cs"), Content = content };
    }

    internal FileModel GenerateModels(TableModel table, List<ColumnModel> columns)
    {
        var content = $"""
using {setting.Company}.{setting.Product}.Entities.{table.Schema};
namespace {setting.Company}.{setting.Product}.Models.{table.Schema};

public class {table.Table}Model : {table.Table}Entity
{"{"}
""";
        foreach (var col in columns.Where(c => !string.IsNullOrWhiteSpace(c.FkSchema)))
        {
            content += $"\n    public string? {col.Column.Replace("Id", "Text")} {"{"} get; set; {"}"}";
        }
        content += "\n}\n";
        return new FileModel { FilePath = Path.Combine(setting.OutputPath, setting.Product, "Models", table.Schema, $"{table.Table}Model.cs"), Content = content };
    }

    internal FileModel GenerateRepositories(TableModel table, List<ColumnModel> columns)
    {
        var variableName = table.Table.ToCamelCase();
        var mappings = columns.Select(col => $"        {col.Column} = {variableName}.{col.Column}").ToList();
        mappings.AddRange(columns.Where(c => !string.IsNullOrWhiteSpace(c.FkTable)).Select(col => $"        {col.Column.Replace("Id", "Text")} = {variableName}.{col.FkTable}.Text"));

        var content = $"""
using Zuhid.Base;
using {setting.Company}.{setting.Product}.Entities.{table.Schema};
using {setting.Company}.{setting.Product}.Models.{table.Schema};

namespace {setting.Company}.{setting.Product}.Repositories.{table.Schema};

public class {table.Table}Repository({setting.Product}Context context) : {table.BaseClass}Repository<{setting.Product}Context>(context)
{"{"}
    protected override IQueryable<{table.Table}Model> Query => context.{table.Table}.Select({variableName} => new {table.Table}Model
    {"{"}
{string.Join(",\n", mappings)}
    {"}"});
{"}"}

""";
        return new FileModel { FilePath = Path.Combine(setting.OutputPath, setting.Product, "Repositories", table.Schema, $"{table.Table}Repository.cs"), Content = content };
    }

    internal FileModel GenerateControllers(TableModel table)
    {
        var content = $"""
using Zuhid.Base;
using {setting.Company}.{setting.Product}.Models.{table.Schema};
using {setting.Company}.{setting.Product}.Entities.{table.Schema};
using {setting.Company}.{setting.Product}.Repositories.{table.Schema};
using Microsoft.AspNetCore.Mvc;

namespace {setting.Company}.{setting.Product}.Controllers.{table.Schema};

public class {table.Table}Controller({table.Table}Repository repository) : {table.BaseClass}Controller
    <{setting.Product}Context, {table.Table}Repository, {table.Table}Entity, {table.Table}Model>(repository)
{"{"}
    [NonAction]
    public override Task Delete(Guid id) => throw new NotImplementedException("");
{"}"}

""";
        return new FileModel 
        { 
            FilePath = Path.Combine(Path.Combine(setting.OutputPath, $"{setting.Product}", "Controllers", table.Schema), $"{table.Table}Controller.cs"),
            Content = content
        };
    }
}
