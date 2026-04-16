using Zuhid.Generator.Models;

namespace Zuhid.Generator.Tools;

public interface IFileService
{
    void WriteAllText(string filePath, string content);
    void WriteAllText(FileModel fileModel);
    void CopyFile(string sourcePath, string destinationPath);
    List<TableModel> LoadTables(string filePath);
    List<ColumnModel> LoadColumns(string inputPath, string filePath, string baseSchema = "", string baseTable = "");
    List<T> LoadData<T>(string filePath);
}
