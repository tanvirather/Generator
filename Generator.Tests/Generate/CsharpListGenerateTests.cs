using Moq;
using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Tests.Generate;

public class CsharpListGenerateTests
{
    private readonly SettingModel _setting;
    private readonly Mock<IFileService> _mockFileService;
    private readonly CsharpListGenerate _listGenerate;

    public CsharpListGenerateTests()
    {
        _setting = new SettingModel("MyCompany", "MyProduct", "InputPath", "OutputPath");
        _mockFileService = new Mock<IFileService>();
        _listGenerate = new CsharpListGenerate(_setting, _mockFileService.Object);
    }

    private TableModel CreateTestTable() => new("List", "Status", "List", "Base", "BaseClass");
    private List<TableModel> CreateAllTables() =>
    [
        new TableModel("List", "Status", "List", "Base", "BaseClass"),
        new TableModel("List", "Category", "List", "Base", "BaseClass"),
        new TableModel("Config", "Settings", "Config", "Base", "BaseClass")
    ];

    private List<ColumnModel> CreateTestColumns() =>
    [
        new ColumnModel("Id", "Text", "Id", "uuid", true, null, 0, 0, 0, "", "", ""),
        new ColumnModel("Text", "Text", "Text", "varchar", true, null, 100, 0, 0, "", "", ""),
        new ColumnModel("Category", "Text", "CategoryId", "int", false, null, 0, 0, 0, "List", "Category", "Id"),
        new ColumnModel("Setting", "Text", "SettingId", "int", false, null, 0, 0, 0, "Config", "Settings", "Id")
    ];

    [Fact]
    public void GenerateEntities_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();
        var allTables = CreateAllTables();

        var result = _listGenerate.GenerateEntities(table, columns, allTables);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Entities", "List", "StatusEntity.cs");
        Assert.Equal(expectedPath, result.FilePath);
        var c = result.Content;
        Assert.Contains("using Zuhid.Base;", c);
        Assert.Contains("using MyCompany.MyProduct.Enums.List;", c);
        Assert.Contains("public class StatusEntity : BaseEntity<StatusEnum>", c);
        Assert.Contains("[ForeignKey(\"Category\")]", c);
        Assert.Contains("public CategoryEnum CategoryId { get; set; }", c);
        Assert.Contains("public virtual CategoryEntity Category { get; set; } = null!;", c);
        Assert.Contains("[ForeignKey(\"Settings\")]", c);
        Assert.Contains("public unum SettingId { get; set; }", c);
        Assert.Contains("public virtual SettingsEntity Settings { get; set; } = null!;", c);
    }

    [Fact]
    public void GenerateModels_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _listGenerate.GenerateModels(table, columns);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Models", "List", "StatusModel.cs");
        Assert.Equal(expectedPath, result.FilePath);
        Assert.Contains("public class StatusModel : BaseModel<StatusEnum>", result.Content);
    }

    [Fact]
    public void GenerateRepositories_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _listGenerate.GenerateRepositories(table);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Repositories", "List", "StatusRepository.cs");
        Assert.Equal(expectedPath, result.FilePath);
        Assert.Contains("public class StatusRepository(MyProductContext context) : BaseRepository<MyProductContext>(context)", result.Content);
    }

    [Fact]
    public void GenerateControllers_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _listGenerate.GenerateControllers(table);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Controllers", "List", "StatusController.cs");
        Assert.Equal(expectedPath, result.FilePath);
        Assert.Contains("public class StatusController(StatusRepository repository) : BaseController<MyProductContext, StatusEntity, StatusEnum>(repository)", result.Content);
    }
}
