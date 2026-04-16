using Moq;
using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Tests.Generate;

public class TemplateGenerateTests
{
    private readonly string _tempTemplatePath = "Template";

    public TemplateGenerateTests()
    {
        if (Directory.Exists(_tempTemplatePath))
        {
            Directory.Delete(_tempTemplatePath, true);
        }
        Directory.CreateDirectory(_tempTemplatePath);
    }

    [Fact]
    public void Generate_ShouldProcessFilesWithReplacements()
    {
        // Arrange
        var settingModel = new SettingModel(
            Company: "MyCompany",
            Product: "MyProduct",
            InputPath: "Input",
            OutputPath: "MyOutput"
        );

        var mockFileService = new Mock<IFileService>();

        // Create a test file in the Template directory
        var subDir = Path.Combine(_tempTemplatePath, "[Product].Api");
        Directory.CreateDirectory(subDir);
        var testFilePath = Path.Combine(subDir, "[Product]Controller.[csproj]");
        var testContent = "namespace [Company].[Product]; public class [product] { } // [company]";
        File.WriteAllText(testFilePath, testContent);

        // Also test [slnx] and [company] in another file
        var slnxPath = Path.Combine(_tempTemplatePath, "[Product].[slnx]");
        var slnxContent = "Solution for [Company]";
        File.WriteAllText(slnxPath, slnxContent);

        var templateGenerate = new TemplateGenerate(settingModel, mockFileService.Object);

        // Act
        templateGenerate.Generate();

        // Assert
        // Expected filename 1:
        // Template/[Product].Api/[Product]Controller.[csproj]
        // -> MyOutput/MyProduct.Api/MyProductController.csproj
        var expectedContent1 = "namespace MyCompany.MyProduct; public class myproduct { } // MyCompany";
        mockFileService.Verify(x => x.WriteAllText(
            It.Is<FileModel>(fm =>
                fm.FilePath.Replace("\\", "/").EndsWith("MyOutput/MyProduct.Api/MyProductController.csproj") &&
                fm.Content == expectedContent1)), Times.Once);

        // Expected filename 2:
        // Template/[Product].[slnx]
        // -> MyOutput/MyProduct.slnx
        var expectedContent2 = "Solution for MyCompany";
        mockFileService.Verify(x => x.WriteAllText(
            It.Is<FileModel>(fm =>
                fm.FilePath.Replace("\\", "/").EndsWith("MyOutput/MyProduct.slnx") &&
                fm.Content == expectedContent2)), Times.Once);
    }
}
