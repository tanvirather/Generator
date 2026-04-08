using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpListGenerate(SettingModel setting) : IGenerate
{
    public void Generate()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList.Where(t => t.BaseSchema == "List"))
        {
            var csvPathColumn = Path.Combine(setting.InputPath, table.Schema, $"{table.Table}.csv");
            var columns = CsvLoader.LoadColumns(setting.InputPath, csvPathColumn);
            GenerateEntities(table, columns);
            GenerateModels(table, columns);
            GenerateRepositories(table, columns);
            GenerateContext(table, columns);
            GenerateControllers(table, columns);
            GenerateEnums(table, columns);
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

public class {repositoryName}({product}Context context) : {table.BaseTable}Repository<{product}Context>(context)
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
using Zuhid.Base;

namespace {company}.{product};

public partial class {product}Context(DbContextOptions<{product}Context> options) : DbContext(options)
{"{"}
    public virtual DbSet<{entityName}> {tableName} {"{"} get; set; {"}"} = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {"{"}
        base.OnModelCreating(builder);
        builder.ToSnakeCase("{setting.Product.ToLower()}");
        var basePath = "../{setting.Product}/Dataload";{GenerateContext_LoadCsvData()}
    {"}"}
{"}"}

""";
        var directory = Path.Combine(setting.OutputPath, setting.Product);
        var fileName = $"{setting.Product}Context.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private string GenerateContext_LoadCsvData()
    {
        var loadDataContent = "";
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList)
        {
            var csvPathData = Path.Combine(setting.InputPath, "Dataload", table.Schema, $"{table.Table}.csv");
            if (File.Exists(csvPathData))
            {
                loadDataContent += $"\n        builder.LoadCsvData<{table.Table}Entity>($\"{{basePath}}/List/{table.Table}.csv\");";
                var outputPath = Path.Combine(setting.OutputPath, setting.Product, "Dataload", table.Schema, $"{table.Table}.csv");
                new FileInfo(csvPathData).CopyToWithDirectory(outputPath);
            }
        }
        return loadDataContent;
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
{"}"}

""";

        var directory = Path.Combine(setting.OutputPath, $"{setting.Product}", "Controllers", schema);
        var fileName = $"{controllerName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        fileInfo.WriteAllText(content);
    }

    private void GenerateEnums(TableModel table, List<ColumnModel> columns)
    {
        var csvPathData = Path.Combine(setting.InputPath, "Dataload", table.Schema, $"{table.Table}.csv");
        if (!File.Exists(csvPathData))
        {
            return;
        }

        var dataList = CsvLoader.LoadData<dynamic>(csvPathData);
        var enumName = table.Table + "Enum";
        var company = setting.Company;
        var product = setting.Product;
        var schema = table.Schema;

        var content = $"""
using System.ComponentModel.DataAnnotations;

namespace {company}.{product}.Enums.{schema};

public enum {enumName}
{"{"}
""";

        foreach (var data in dataList)
        {
            var dict = (IDictionary<string, object>)data;
            content += $"\n    [Display(Name = \"{dict["Text"].ToString()}\")]";
            content += $" {dict["Text"].ToString()?.Replace(" ", "")} = {dict["Id"].ToString()},\n";
        }

        content += "}\n";

        var directory = Path.Combine(setting.OutputPath, setting.Product, "Enums", schema);
        var fileName = $"{enumName}.cs";
        var fileInfo = new FileInfo(Path.Combine(directory, fileName));
        Console.WriteLine(fileInfo.FullName);
        fileInfo.WriteAllText(content);
    }
}
