using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Yatorm;

public class RandomCode
{
    private static T GetValue<T>(DbDataReader reader, Type effectiveType, object? val)
    {
        if (val is T tVal)
        {
            return tVal;
        }

        if (val is null && (!effectiveType.IsValueType || Nullable.GetUnderlyingType(effectiveType) is not null))
        {
            return default!;
        }

        if (val is Array array && typeof(T).IsArray)
        {
            var elementType = typeof(T).GetElementType()!;
            var result = Array.CreateInstance(elementType, array.Length);
            for (int i = 0; i < array.Length; i++)
                result.SetValue(Convert.ChangeType(array.GetValue(i), elementType, CultureInfo.InvariantCulture), i);
            return (T)(object)result;
        }

        try
        {
            var convertToType = Nullable.GetUnderlyingType(effectiveType) ?? effectiveType;
            return (T)Convert.ChangeType(val, convertToType, CultureInfo.InvariantCulture)!;
        }
        catch (Exception ex)
        {
            throw new DataException("Error converting value to type " + effectiveType, ex);
            // ThrowDataException(ex, 0, reader, val);
        }
    }
}
