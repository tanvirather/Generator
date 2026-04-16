using Moq;
using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

namespace Zuhid.Generator.Tests.Generate;

public class PostgresGenerateTests
{
    private readonly PostgresGenerate _postgresGenerate;

    public PostgresGenerateTests()
    {
        _postgresGenerate = new PostgresGenerate(new SettingModel("MyCompany", "MyProduct", "InputPath", "OutputPath"), new Mock<IFileService>().Object);
    }

    [Fact]
    public void GenerateTable_ShouldReturnCorrectFileModel()
    {
        // Arrange
        var table = new TableModel("Public", "Users", "BaseSchema", "BaseTable", "BaseClass");
        var columns = new List<ColumnModel>
        {
            new("Id", "Text", "Id", "uuid", true, "gen_random_uuid()", 0, 0, 0, "", "", ""),
            new("Username", "Text", "Username", "varchar", true, null, 50, 0, 1, "", "", ""),
            new("Age", "Number", "Age", "int", false, "0", 0, 0, 0, "", "", ""),
            new("Price", "Number", "Price", "decimal", true, null, 0, 10, 0, "", "", "")
        };

        var expectedFilePath = Path.Combine("OutputPath", "Db.Postgres", "tables", "public", "users.sql");
        var expectedContent = "create table if not exists public.users (\n" +
                              "    id uuid not null default gen_random_uuid(),\n" +
                              "    username varchar(50) not null unique,\n" +
                              "    age int default 0,\n" +
                              "    price decimal(10) not null\n" +
                              ");\n";

        // Act
        var result = _postgresGenerate.GenerateTable(table, columns);

        // Assert
        Assert.Equal(expectedFilePath, result.FilePath);
        Assert.Equal(expectedContent, result.Content);
    }

    [Fact]
    public void GenerateForeignKeys_ShouldReturnCorrectFileModel()
    {
        // Arrange
        var table = new TableModel("Public", "Orders", "BaseSchema", "BaseTable", "BaseClass");
        var columns = new List<ColumnModel>
        {
            new("Id", "Text", "Id", "uuid", true, null, 0, 0, 0, "", "", ""),
            new("UserId", "Text", "UserId", "uuid", true, null, 0, 0, 0, "Public", "Users", "Id"),
            new("ProductId", "Text", "ProductId", "uuid", true, null, 0, 0, 0, "Inventory", "Products", "Id")
        };

        var expectedFilePath = Path.Combine("OutputPath", "Db.Postgres", "tables", "public", "orders.fk.sql");
        var expectedContent = "alter table public.orders add constraint fk_orders_user_id foreign key (user_id) references public.users(id);\n" +
                              "alter table public.orders add constraint fk_orders_product_id foreign key (product_id) references inventory.products(id);\n";

        // Act
        var result = _postgresGenerate.GenerateForeignKeys(table, columns);

        // Assert
        Assert.Equal(expectedFilePath, result.FilePath);
        Assert.Equal(expectedContent, result.Content);
    }

    [Fact]
    public void GenerateForeignKeys_ShouldReturnEmptyContentWhenNoFk()
    {
        // Arrange
        var table = new TableModel("Public", "Users", "BaseSchema", "BaseTable", "BaseClass");
        var columns = new List<ColumnModel>
        {
            new("Id", "Text", "Id", "uuid", true, null, 0, 0, 0, "", "", "")
        };

        // Act
        var result = _postgresGenerate.GenerateForeignKeys(table, columns);

        // Assert
        Assert.Empty(result.Content);
    }

    [Fact]
    public void GenerateSchema_ShouldReturnCorrectFileModel()
    {
        // Arrange
        var tables = new List<TableModel>
        {
            new("Public", "Users", "", "", ""),
            new("Inventory", "Products", "", "", ""),
            new("Public", "Orders", "", "", "")
        };

        // Act
        var result = _postgresGenerate.GenerateSchema(tables);

        // Assert
        var expectedFilePath = Path.Combine("OutputPath", "Db.Postgres", "schema.sql");
        var expectedContent = "create schema if not exists inventory;\n" +
                              "create schema if not exists public;\n";
        Assert.Equal(expectedFilePath, result.FilePath);
        Assert.Equal(expectedContent, result.Content);
    }
}
