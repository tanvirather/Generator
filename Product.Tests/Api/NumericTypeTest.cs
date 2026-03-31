using Zuhid.Product.Entities.Product;

namespace Zuhid.Product.Tests.Api;

public class NumericTypeTest : BaseApiTest
{
    [Fact]
    public async Task CrudTest()
    {
        // Arrange
        var addModel = new NumericTypeEntity
        {
            Id = Guid.NewGuid(),
            UpdatedById = Guid.NewGuid(),
            Updated = DateTime.UtcNow,
            SmallintType = 101,
            IntegerType = 101,
            BigintType = 101,
        };
        var updateModel = new NumericTypeEntity
        {
            Id = addModel.Id,
            UpdatedById = Guid.NewGuid(),
            Updated = DateTime.UtcNow.AddDays(1),
            SmallintType = 201,
            IntegerType = 201,
            BigintType = 201,
        };

        // Act and Assert
        await BaseCrudTest("NumericType", addModel, updateModel);
    }
}
