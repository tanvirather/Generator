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
            var columns = CsvLoader.LoadColumns(setting.InputPath, csvPathColumn);
            GenerateContext(table, columns);
        }
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
{GenerateContext_Namespace()}
using Zuhid.Base;

namespace {company}.{product};

public partial class {product}Context(DbContextOptions<{product}Context> options) : DbContext(options)
{"{"}
{GenerateContext_DbSet()}

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

    private string GenerateContext_Namespace()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        var schemas = tableList.Select(x => x.Schema).Distinct().ToList().OrderBy(x => x).ToList();
        return string.Join("\n", schemas.Select(schema => $"using {setting.Company}.{setting.Product}.Entities.{schema};"));
    }

    private string GenerateContext_DbSet()
    {
        var tableList = CsvLoader.LoadTables(Path.Combine(setting.InputPath, "Table.csv")).OrderBy(x => x.Schema).ThenBy(x => x.Table).ToList();
        return string.Join("\n", tableList.Select(table => $"    public virtual DbSet<{table.Table}Entity> {table.Table} {"{"} get; set; {"}"} = null!;"));
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
}
