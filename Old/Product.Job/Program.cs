// using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Zuhid.Product.Etls;

namespace Zuhid.Product.Job;

/// <summary>
/// export DOTNET_ENVIRONMENT=Development
/// dotnet run --PostgresEtl
/// dotnet run --CosmosEtl
/// dotnet run --RedisEtl
/// </summary>
public class Program {
  static async Task Main(string[] args) {
    var config = GetConfigurationRoot();
    var appSetting = new Zuhid.Product.AppSetting(config);

    if (args.Contains("--PostgresEtl")) {
      var postgresContext = new ProductContext(new DbContextOptionsBuilder<ProductContext>().UseNpgsql(appSetting.ConnectionStrings.Product).Options);
      await new PostgresEtl(appSetting, postgresContext).Run();
    }
    if (args.Contains("--CosmosEtl")) {
      var cosmosContext = new CosmosContext(appSetting);
      await new CosmosEtl(appSetting, cosmosContext).Run();
    }
    if (args.Contains("--RedisEtl")) {
      await new RedisEtl(appSetting, new HttpClient(), ConnectionMultiplexer.Connect(appSetting.ConnectionStrings.Redis)).Run();
    }
  }

  private static IConfigurationRoot GetConfigurationRoot() {
    var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    var builder = new ConfigurationBuilder()
      .SetBasePath(AppContext.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    if (!string.IsNullOrEmpty(environment)) {
      builder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
      // Add user secrets in development environment only
      if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase)) {
        builder.AddUserSecrets<Program>();
      }
    }
    return builder
      .AddEnvironmentVariables() // allow overrides from env vars
      .Build();
  }
}
