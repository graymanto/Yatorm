using System.Diagnostics;

namespace Yatorm.Tools;

internal static class ObjectExtensions
{
    [DebuggerStepThrough]
    public static bool IsParameterBinding(this object input)
    {
        string? stringInput = input.ToString();
        if (stringInput is null)
        {
            return false;
        }

        return stringInput.Contains("@") || stringInput.Contains("$") || stringInput.Contains("{");
    }

    internal static string StringifySqlValue(this object? value, SqlSyntax syntax = SqlSyntax.Standard)
    {
        if (value is null)
        {
            return "null";
        }

        var valueType = value.GetType();

        if (Type.GetTypeCode(valueType) == TypeCode.Boolean && syntax == SqlSyntax.SqlServer)
        {
            return (bool)value ? "1" : "0";
        }

        return valueType.IsNumericType() || value.IsParameterBinding() ? value.ToString() ?? "" : $"'{value}'";
    }

    internal static string StringifySqlValue<T>(this T value, SqlSyntax syntax = SqlSyntax.Standard)
    {
        if (value is null)
        {
            throw new ArgumentException("Value to stringify can not be null");
        }

        var valueType = value.GetType();

        if (valueType == typeof(bool) && syntax == SqlSyntax.SqlServer)
        {
            dynamic dynamicValue = value;
            return (bool)dynamicValue ? "1" : "0";
        }

        return value.GetType().IsNumericType() || value.IsParameterBinding() ? value.ToString() ?? "" : $"'{value}'";
    }

    internal static IEnumerable<string> StringifySqlValues<T>(
        this IEnumerable<T> values,
        SqlSyntax syntax = SqlSyntax.Standard
    )
    {
        if (values is null)
        {
            throw new ArgumentException("Values to stringify can not be null");
        }

        return values.Select(v => v.StringifySqlValue(syntax));
    }
}
