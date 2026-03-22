namespace Zuhid.Product;

public class AppSetting {
  public string Name { get; init; } = default!;
  public string Version { get; init; } = default!;
  public string CorsOrigin { get; init; } = default!;
  public string AviationUrl { get; init; } = default!;
  public ConnectionString ConnectionStrings { get; init; } = default!;
  public CosmosSetting Cosmos { get; init; } = default!;

  public class ConnectionString {
    public string Product { get; init; } = default!;
    public string Log { get; init; } = default!;
    public string Redis { get; init; } = default!;
  }

  public class CosmosSetting {
    public string Endpoint { get; init; } = default!;
    public string Key { get; init; } = default!;
    public string DatabaseId { get; init; } = default!;
  }

  public AppSetting(IConfiguration configuration) {
    configuration.GetSection("AppSettings").Bind(this);
    ConnectionStrings = new ConnectionString {
      Product = ReplaceCredential(configuration, "Product"),
      Log = ReplaceCredential(configuration, "Log"),
      Redis = configuration.GetConnectionString("Redis") ?? string.Empty
    };
  }
  /// <summary>
  /// Get Connection string and replace "[postgres_credential]" with value from secrets
  /// </summary>
  /// <param name="configuration"></param>
  /// <param name="connString"></param>
  /// <returns></returns>
  private static string ReplaceCredential(IConfiguration configuration, string connString) {
    return (configuration.GetConnectionString(connString) ?? "")
      .Replace("[postgres_credential]", configuration.GetValue<string>("postgres_credential"), StringComparison.Ordinal);
  }
}
