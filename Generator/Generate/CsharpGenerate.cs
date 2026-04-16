using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class CsharpGenerate(SettingModel setting, IFileService fileService) : IGenerate
{
    public void Generate()
    {
        fileService.WriteAllText(GenerateContext());
        foreach (var (sourcePath, destinationPath) in GenerateContext_LoadCsvData())
        {
            fileService.CopyFile(sourcePath, destinationPath);
        }
    }

    internal FileModel GenerateContext()
    {
        // var company = setting.Company;
        // var product = setting.Product;
        // var schema = table.Schema;
        // var entityName = table.Table + "Entity";
        // var tableName = table.Table;

        var content = $"""
using Microsoft.EntityFrameworkCore;
{GenerateContext_Namespace()}
using Zuhid.Base;

namespace {setting.Company}.{setting.Product};

public partial class {setting.Product}Context(DbContextOptions<{setting.Product}Context> options) : DbContext(options)
{"{"}
{GenerateContext_DbSet()}

    protected override void OnModelCreating(ModelBuilder builder)
    {"{"}
        base.OnModelCreating(builder);
        builder.ToSnakeCase("{setting.Product.ToLower()}");
        var basePath = "../{setting.Product}/Dataload";{GenerateContext_LoadCsvData_Content()}
    {"}"}
{"}"}

""";
        var directory = Path.Combine(setting.OutputPath, setting.Product);
        var fileName = $"{setting.Product}Context.cs";
        var filePath = Path.Combine(directory, fileName);
        return new FileModel { FilePath = filePath, Content = content };
    }

    private string GenerateContext_Namespace()
    {
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        var schemas = tableList.Select(x => x.Schema).Distinct().ToList().OrderBy(x => x).ToList();
        return string.Join("\n", schemas.Select(schema => $"using {setting.Company}.{setting.Product}.Entities.{schema};"));
    }

    private string GenerateContext_DbSet()
    {
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv")).OrderBy(x => x.Schema).ThenBy(x => x.Table).ToList();
        return string.Join("\n", tableList.Select(table => $"    public virtual DbSet<{table.Table}Entity> {table.Table} {"{"} get; set; {"}"} = null!;"));
    }

    private string GenerateContext_LoadCsvData_Content()
    {
        var loadDataContent = "";
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList)
        {
            var csvPathData = Path.Combine(setting.InputPath, "Dataload", table.Schema, $"{table.Table}.csv");
            if (File.Exists(csvPathData))
            {
                loadDataContent += $"\n        builder.LoadCsvData<{table.Table}Entity>($\"{{basePath}}/List/{table.Table}.csv\");";
            }
        }
        return loadDataContent;
    }

    private List<(string SourcePath, string DestinationPath)> GenerateContext_LoadCsvData()
    {
        var filesToCopy = new List<(string SourcePath, string DestinationPath)>();
        var tableList = fileService.LoadTables(Path.Combine(setting.InputPath, "Table.csv"));
        foreach (var table in tableList)
        {
            var csvPathData = Path.Combine(setting.InputPath, "Dataload", table.Schema, $"{table.Table}.csv");
            if (File.Exists(csvPathData))
            {
                var outputPath = Path.Combine(setting.OutputPath, setting.Product, "Dataload", table.Schema, $"{table.Table}.csv");
                filesToCopy.Add((csvPathData, outputPath));
            }
        }
        return filesToCopy;
    }
}
