package generate

import (
	"fmt"
	"strings"

	"zuhid.com/generator/models"
	"zuhid.com/generator/tools"
)

// Csharp represents a C# code generator.
type Csharp struct {
	Company    string
	Product    string
	InputPath  string
	OutputPath string
}

// Generate generates C# code based on the provided table data.
func (csharp *Csharp) Generate(tables []models.Table) {
	template := Template{
		Company:    csharp.Company,
		Product:    csharp.Product,
		OutputPath: csharp.OutputPath,
		Folders:    []string{"[Product]", "[Product].Api", "[Product].Tests"},
	}

	template.Generate()

	for _, table := range tables {
		filePath := fmt.Sprintf("%s/%s/%s.csv", csharp.InputPath, table.Schema, table.Table)
		csv := tools.Csv{}
		columns, _ := csv.ReadColumns(filePath)

		_ = csharp.generateEntities(table, columns)
		_ = csharp.generateModels(table, columns)
		_ = csharp.generateRepositories(table, columns)
		_ = csharp.generateControllers(table, columns)
		_ = csharp.generateApiTest(table, columns)
	}

	_ = csharp.generateDbContext(tables)
}

// PostgresToCsharpType returns the C# type corresponding to the provided PostgreSQL type.
func (csharp *Csharp) PostgresToCsharpType(postgresType string) string {
	// https://www.postgresql.org/docs/current/datatype.html
	types := map[string]string{
		// numeric type
		"smallint": "short",
		"integer":  "int",
		"bigint":   "long",
		// other
		"int8":                        "long",
		"bigserial":                   "long",
		"serial8":                     "long",
		"bit":                         "bool",
		"boolean":                     "bool",
		"bool":                        "bool",
		"box":                         "string",
		"bytea":                       "byte[]",
		"character":                   "string",
		"char":                        "string",
		"character varying":           "string",
		"varchar":                     "string",
		"cidr":                        "string",
		"circle":                      "string",
		"date":                        "DateTime",
		"double precision":            "double",
		"float8":                      "double",
		"inet":                        "string",
		"int":                         "int",
		"int4":                        "int",
		"interval":                    "TimeSpan",
		"json":                        "string",
		"jsonb":                       "string",
		"line":                        "string",
		"lseg":                        "string",
		"macaddr":                     "string",
		"macaddr8":                    "string",
		"money":                       "decimal",
		"numeric":                     "decimal",
		"decimal":                     "decimal",
		"path":                        "string",
		"pg_lsn":                      "long",
		"point":                       "string",
		"polygon":                     "string",
		"real":                        "float",
		"float4":                      "float",
		"int2":                        "short",
		"smallserial":                 "short",
		"serial2":                     "short",
		"serial":                      "int",
		"serial4":                     "int",
		"text":                        "string",
		"time":                        "TimeSpan",
		"time without time zone":      "TimeSpan",
		"time with time zone":         "DateTimeOffset",
		"timetz":                      "DateTimeOffset",
		"timestamp":                   "DateTime",
		"timestamp without time zone": "DateTime",
		"timestamp with time zone":    "DateTimeOffset",
		"timestamptz":                 "DateTimeOffset",
		"tsquery":                     "string",
		"tsvector":                    "string",
		"txid_snapshot":               "string",
		"uuid":                        "Guid",
		"xml":                         "string",
	}

	if val, ok := types[postgresType]; ok {
		return val
	}

	return "string"
}

func (csharp *Csharp) generateDbContext(tables []models.Table) error {

	content := fmt.Sprintf(`using Microsoft.EntityFrameworkCore;
using %[1]s.Base;
using %[1]s.%[2]s.Entities;

namespace %[1]s.%[2]s;

public class %[2]sContext(DbContextOptions<%[2]sContext> options) : DbContext(options)
{
`, csharp.Company, csharp.Product)

	for _, table := range tables {
		entityName := table.TableCsharp()
		content += fmt.Sprintf("    public DbSet<%sEntity> %s { get; set; }\n", entityName, entityName)
	}

	content += fmt.Sprintf(`
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("%[1]s");
    }
}
`, strings.ToLower(csharp.Product))

	directory := fmt.Sprintf("%s/%s", csharp.OutputPath, csharp.Product)
	fileName := fmt.Sprintf("%s.cs", csharp.Product+"Context")

	tools.CreateFile(directory, fileName, content)

	return nil
}

func (csharp *Csharp) generateEntities(table models.Table, columns []models.Column) error {
	entityName := table.TableCsharp() + "Entity"

	content := fmt.Sprintf(`using Zuhid.Base;
namespace %s.%s.Entities.%s;

public class %s : IEntity
{
`, csharp.Company, csharp.Product, table.Schema, entityName)

	for _, col := range columns {
		csharpType := csharp.PostgresToCsharpType(col.Datatype)
		content += fmt.Sprintf("    public %s %s { get; set; }\n", csharpType, col.ColumnCsharp())
	}

	content += "}\n"

	directory := fmt.Sprintf("%s/%s/Entities/%s", csharp.OutputPath, csharp.Product, table.Schema)
	fileName := fmt.Sprintf("%s.cs", entityName)

	tools.CreateFile(directory, fileName, content)

	return nil
}

func (csharp *Csharp) generateModels(table models.Table, columns []models.Column) error {
	modelName := table.TableCsharp() + "Model"
	entityName := table.TableCsharp() + "Entity"

	content := fmt.Sprintf(`using %s.%s.Entities.%s;

namespace %s.%s.Models.%s;

public class %s : %s
{
`, csharp.Company, csharp.Product, table.Schema, csharp.Company, csharp.Product, table.Schema, modelName, entityName)

	// for _, col := range columns {
	// 	csharpType := csharp.PostgresToCsharpType(col.Datatype)
	// 	content += fmt.Sprintf("    public %s %s { get; set; }\n", csharpType, col.Column)
	// }

	content += "}\n"

	directory := fmt.Sprintf("%s/%s/Models/%s", csharp.OutputPath, csharp.Product, table.Schema)
	fileName := fmt.Sprintf("%s.cs", modelName)

	tools.CreateFile(directory, fileName, content)

	return nil
}

func (csharp *Csharp) generateRepositories(table models.Table, columns []models.Column) error {
	repositoryName := table.TableCsharp() + "Repository"
	modelName := table.TableCsharp() + "Model"
	entityName := table.TableCsharp() + "Entity"

	content := fmt.Sprintf(`using Zuhid.Base;
using %[1]s.%[2]s.Entities.%[6]s;
using %[1]s.%[2]s.Models.%[6]s;

namespace %[1]s.%[2]s.Repositories.%[6]s;

public class %[3]sRepository(%[2]sContext context) : BaseRepository<%[2]sContext, %[4]s, %[5]s>(context)
{
    protected override IQueryable<%[5]s> Query => _context.%[3]s.Select(entity => new %[5]s
    {
`, csharp.Company, csharp.Product, table.TableCsharp(), entityName, modelName, table.SchemaCsharp())

	for _, col := range columns {
		content += fmt.Sprintf("        %s = entity.%s,\n", col.ColumnCsharp(), col.ColumnCsharp())
	}

	content += "    });\n"
	content += "}\n"

	directory := fmt.Sprintf("%s/%s/Repositories", csharp.OutputPath, csharp.Product)
	fileName := fmt.Sprintf("%s.cs", repositoryName)

	tools.CreateFile(directory, fileName, content)

	return nil
}

func (csharp *Csharp) generateControllers(table models.Table, columns []models.Column) error {
	controllerName := table.TableCsharp() + "Controller"
	repositoryName := table.TableCsharp() + "Repository"
	modelName := table.TableCsharp() + "Model"
	entityName := table.TableCsharp() + "Entity"

	content := fmt.Sprintf(`using Microsoft.AspNetCore.Components;
using %[1]s.Base;
using %[1]s.%[2]s.Entities.%[7]s;
using %[1]s.%[2]s.Models.%[7]s;
using %[1]s.%[2]s.Repositories.%[7]s;

namespace %[1]s.%[2]s.Controllers.%[7]s;

[Route("%[7]s/[controller]")]
public class %[3]s(%[4]s repository, BaseMapper<%[5]s, %[6]s> mapper)
  : BaseCrudController<%[4]s, BaseMapper<%[5]s, %[6]s>, %[2]sContext, %[5]s, %[6]s>(repository, mapper)
{
}
`, csharp.Company, csharp.Product, controllerName, repositoryName, entityName, modelName, table.SchemaCsharp())

	directory := fmt.Sprintf("%s/%s/Controllers/%s", csharp.OutputPath, csharp.Product, table.SchemaCsharp())
	fileName := fmt.Sprintf("%s.cs", controllerName)
	tools.CreateFile(directory, fileName, content)

	return nil
}

func (csharp *Csharp) generateApiTest(table models.Table, columns []models.Column) error {
	entityName := table.TableCsharp() + "Entity"
	testName := table.TableCsharp() + "Test"

	var addFields string
	var updateFields string

	for _, col := range columns {
		csharpType := csharp.PostgresToCsharpType(col.Datatype)
		columnName := col.ColumnCsharp()

		if columnName == "Id" {
			addFields += "            Id = Guid.NewGuid(),\n"
			updateFields += "            Id = addModel.Id,\n"
			continue
		}

		var addValue string
		var updateValue string

		switch csharpType {
		case "short", "int", "long":
			addValue = "101"
			updateValue = "201"
		case "double", "decimal", "float":
			addValue = "10.1"
			updateValue = "20.1"
		case "bool":
			addValue = "true"
			updateValue = "false"
		case "DateTime", "DateTimeOffset":
			addValue = "DateTime.UtcNow"
			updateValue = "DateTime.UtcNow.AddDays(1)"
		case "TimeSpan":
			addValue = "TimeSpan.FromHours(1)"
			updateValue = "TimeSpan.FromHours(2)"
		case "Guid":
			addValue = "Guid.NewGuid()"
			updateValue = "Guid.NewGuid()"
		default:
			addValue = fmt.Sprintf("\"Add %s\"", columnName)
			updateValue = fmt.Sprintf("\"Update %s\"", columnName)
		}

		addFields += fmt.Sprintf("            %s = %s,\n", columnName, addValue)
		updateFields += fmt.Sprintf("            %s = %s,\n", columnName, updateValue)
	}

	content := fmt.Sprintf(`using %[1]s.%[2]s.Entities;

namespace %[1]s.%[2]s.Tests.Api;

public class %[3]s : BaseApiTest
{
    [Fact]
    public async Task CrudTest()
    {
        // Arrange
        var addModel = new %[4]s
        {
%[5]s        };
        var updateModel = new %[4]s
        {
%[6]s        };

        // Act and Assert
        await BaseCrudTest("%[7]s", addModel, updateModel);
    }
}
`, csharp.Company, csharp.Product, testName, entityName, addFields, updateFields, table.TableCsharp())

	directory := fmt.Sprintf("%s/%s.Tests/Api", csharp.OutputPath, csharp.Product)
	fileName := fmt.Sprintf("%s.cs", testName)

	tools.CreateFile(directory, fileName, content)

	return nil
}
