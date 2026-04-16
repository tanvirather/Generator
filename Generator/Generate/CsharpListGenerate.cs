using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpListGenerate(SettingModel setting, IFileService fileService) : IGenerate
{
    public void Generate()
    {
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList.Where(t => t.BaseSchema == "List"))
        {
            var csvPathColumn = Path.Combine(setting.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = fileService.LoadColumns(setting.InputPath, csvPathColumn);

            fileService.WriteAllText(GenerateEntities(table, columns, tableList));
            fileService.WriteAllText(GenerateModels(table, columns));
            fileService.WriteAllText(GenerateRepositories(table));
            fileService.WriteAllText(GenerateControllers(table));

            var enumFile = GenerateEnums(table);
            if (enumFile != null)
            {
                fileService.WriteAllText(enumFile);
            }
        }
    }

    internal FileModel GenerateEntities(TableModel table, List<ColumnModel> columns, List<TableModel> allTables)
    {
        var hasEnum = columns.Any(col =>
        {
            var isFk = !string.IsNullOrWhiteSpace(col.FkTable);
            var fkTable = allTables.FirstOrDefault(t => t.Table == col.FkTable && t.Schema == col.FkSchema);
            return isFk && fkTable?.BaseSchema == "List";
        });

        var navigationNamespaces = columns
            .Where(col => !string.IsNullOrWhiteSpace(col.FkTable) && !string.IsNullOrWhiteSpace(col.FkSchema) && col.FkSchema != table.Schema)
            .Select(col => col.FkSchema)
            .Distinct()
            .Select(fkSchema => $"using {setting.Company}.{setting.Product}.Entities.{fkSchema};")
            .ToList();

        var content = $"""
using Zuhid.Base;
using {setting.Company}.{setting.Product}.Enums.List;

namespace {setting.Company}.{setting.Product}.Entities.{table.Schema};

public class {table.Table}Entity : {table.BaseTable}Entity<{table.Table}Enum>
{"{"}
""";

        foreach (var col in columns)
        {
            var isFk = !string.IsNullOrWhiteSpace(col.FkTable);
            var fkTable = allTables.FirstOrDefault(t => t.Table == col.FkTable && t.Schema == col.FkSchema);
            var csharpType = isFk ? (fkTable?.BaseSchema == "List" ? $"{col.FkTable}Enum" : "unum") : PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            if (isFk)
            {
                content += $"\n  [ForeignKey(\"{col.FkTable}\")]";
            }
            content += $"\n  public {csharpType} {col.Column} {"{"} get; set; {"}"}";

            if (isFk)
            {
                content += $"\n  public virtual {col.FkTable}Entity {col.FkTable} {"{"} get; set; {"}"} = null!;";
            }
        }

        content += "\n}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Entities", table.Schema);
        var fileName = $"{table.Table}Entity.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }

    internal FileModel GenerateModels(TableModel table, List<ColumnModel> columns)
    {
            var content = $"""
using Zuhid.Base;
using {setting.Company}.{setting.Product}.Enums.{table.Schema};

namespace {setting.Company}.{setting.Product}.Models.{table.Schema};

public class {table.Table}Model : {table.BaseTable}Model<{table.Table}Enum>
{"{"}
""";

        foreach (var col in columns)
        {
            var csharpType = PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            content += $"\n  public {csharpType} {col.Column} {"{"} get; set; {"}"}";
        }
            content += "\n}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Models", table.Schema);
        var fileName = $"{table.Table}Model.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }

    internal FileModel GenerateRepositories(TableModel table)
    {
        var repositoryName = table.Table + "Repository";
        var entityName = table.Table + "Entity";
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;

        var content = $"""
using Zuhid.Base;
using {company}.{product}.Entities.{schema};

namespace {company}.{product}.Repositories.{schema};

public class {repositoryName}({product}Context context) : {table.BaseTable}Repository<{product}Context>(context)
{"{"}
{"}"}

""";
        var directory = Path.Combine(setting.OutputPath, setting.Product, "Repositories", schema);
        var fileName = $"{repositoryName}.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }

    internal FileModel GenerateControllers(TableModel table)
    {
        var content = $"""
using Zuhid.Base;
using {setting.Company}.{setting.Product}.Enums.{table.Schema};
using {setting.Company}.{setting.Product}.Models.{table.Schema};
using {setting.Company}.{setting.Product}.Entities.{table.Schema};
using {setting.Company}.{setting.Product}.Repositories.{table.Schema};

namespace {setting.Company}.{setting.Product}.Controllers.{table.Schema};

public class {table.Table}Controller({table.Table}Repository repository) : {table.BaseTable}Controller<{setting.Product}Context, {table.Table}Entity, {table.Table}Enum>(repository)
{"{"}
{"}"}

""";
        var directory = Path.Combine(setting.OutputPath, $"{setting.Product}", "Controllers", table.Schema);
        var fileName = $"{table.Table}Controller.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }

    internal FileModel? GenerateEnums(TableModel table)
    {
        var csvPathData = Path.Combine(setting.InputPath, "Dataload", table.Schema, $"{table.Table}.csv");
        if (!File.Exists(csvPathData))
        {
            return null;
        }

        var dataList = fileService.LoadData<dynamic>(csvPathData);
        var content = $"""
using System.ComponentModel.DataAnnotations;

namespace {setting.Company}.{setting.Product}.Enums.{table.Schema};

public enum {table.Table}Enum
{"{"}
""";

        foreach (var data in dataList)
        {
            var dict = (IDictionary<string, object>)data;
            content += $"\n    [Display(Name = \"{dict["Text"]}\")]";
            content += $" {dict["Text"].ToString()?.Replace(" ", "")} = {dict["Id"]},\n";
        }

        content += "}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Enums", table.Schema);
        var fileName = $"{table.Table}Enum.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }
}
