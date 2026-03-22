using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using StackExchange.Redis;
using System.Text.Json;
using Zuhid.Product.AviationModels;
using Zuhid.Product.Entities;
using Zuhid.Product.Repositories;

namespace Zuhid.Product.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportWeatherController(AppSetting appSetting, HttpClient httpClient,
  ConnectionMultiplexer connectionMultiplexer, CosmosContext cosmosContext, TafRepository tafRepository) : ControllerBase {

  [HttpGet("Json")]
  public async Task<string> Json(string icao = "kden") {
    var result = string.Empty;
    var response = await httpClient.GetAsync($"{appSetting.AviationUrl}?ids={icao}&format=json");
    if (response != null && response.IsSuccessStatusCode) {
      result = await response.Content.ReadAsStringAsync();
    }
    return result;
  }

  [HttpGet("Model")]
  public async Task<TafModel> Model(string icao = "kden") {
    var result = new TafModel();
    var response = await httpClient.GetAsync($"{appSetting.AviationUrl}?ids={icao}&format=json");
    if (response != null && response.IsSuccessStatusCode) {
      var list = await response.Content.ReadFromJsonAsync<List<TafModel>>();
      if (list != null && list.Count > 0) {
        result = list[0];
      }
    }
    return result;
  }


  [HttpGet("Redis")]
  public async Task<TafModel> Redis(string icao = "kden") {
    var database = connectionMultiplexer.GetDatabase();
    var tafData = await database.StringGetAsync($"taf:{icao.ToUpper()}");
    return JsonSerializer.Deserialize<TafModel>(tafData.ToString()) ?? new TafModel();
  }

  [HttpGet("Cosmos")]
  public async Task<TafModel> Cosmos(string icao = "kden") {
    var item = await cosmosContext.Tafs.ReadItemAsync<dynamic>(icao.ToUpper(), new PartitionKey(icao.ToUpper()));
    return Deserialize(item.Resource.ToString());
  }

  [HttpGet("Postgres")]
  public async Task<TafEntity?> Postgres(string icao = "kden") {
    return await tafRepository.Get(icao);
  }

  private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
  private TafModel Deserialize(string json) {
    return JsonSerializer.Deserialize<TafModel>(json, _jsonSerializerOptions) ?? new TafModel();
  }
}

