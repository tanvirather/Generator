using Newtonsoft.Json.Linq;

namespace Zuhid.Product.Etls;

public class CosmosEtl(AppSetting appSetting, CosmosContext cosmosContext) {
  public async Task Run() {
    var airports = new List<string> { "kden", "ksea", "kord" };
    var modelList = await Extract(airports);
    var jObjects = Transform(modelList);
    await Load(jObjects);
  }

  private async Task<List<string>> Extract(List<string> airports) {
    var client = new HttpClient();
    var modelList = new List<string>();
    foreach (var airport in airports) {
      var response = await client.GetAsync($"{appSetting.AviationUrl}?ids={airport}&format=json");
      if (response != null && response.IsSuccessStatusCode) {
        var models = await response.Content.ReadAsStringAsync();
        modelList.Add(models);
      }
    }
    return modelList;
  }

  private List<JObject> Transform(List<string> models) {
    var jObjects = new List<JObject>();
    foreach (var model in models) {
      var firstItem = (JObject)JArray.Parse(model)[0];
      firstItem["id"] = firstItem["icaoId"];
      jObjects.Add(firstItem);
    }
    return jObjects;
  }

  private async Task Load(List<JObject> modelList) {
    foreach (var model in modelList) {
      await cosmosContext.Tafs.UpsertItemAsync(item: model);
    }
  }
}
