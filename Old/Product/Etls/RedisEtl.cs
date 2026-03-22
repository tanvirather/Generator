using StackExchange.Redis;
using Zuhid.Product.AviationModels;
using Zuhid.Product.Entities;

namespace Zuhid.Product.Etls;

public class RedisEtl(AppSetting appSetting, HttpClient client, ConnectionMultiplexer connectionMultiplexer) {
  public async Task Run() {
    var airports = new List<string> { "kden", "ksea", "kord" };
    var modelList = await Extract(airports);
    var entityList = Transform(modelList);
    await Load(entityList);
  }

  private async Task<List<TafModel>> Extract(List<string> airports) {
    var modelList = new List<TafModel>();
    foreach (var airport in airports) {
      var response = await client.GetAsync($"{appSetting.AviationUrl}?ids={airport}&format=json");
      if (response != null && response.IsSuccessStatusCode) {
        var models = await response.Content.ReadFromJsonAsync<List<TafModel>>() ?? [];
        modelList.AddRange(models);
      }
    }
    return modelList;
  }

  private List<TafEntity> Transform(List<TafModel> models) {
    return [.. models.Select(m => new TafEntity {
      IcaoId = m.IcaoId,
      DbPopTime = m.DbPopTime,
      BulletinTime = m.BulletinTime,
      IssueTime = m.IssueTime,
      ValidTimeFrom = m.ValidTimeFrom,
      ValidTimeTo = m.ValidTimeTo,
      // RawTAF = m.RawTAF,
      MostRecent = m.MostRecent,
      Remarks = m.Remarks,
      Lat = m.Lat,
      Lon = m.Lon,
      Elev = m.Elev,
      Prior = m.Prior,
      Name = m.Name,
      Fcsts = [.. m.Fcsts.Select(f => new TafForecastEntity {
        TimeFrom = f.TimeFrom,
        TimeTo = f.TimeTo,
        TimeBec = f.TimeBec,
        FcstChange = f.FcstChange,
        Probability = f.Probability,
        // Wdir = f.Wdir,
        Wspd = f.Wspd,
        Wgst = f.Wgst,
        // WshearHgt = f.WshearHgt,
        // WshearDir = f.WshearDir,
        // WshearSpd = f.WshearSpd,
        Visib = f.Visib?.ToString(),
        // Altim = f.Altim,
        // VertVis = f.VertVis,
        // WxString = f.WxString,
        // NotDecoded = f.NotDecoded,
        Clouds = [.. f.Clouds.Select(c => new TafCloudLayerEntity {
          Cover = c.Cover,
          Base = c.Base,
          // Type = c.Type
        })]
      })]
    })];
  }

  private async Task Load(List<TafEntity> entityList) {
    var database = connectionMultiplexer.GetDatabase();
    foreach (var entity in entityList) {
      var key = $"taf:{entity.IcaoId}";
      await database.StringSetAsync(key, System.Text.Json.JsonSerializer.Serialize(entity));
    }
  }
}
