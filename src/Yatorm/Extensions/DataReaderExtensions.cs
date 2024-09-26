using System.Data;

namespace Yatorm.Extensions
{
    public static class DataReaderExtensions
    {
        public static IEnumerable<string> GetColumnNames(this IDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                yield return reader.GetName(i);
            }
        }
    }
}
