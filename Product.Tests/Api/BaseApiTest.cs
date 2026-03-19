using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;

namespace Zuhid.Product.Tests.Api;

public class BaseApiTest {
  protected static readonly HttpClient Client = null!;

  static BaseApiTest() {
    var factory = new WebApplicationFactory<Zuhid.Product.Api.Program>();
    factory.WithWebHostBuilder(static builder =>
      // ConfigureTestServices
      builder.ConfigureTestServices(static services => {
        // services.AddTransient<IMessageService>(option => new MessageServiceMock(Path.Join(Path.GetTempPath(), "Zuhid.Auth.Tests")));
      }));
    Client = factory.CreateClient();
  }
}
