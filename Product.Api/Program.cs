// using StackExchange.Redis;
using System.Reflection;
using Zuhid.Base;
// using Zuhid.Product.Hubs;

namespace Zuhid.Product.Api;

public class Program {
  public static void Main(string[] args) {
    var builder = WebApplicationExtension.AddServices(args);
    Assembly.GetAssembly(typeof(ProductContext))!.GetTypes().Where(s => s.IsClass && (
        s.Name.EndsWith("Repository")
        || s.Name.EndsWith("Mapper")
        || s.Name.EndsWith("Validator")
      ))
      .ToList()
      .ForEach(item => builder.Services.AddScoped(item));
    var appSetting = new AppSetting(builder.Configuration);
    builder.Services.AddSingleton(appSetting);
    builder.Services.AddSingleton(new HttpClient());
    // builder.Services.AddSingleton(ConnectionMultiplexer.Connect(appSetting.ConnectionStrings.Redis));
    // builder.Services.AddSingleton(sp => new CosmosContext(appSetting));

    builder.AddDatabase<ProductContext, ProductContext>(appSetting.ConnectionStrings.Product);
    // builder.Services.AddSignalR();
    var app = builder.BuildServices();
    // // app.MapGet("/api/health", () => new { status = "ok", time = DateTimeOffset.UtcNow });
    // app.MapHub<ChatHub>("/hubs/chat" /*, options => { options.Transports = ... }*/);
    // app.MapHub<TableHub>("/hubs/table");
    // app.MapHub<UserHub>("/hubs/user");
    app.Run();
  }
}
