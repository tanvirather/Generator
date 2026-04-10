using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpConfigGenerate(SettingModel setting) : IGenerate
{
    public void Generate()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        // string[] excludeList = ["List", "Config"];
        foreach (var table in tableList.Where(t => t.BaseSchema == "Config"))
        {
            var csvPathColumn = Path.Combine(setting.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = CsvLoader.LoadColumns(setting.InputPath, csvPathColumn);
            GenerateEntities(table, columns);
            GenerateModels(table, columns);
            GenerateRepositories(table, columns);
            GenerateControllers(table, columns);
        }
    }

    private void GenerateEntities(TableModel table, List<ColumnModel> columns)
    {
        var entityName = table.Table + "Entity";
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;

        var content = $"""
using Zuhid.Base;

namespace {company}.{product}.Entities.{schema};

public class {entityName} : BaseEntity
{"{"}
""";

        foreach (var col in columns)
        {
            var csharpType = PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            content += $"\n    public {csharpType} {col.Column} {"{"} get; set; {"}"}";
        }

        content += "\n}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Entities", schema);
        var fileName = $"{entityName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private void GenerateModels(TableModel table, List<ColumnModel> columns)
    {
        var modelName = table.Table + "Model";
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;

        var content = $"""
using {company}.{product}.Entities.{schema};
namespace {company}.{product}.Models.{schema};

public class {modelName} : {table.Table}Entity
{"{"}
""";

        foreach (var col in columns.Where(c => !string.IsNullOrWhiteSpace(c.FkSchema)))
        {
            content += $"\n    public string? {col.Column.Replace("Id", "Text")} {"{"} get; set; {"}"}";
        }

        content += "\n}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Models", schema);
        var fileName = $"{modelName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private void GenerateRepositories(TableModel table, List<ColumnModel> columns)
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
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private void GenerateControllers(TableModel table, List<ColumnModel> columns)
    {
        var controllerName = table.Table + "Controller";
        var modelName = table.Table + "Model";
        var entityName = table.Table + "Entity";
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;

        var content = $"""
using Zuhid.Base;
using {company}.{product}.Models.{schema};
using {company}.{product}.Entities.{schema};
using {company}.{product}.Repositories.{schema};

namespace {company}.{product}.Controllers.{schema};

public class {controllerName}({table.Table}Repository repository) : {table.BaseTable}Controller<{product}Context, {entityName}>(repository)
{"{"}
    [NonAction]
    public override Task Delete(Guid id) => throw new NotImplementedException("");
{"}"}

""";

        var directory = Path.Combine(setting.OutputPath, $"{setting.Product}", "Controllers", schema);
        var fileName = $"{controllerName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }
}
