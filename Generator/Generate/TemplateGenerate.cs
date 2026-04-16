using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class TemplateGenerate(SettingModel settingModel, IFileService fileService) : IGenerate
{
    /// <summary>
    /// Copy each folder and file from
    /// </summary>
    public void Generate()
    {
        //Copy all the files & Replaces any files with the same name
        foreach (var templatePath in Directory.GetFiles(Path.Combine("Template"), "*.*", SearchOption.AllDirectories))
        {
            fileService.WriteAllText(GenerateFile(templatePath));
        }
    }

    internal FileModel GenerateFile(string templatePath)
    {
        var fileName = templatePath
            .Replace("Template", settingModel.OutputPath)
            .Replace("[Company]", settingModel.Company, StringComparison.InvariantCulture)
            .Replace("[company]", settingModel.Company, StringComparison.InvariantCulture)
            .Replace("[Product]", settingModel.Product, StringComparison.InvariantCulture)
            .Replace("[product]", settingModel.Product.ToLower(), StringComparison.InvariantCulture)
            .Replace("[slnx]", "slnx")
            .Replace("[csproj]", "csproj")
            ;
        var fileContent = File.ReadAllText(templatePath)
            .Replace("[Company]", settingModel.Company, StringComparison.InvariantCulture)
            .Replace("[company]", settingModel.Company, StringComparison.InvariantCulture)
            .Replace("[Product]", settingModel.Product, StringComparison.InvariantCulture)
            .Replace("[product]", settingModel.Product.ToLower(), StringComparison.InvariantCulture);
        return new FileModel { FilePath = fileName, Content = fileContent };
    }
}

