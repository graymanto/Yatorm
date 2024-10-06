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

    internal static string StringifySqlValue(this object value)
    {
        if (value is null)
        {
            throw new ArgumentException("Value to stringify can not be null");
        }

        return value.GetType().IsNumericType() || value.IsParameterBinding() ? value.ToString() ?? "" : $"'{value}'";
    }

    internal static string StringifySqlValue<T>(this T value)
    {
        if (value is null)
        {
            throw new ArgumentException("Value to stringify can not be null");
        }

        return value.GetType().IsNumericType() || value.IsParameterBinding() ? value.ToString() ?? "" : $"'{value}'";
    }

    internal static IEnumerable<string> StringifySqlValues<T>(this IEnumerable<T> values)
    {
        if (values is null)
        {
            throw new ArgumentException("Values to stringify can not be null");
        }

        return values.Select(StringifySqlValue);
    }
}
