namespace Zuhid.Generator.Tools;

public static class PostgresTypeConverter
{
    public static string ToCsharpType(string postgresType, bool required)
    {
        var type = postgresType.ToLower() switch
        {
            "bigint" or "int8" => "long",
            "bigserial" or "serial8" => "long",
            "bit" or "varbit" => "bool",
            "bool" or "boolean" => "bool",
            "bytea" => "byte[]",
            "char" or "character" => "string",
            "date" => "DateTime",
            "decimal" or "numeric" => "decimal",
            "double precision" or "float8" => "double",
            "integer" or "int" or "int4" => "int",
            "interval" => "TimeSpan",
            "json" or "jsonb" => "string",
            "money" => "decimal",
            "real" or "float4" => "float",
            "smallint" or "int2" => "short",
            "smallserial" or "serial2" => "short",
            "serial" or "serial4" => "int",
            "text" or "citext" => "string",
            "time" or "timetz" => "TimeSpan",
            "timestamp" or "timestamptz" => "DateTime",
            "uuid" => "Guid",
            "varchar" or "character varying" => "string",
            "xml" => "string",
            _ => "object"
        };
        if (type == "string")
        {
            type = required ? "required string" : "string?";
        }
        // if (!required && type != "string" && type != "byte[]" && type != "object")
        // {
        //     type += "?";
        // }

        return type;
    }
}
