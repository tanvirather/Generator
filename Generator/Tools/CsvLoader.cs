using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Zuhid.Generator.Models;

namespace Zuhid.Generator.Tools;

public static class CsvLoader
{
    private static readonly CsvConfiguration Config = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
    };

    public static List<TableModel> LoadTables(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, Config);
        return [.. csv.GetRecords<TableModel>()];
    }

    public static List<ColumnModel> LoadColumns(string inputPath, string filePath, string baseSchema = "", string baseTable = "")
    {
        try
        {
            var rows = new List<ColumnModel>();
            if (!string.IsNullOrWhiteSpace(baseSchema) || !string.IsNullOrWhiteSpace(baseTable))
            {
                var csvPathBaseColumn = Path.Combine(inputPath, baseSchema, $"{baseTable}.csv");
                rows.AddRange(LoadColumns(inputPath, csvPathBaseColumn, "", ""));
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, Config);
            csv.Context.TypeConverterOptionsCache.GetOptions<bool>().BooleanFalseValues.Add("");
            rows.AddRange([.. csv.GetRecords<ColumnModel>()]);
            return rows;
        }
        catch
        {
            Console.WriteLine($"ERROR: loading columns from {filePath}");
            throw;
        }

    }

    public static List<T> LoadData<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, Config);
        return [.. csv.GetRecords<T>()];
    }
}
