using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Zuhid.Base;
// using Microsoft.Extensions.DependencyInjection;

namespace [Company].[Product].Tests.Api;

public class BaseApiTest
{
    protected static readonly HttpClient Client = null!;

    static BaseApiTest()
    {
        var factory = new WebApplicationFactory<[Company].[Product].Api.Program>();
        factory.WithWebHostBuilder(static builder =>
          // ConfigureTestServices
          builder.ConfigureTestServices(static services =>
          {
              // services.AddTransient<IMessageService>(option => new MessageServiceMock(Path.Join(Path.GetTempPath(), "Zuhid.Auth.Tests")));
          }));
        Client = factory.CreateClient();
    }

    protected async Task BaseCrudTest<TModel>(string url, TModel addModel, TModel updateModel, bool allowDelete = true) where TModel : IEntity
    {

        // Get: Act and Assert
        var response = await Client.GetAsync(url);
        Assert.True(response.IsSuccessStatusCode, $"GET {url} failed with status code {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        await response.Content.ReadFromJsonAsync<List<TModel>>();

        // Add: Act and Assert
        response = await Client.PostAsync(url, new StringContent(JsonSerializer.Serialize(addModel), Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode, $"POST {url} failed with status code {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        response = await Client.GetAsync($"{url}?Id={addModel.Id}");
        var responseList = await response.Content.ReadFromJsonAsync<List<TModel>>();
        Assert.Equal(1, responseList?.Count);
        Assert.Equal(addModel.Id, responseList?[0].Id);

        // Update: Act and Assert
        response = await Client.PutAsync(url, new StringContent(JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode, $"PUT {url} failed with status code {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        response = await Client.GetAsync($"{url}?Id={addModel.Id}");
        responseList = await response.Content.ReadFromJsonAsync<List<TModel>>();
        Assert.Equal(1, responseList?.Count);
        Assert.Equal(updateModel.Id, responseList?[0].Id);

        // Delete: Act and Assert
        if (allowDelete)
        {
            response = await Client.DeleteAsync($"{url}?Id={addModel.Id}");
            Assert.True(response.IsSuccessStatusCode, $"DELETE {url} failed with status code {response.StatusCode}");
            response = await Client.GetAsync($"{url}?Id={addModel.Id}");
            responseList = await response.Content.ReadFromJsonAsync<List<TModel>>();
            Assert.Equal(0, responseList?.Count);
        }
    }
}
