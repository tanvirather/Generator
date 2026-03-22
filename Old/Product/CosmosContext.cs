using Microsoft.Azure.Cosmos;

namespace Zuhid.Product;

public interface ICosmosContext {
  Container Tafs { get; }
}


public class CosmosContext : ICosmosContext {

  public Container Tafs { get; private set; } = default!;

  public CosmosContext(AppSetting appSetting) {
    Initialize(appSetting).GetAwaiter().GetResult(); // blocking call to initialize the context
  }

  private async Task Initialize(AppSetting appSetting) {
    var options = new CosmosClientOptions() {
#if DEBUG
      HttpClientFactory = () => new HttpClient(new HttpClientHandler() {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      }),
#endif
      ConnectionMode = ConnectionMode.Gateway,
    };
    var cosmosClient = new CosmosClient(appSetting.Cosmos.Endpoint, appSetting.Cosmos.Key, options);

    // Create DB if not exists
    var dbResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(appSetting.Cosmos.DatabaseId).ConfigureAwait(false);
    var database = dbResponse.Database;

    // Users container
    var tafsProps = new ContainerProperties(id: "Tafs", partitionKeyPath: "/icaoId") {
      IndexingPolicy = new IndexingPolicy { Automatic = true, IndexingMode = IndexingMode.Lazy }, // indexing policy tuning
      DefaultTimeToLive = 60 * 60 * 24 * 1 // 1 day TTL for automatic deletion of items
    };
    Tafs = (await database.CreateContainerIfNotExistsAsync(tafsProps, ThroughputProperties.CreateManualThroughput(1000)).ConfigureAwait(false)).Container;
  }
}

