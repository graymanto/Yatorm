using System.Diagnostics;

namespace Yatorm.Extensions
{
    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T>? input, Action<T> action)
        {
            foreach (var item in input ?? [])
            {
                action(item);
            }
        }

        [DebuggerStepThrough]
        public static TU GetOrDefault<T, TU>(this IDictionary<T, TU> dic, T key)
        {
            if (dic.ContainsKey(key))
                return dic[key];
            return default(TU);
        }
    }
}
