using Moq;
using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Tests.Generate;

public class CsharpConfigGenerateTests
{
    private readonly SettingModel _setting;
    private readonly Mock<IFileService> _mockFileService;
    private readonly CsharpConfigGenerate _configGenerate;

    public CsharpConfigGenerateTests()
    {
        _setting = new SettingModel("MyCompany", "MyProduct", "InputPath", "OutputPath");
        _mockFileService = new Mock<IFileService>();
        _configGenerate = new CsharpConfigGenerate(_setting, _mockFileService.Object);
    }

    private TableModel CreateTestTable() => new("Config", "User", "Config", "Base", "BaseClass");
    private List<TableModel> CreateAllTables() =>
    [
        new TableModel("Config", "User", "Config", "Base", "BaseClass"),
        new TableModel("Config", "Role", "Config", "Base", "BaseClass"),
        new TableModel("List", "Status", "List", "Base", "BaseClass")
    ];

    private List<ColumnModel> CreateTestColumns() =>
    [
        new ColumnModel("Id", "Text", "Id", "uuid", true, null, 0, 0, 0, "", "", ""),
        new ColumnModel("Name", "Text", "Name", "varchar", true, null, 50, 0, 0, "", "", ""),
        new ColumnModel("Role", "Text", "RoleId", "int", false, null, 0, 0, 0, "Config", "Role", "Id"),
        new ColumnModel("Status", "Text", "StatusId", "int", false, null, 0, 0, 0, "List", "Status", "Id")
    ];

    [Fact]
    public void GenerateEntities_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();
        var allTables = CreateAllTables();

        var result = _configGenerate.GenerateEntities(table, columns, columns, allTables);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Entities", "Config", "UserEntity.cs");
        Assert.Equal(expectedPath, result.FilePath);
        var c = result.Content;
        // System.Console.WriteLine("[DEBUG_LOG] Result Content:\n" + c);
        Assert.Contains("using System.ComponentModel.DataAnnotations.Schema;", c);
        Assert.Contains("using MyCompany.MyProduct.Enums.List;", c);
        Assert.Contains("using MyCompany.MyProduct.Entities.List;", c);
        Assert.Contains("public class UserEntity : BaseEntity", c);
        Assert.Contains("public Guid Id { get; set; }", c);
        Assert.Contains("public required string Name { get; set; }", c);
        Assert.Contains("[ForeignKey(nameof(Role))]", c);
        Assert.Contains("public int RoleId { get; set; }", c);
        Assert.Contains("public virtual RoleEntity? Role { get; set; }", c);
        Assert.Contains("[ForeignKey(nameof(Status))]", c);
        Assert.Contains("public StatusEnum? StatusId { get; set; }", c);
        Assert.Contains("public virtual StatusEntity? Status { get; set; }", c);
    }

    [Fact]
    public void GenerateModels_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _configGenerate.GenerateModels(table, columns);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Models", "Config", "UserModel.cs");
        Assert.Equal(expectedPath, result.FilePath);
        Assert.Contains("public class UserModel : UserEntity", result.Content);
        Assert.Contains("public string? RoleText { get; set; }", result.Content);
    }

    [Fact]
    public void GenerateRepositories_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _configGenerate.GenerateRepositories(table, columns);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Repositories", "Config", "UserRepository.cs");
        Assert.Equal(expectedPath, result.FilePath);
        var c = result.Content;
        Assert.Contains("public class UserRepository(MyProductContext context) : BaseClassRepository<MyProductContext>(context)", c);
        Assert.Contains("protected override IQueryable<UserModel> Query => context.User.Select(user => new UserModel", c);
        Assert.Contains("Id = user.Id", c);
        Assert.Contains("RoleId = user.RoleId", c);
        Assert.Contains("RoleText = user.Role.Text", c);
    }

    [Fact]
    public void GenerateControllers_ShouldReturnCorrectFileModel()
    {
        var table = CreateTestTable();
        var columns = CreateTestColumns();

        var result = _configGenerate.GenerateControllers(table);

        var expectedPath = Path.Combine("OutputPath", "MyProduct", "Controllers", "Config", "UserController.cs");
        Assert.Equal(expectedPath, result.FilePath);
        var c = result.Content;
        Assert.Contains("public class UserController(UserRepository repository) : BaseClassController", c);
        Assert.Contains("<MyProductContext, UserRepository, UserEntity, UserModel>(repository)", c);
    }
}
