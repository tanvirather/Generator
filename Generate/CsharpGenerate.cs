using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpGenerate(SettingModel setting) : IGenerate
{
    public void Generate()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList)
        {
            var csvPathColumn = Path.Combine(setting.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = CsvLoader.LoadColumns(csvPathColumn);
            GenerateEntities(table, columns);
            GenerateModels(table, columns);
            GenerateRepositories(table, columns);
            GenerateContext(table, columns);
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

public class {entityName} : {table.BaseTable}Entity
{"{"}
""";

        foreach (var col in columns)
        {
            var csharpType = PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            content += $"\n  public {csharpType} {col.Column} {"{"} get; set; {"}"}";
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
using Zuhid.Base;
namespace {company}.{product}.Models.{schema};

public class {modelName} : {table.BaseTable}Model
{"{"}
""";

        foreach (var col in columns)
        {
            var csharpType = PostgresTypeConverter.ToCsharpType(col.Datatype, col.Required);
            content += $"\n  public {csharpType} {col.Column} {"{"} get; set; {"}"}";
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

public class {repositoryName}({product}Context context) : BaseRepository<{entityName}>(context)
{"{"}
{"}"}
""";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Repositories", schema);
        var fileName = $"{repositoryName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private void GenerateContext(TableModel table, List<ColumnModel> columns)
    {
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;
        var entityName = table.Table + "Entity";
        var tableName = table.Table;

        var content = $"""
using Microsoft.EntityFrameworkCore;
using {company}.{product}.Entities.{schema};

namespace {company}.{product};

public partial class {product}Context
{"{"}
    public virtual DbSet<{entityName}> {tableName} {"{"} get; set; {"}"} = null!;
{"}"}
""";

        var directory = Path.Combine(setting.OutputPath, setting.Product);
        var fileName = $"{tableName}Context.cs";
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

public class {controllerName}({table.Table}Repository repository) : {table.BaseTable}Controller<{entityName}, {modelName}>(repository)
{"{"}
{"}"}
""";

        var directory = Path.Combine(setting.OutputPath, $"{setting.Product}", "Controllers", schema);
        var fileName = $"{controllerName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }
}
