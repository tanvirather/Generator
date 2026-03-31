using Zuhid.Product.Entities.List;

namespace Zuhid.Product.Tests.Api;

public class StateTest : BaseApiTest
{
    [Fact]
    public async Task CrudTest()
    {
        // Arrange
        var addModel = new StateEntity
        {
        };
        var updateModel = new StateEntity
        {
        };

        // Act and Assert
        await BaseCrudTest("State", addModel, updateModel);
    }
}
