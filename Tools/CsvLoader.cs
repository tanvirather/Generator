using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
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

    public static List<ColumnModel> LoadColumns(string filePath, string baseSchema = "", string baseTable = "")
    {
        var rows = new List<ColumnModel>();
        if (!string.IsNullOrWhiteSpace(baseSchema) || !string.IsNullOrWhiteSpace(baseTable))
        {
            var csvPathBaseColumn = Path.Combine("Input", baseSchema, $"{baseTable}.csv");
            rows.AddRange(LoadColumns(csvPathBaseColumn, "", ""));
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, Config);
        csv.Context.TypeConverterOptionsCache.GetOptions<bool>().BooleanFalseValues.Add("");
        rows.AddRange([.. csv.GetRecords<ColumnModel>()]);
        return rows;
    }
}
