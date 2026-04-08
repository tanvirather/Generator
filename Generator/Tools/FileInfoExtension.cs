namespace Zuhid.Generator.Tools;

public static class FileInfoExtension
{
    public static void WriteAllText(this FileInfo fileInfo, string content)
    {
        fileInfo.Directory?.Create();
        File.WriteAllText(fileInfo.FullName, content);
    }

    public static void CopyToWithDirectory(this FileInfo fileInfo, string destinationPath)
    {
        var destFileInfo = new FileInfo(destinationPath);
        destFileInfo.Directory?.Create();
        fileInfo.CopyTo(destinationPath, true);
    }
}
