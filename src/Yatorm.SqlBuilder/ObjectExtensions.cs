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
}
