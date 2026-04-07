namespace Zuhid.Generator.Tools;

public static class FileInfoExtension
{
    public static void WriteAllText(this FileInfo fileInfo, string content)
    {
        fileInfo.Directory?.Create();
        File.WriteAllText(fileInfo.FullName, content);
    }
}
