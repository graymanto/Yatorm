namespace Yatorm.Extensions
{
    public static class NumericExtensions
    {
        public static IEnumerable<int> UpTo(this int start, int end)
        {
            return Enumerable.Range(start, end - start + 1);
        }
    }
}
