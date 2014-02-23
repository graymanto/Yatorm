using System.Collections.Generic;
using System.Data;

namespace YatORM.Extensions
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