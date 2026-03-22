namespace Zuhid.Product.AviationModels;


public class TafModel {
  public string IcaoId { get; set; } = string.Empty;
  public DateTime? DbPopTime { get; set; }
  public DateTime? BulletinTime { get; set; }
  public DateTime? IssueTime { get; set; }
  public long? ValidTimeFrom { get; set; }
  public long? ValidTimeTo { get; set; }
  // public string? RawTAF { get; set; }
  public int? MostRecent { get; set; }
  public string? Remarks { get; set; }
  public double? Lat { get; set; }
  public double? Lon { get; set; }
  public double? Elev { get; set; }
  public int? Prior { get; set; }
  public string? Name { get; set; }

  public List<TafForecast> Fcsts { get; set; } = [];
}

public sealed class TafForecast {
  // Epoch seconds
  public long? TimeFrom { get; set; }
  public long? TimeTo { get; set; }
  public long? TimeBec { get; set; }
  public string? FcstChange { get; set; }
  public int? Probability { get; set; }
  public object? Wdir { get; set; }
  public int? Wspd { get; set; }
  public int? Wgst { get; set; }
  // public int? WshearHgt { get; set; }
  // public int? WshearDir { get; set; }
  // public int? WshearSpd { get; set; }
  public object? Visib { get; set; }
  // public double? Altim { get; set; }
  // public int? VertVis { get; set; }
  // public string? WxString { get; set; }
  // public string? NotDecoded { get; set; }
  public List<TafCloudLayer> Clouds { get; set; } = [];
  // public List<string> IcgTurb { get; set; } = [];
  // public List<string> Temp { get; set; } = [];
}

public sealed class TafCloudLayer {
  public string Cover { get; set; } = string.Empty;
  public int? Base { get; set; }
  // public string? Type { get; set; }
}

