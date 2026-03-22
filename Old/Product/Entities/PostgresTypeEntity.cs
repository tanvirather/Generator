using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zuhid.Base;
using Zuhid.Product.Enums;

namespace Zuhid.Product.Entities;

public class PostgresTypeEntity : BaseEntity {
  public byte ByteType { get; set; } // smallint
  public short ShortType { get; set; } // smallint
  public int IntegerType { get; set; } // integer
  public long LongType { get; set; } // bigint
  [Precision(6, 3)] public decimal DecimalType { get; set; } // numeric(6,3)
  public float FloatType { get; set; } // real
  public double DoubleType { get; set; } // double precision
  public string? StringType { get; set; } // text
  [MaxLength(100)] public string? StringTypeMaxLength { get; set; } // character varying(100)
  public char? CharType { get; set; }  // character(1)
  [Column(TypeName = "timestamp without time zone")] public DateTime? DateTimeWithoutTzType { get; set; } // timestamp without time zone
  public DateTime? DateTimeType { get; set; } // timestamp with time zone
  public TimeSpan? TimeSpanType { get; set; } // interval
  public bool? BooleanType { get; set; } // boolean
  public Guid? UuidType { get; set; } // uuid
  public System.Net.IPAddress? InetType { get; set; } // inet
  [Column(TypeName = "json")] public string? JsonType { get; set; } // json
  [Column(TypeName = "jsonb")] public string? JsonbType { get; set; } // jsonb
  [Column(TypeName = "xml")] public string? XmlType { get; set; } // xml
  public BitArray? BitArrayType { get; set; } // bit varying
  public byte[]? ByteArrayType { get; set; } // bytea
  public short[]? ShortArrayType { get; set; } // smallint[]
  public int[]? IntArrayType { get; set; } // integer[]
  public long[]? LongArrayType { get; set; } // bigint[]
  public bool[]? BooloenArrayType { get; set; } // boolean[]
  public Guid[]? UuidArrayType { get; set; } // uuid[]
  public DateTime[]? DateTimeArrayType { get; set; } // timestamp with time zone[]
  public TimeSpan[]? TimeSpanArrayType { get; set; } // interval[]
  public Dictionary<string, string>? DictionaryType { get; set; } // hstore
  public IntegerEnumType? StatusType { get; set; } // integer
  public List<IntegerEnumType>? StatusListType { get; set; } // integer[]
  public ByteEnumType? ByteEnumType { get; set; } // smallint
  public List<ByteEnumType>? ByteEnumTypeList { get; set; } // smallint[]
  public ShortEnumType? ShortEnumType { get; set; } // smallint
  public List<ShortEnumType>? ShortEnumTypeList { get; set; } // smallint[]
  public IntegerEnumType? IntegerEnumType { get; set; } // integer
  public List<IntegerEnumType>? IntegerEnumTypeList { get; set; } // integer[]
  public LongEnumType? LongEnumType { get; set; } // bigint
  public List<LongEnumType>? LongEnumTypeList { get; set; } // bigint[]
  public UshortEnumType? UshortEnumType { get; set; } // integer
  public List<UshortEnumType>? UshortEnumTypeList { get; set; } // integer
  public UintegerEnumType? UintegerEnumType { get; set; } // bigint
  public List<UintegerEnumType>? UintegerEnumTypeList { get; set; } // bigint[]
  public UlongEnumType? UlongEnumType { get; set; } // numeric(20,0)
  public List<UlongEnumType>? UlongEnumTypeList { get; set; } // numeric(20,0)[]
  public EnumFlagType? EnumFlagType { get; set; } // smallint
  public List<EnumFlagType>? EnumFlagTypeList { get; set; } // smallint[]
}

