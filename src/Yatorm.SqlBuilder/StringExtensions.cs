namespace Yatorm.Tools;

internal static class StringExtensions
{
    internal static string JoinWithComma(this IEnumerable<string> strings) => string.Join(", ", strings);
}
