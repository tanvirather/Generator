using System.Text.RegularExpressions;

namespace Zuhid.Generator.Tools;

public static class StringExtension
{
    public static string ToSnakeCase(this string str)
    {
        var result = Regex.Replace(str, "([A-Z][a-z]|(?<=[a-z])[^a-z]|(?<=[A-Z])[0-9_])", "_$1").ToLower();
        return result.StartsWith('_') ? result[1..] : result;
    }

    public static string ToPascalCase(this string str)
    {
        return string.Concat(str.Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => char.ToUpper(s[0]) + s[1..]));
    }

    public static string ToCamelCase(this string str)
    {
        return string.Concat(str.Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => char.ToLower(s[0]) + s[1..]));
    }

}
