using System.Diagnostics;

namespace Yatorm.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string Formatted(this string input, params object[] args)
        {
            return string.Format(input, args);
        }
    }
}
