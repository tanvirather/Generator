using System.ComponentModel.DataAnnotations;

namespace Zuhid.Product.Entities;

public class TafEntity {
  [Key]
  public string IcaoId { get; set; } = string.Empty;
  public DateTime? DbPopTime { get; set; }
  public DateTime? BulletinTime { get; set; }
  public DateTime? IssueTime { get; set; }
  public long? ValidTimeFrom { get; set; }
  public long? ValidTimeTo { get; set; }
  public string? RawTAF { get; set; }
  public int? MostRecent { get; set; }
  public string? Remarks { get; set; }
  public double? Lat { get; set; }
  public double? Lon { get; set; }
  public double? Elev { get; set; }
  public int? Prior { get; set; }
  public string? Name { get; set; }
  public List<TafForecastEntity> Fcsts { get; set; } = [];
}

public sealed class TafForecastEntity {
  public Guid Id { get; set; }
  public long? TimeFrom { get; set; }
  public long? TimeTo { get; set; }
  public long? TimeBec { get; set; }
  public string? FcstChange { get; set; }
  public int? Probability { get; set; }
  public int? Wdir { get; set; }
  public int? Wspd { get; set; }
  public int? Wgst { get; set; }
  public int? WshearHgt { get; set; }
  public int? WshearDir { get; set; }
  public int? WshearSpd { get; set; }
  public string? Visib { get; set; }
  public double? Altim { get; set; }
  public int? VertVis { get; set; }
  public string? WxString { get; set; }
  public string? NotDecoded { get; set; }
  public List<TafCloudLayerEntity> Clouds { get; set; } = [];
}

public sealed class TafCloudLayerEntity {
  public Guid Id { get; set; }
  public string Cover { get; set; } = string.Empty;
  public int? Base { get; set; }
  public string? Type { get; set; }
}
