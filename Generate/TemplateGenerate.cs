using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Generate;

public class TemplateGenerate(SettingModel settingModel) : IGenerate
{
    /// <summary>
    /// Copy each folder and file from 
    /// </summary>
    public void Generate()
    {
        //Copy all the files & Replaces any files with the same name
        foreach (var templatePath in Directory.GetFiles(Path.Combine("Template"), "*.*", SearchOption.AllDirectories))
        {
            var fileName = templatePath
                .Replace("Template", settingModel.OutputPath)
                .Replace("[Company]", settingModel.Company, StringComparison.InvariantCulture)
                .Replace("[company]", settingModel.Company, StringComparison.InvariantCulture)
                .Replace("[Product]", settingModel.Product, StringComparison.InvariantCulture)
                .Replace("[product]", settingModel.Product, StringComparison.InvariantCulture)
                .Replace("[csproj]", "csproj");
            var fileContent = File.ReadAllText(templatePath)
                .Replace("[Company]", settingModel.Company, StringComparison.InvariantCulture)
                .Replace("[company]", settingModel.Company, StringComparison.InvariantCulture)
                .Replace("[Product]", settingModel.Product, StringComparison.InvariantCulture)
                .Replace("[product]", settingModel.Product, StringComparison.InvariantCulture);
            new FileInfo(fileName).WriteAllText(fileContent);
        }
    }
}

