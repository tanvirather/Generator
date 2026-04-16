using Moq;
using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Tests.Generate;

public class CsharpGenerateTests
{
    private readonly SettingModel _setting;
    private readonly Mock<IFileService> _mockFileService;
    private readonly CsharpGenerate _csharpGenerate;

    public CsharpGenerateTests()
    {
        _setting = new SettingModel("MyCompany", "MyProduct", "InputPath", "OutputPath");
        _mockFileService = new Mock<IFileService>();
        _csharpGenerate = new CsharpGenerate(_setting, _mockFileService.Object);
    }

    [Fact]
    public void GenerateContext_ShouldReturnCorrectFileModel()
    {
        // Arrange
        var table = new TableModel("Public", "Users", "BaseSchema", "BaseTable", "BaseClass");
        var tables = new List<TableModel> { table };
        var columns = new List<ColumnModel>
        {
            new("Id", "Text", "Id", "uuid", true, null, 0, 0, 0, "", "", "")
        };

        _mockFileService.Setup(x => x.LoadTables(It.IsAny<string>())).Returns(tables);

        // Act
        var result = _csharpGenerate.GenerateContext();

        // Assert
        var expectedPath = Path.Combine("OutputPath", "MyProduct", "MyProductContext.cs");
        Assert.Equal(expectedPath, result.FilePath);
        Assert.Contains("public partial class MyProductContext", result.Content);
        Assert.Contains("namespace MyCompany.MyProduct;", result.Content);
    }
}
