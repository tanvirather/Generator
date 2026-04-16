using Zuhid.Generator.Models;

namespace Zuhid.Generator.Tools;

public class FileService : IFileService
{
    public void WriteAllText(string filePath, string content)
    {
        var fileInfo = new FileInfo(filePath);
        fileInfo.Directory?.Create();
        File.WriteAllText(filePath, content);
    }

    public void WriteAllText(FileModel fileModel)
    {
        if (!string.IsNullOrEmpty(fileModel.Content))
        {
            var fileInfo = new FileInfo(fileModel.FilePath);
            fileInfo.Directory?.Create();
            File.WriteAllText(fileModel.FilePath, fileModel.Content);
        }
    }

    public void CopyFile(string sourcePath, string destinationPath)
    {
        var destFileInfo = new FileInfo(destinationPath);
        destFileInfo.Directory?.Create();
        File.Copy(sourcePath, destinationPath, true);
    }

    public List<TableModel> LoadTables(string filePath) => CsvLoader.LoadTables(filePath);

    public List<ColumnModel> LoadColumns(string inputPath, string filePath, string baseSchema = "", string baseTable = "")
        => CsvLoader.LoadColumns(inputPath, filePath, baseSchema, baseTable);

    public List<T> LoadData<T>(string filePath) => CsvLoader.LoadData<T>(filePath);
}
