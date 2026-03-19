using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Zuhid.Product.Entities;

namespace Zuhid.Product.Tests.Api;

public class ProductTest : BaseApiTest {
  [Fact]
  public async Task CrudTest() {

    // Arrange
    var addModel = new PostgresTypeEntity {
      Id = Guid.NewGuid(),
      ShortType = 1,
    };
    var updateModel = new PostgresTypeEntity {
      Id = addModel.Id,
      ShortType = 2,
    };

    // Get: Act and Assert
    var response = await Client.GetAsync($"/PostgresType");
    var responseList = await response.Content.ReadFromJsonAsync<List<PostgresTypeEntity>>();
    Assert.True(response.IsSuccessStatusCode);

    // Add: Act and Assert
    response = await Client.PostAsync("/PostgresType", new StringContent(JsonSerializer.Serialize(addModel), Encoding.UTF8, "application/json"));
    Assert.True(response.IsSuccessStatusCode);
    response = await Client.GetAsync($"/PostgresType?Id={addModel.Id}");
    responseList = await response.Content.ReadFromJsonAsync<List<PostgresTypeEntity>>();
    Assert.Equal(1, responseList?.Count);
    Assert.Equal(addModel.Id, responseList?[0].Id);
    Assert.Equal(addModel.ShortType, responseList?[0].ShortType);

    // Update: Act and Assert
    response = await Client.PutAsync("/PostgresType", new StringContent(JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json"));
    Assert.True(response.IsSuccessStatusCode);
    response = await Client.GetAsync($"/PostgresType?Id={addModel.Id}");
    responseList = await response.Content.ReadFromJsonAsync<List<PostgresTypeEntity>>();
    Assert.Equal(1, responseList?.Count);
    Assert.Equal(updateModel.Id, responseList?[0].Id);
    Assert.Equal(updateModel.ShortType, responseList?[0].ShortType);

    // Delete: Act and Assert 
    response = await Client.DeleteAsync($"/PostgresType?Id={addModel.Id}");
    Assert.True(response.IsSuccessStatusCode);
    response = await Client.GetAsync($"/PostgresType?Id={addModel.Id}");
    responseList = await response.Content.ReadFromJsonAsync<List<PostgresTypeEntity>>();
    Assert.Equal(0, responseList?.Count);
  }
}
