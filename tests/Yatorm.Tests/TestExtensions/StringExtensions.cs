using System.Text;
using System.Text.RegularExpressions;

namespace Yatorm.Tests.TestExtensions;

public static partial class StringExtensions
{
    public static string NormalizeForComparison(this string input)
    {
        input = input.Normalize(NormalizationForm.FormC).Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        input = WhiteSpaceRegex().Replace(input, " ");
        return input.ToLowerInvariant().Trim();
    }

    [GeneratedRegex(@"\s{2,}")]
    private static partial Regex WhiteSpaceRegex();
}
