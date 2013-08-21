using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YatORM.Extensions
{
    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (var item in input ?? Enumerable.Empty<T>())
            {
                action(item);
            }
        }

        [DebuggerStepThrough]
        public static TU GetOrDefault<T, TU>(this IDictionary<T, TU> dic, T key)
        {
            if (dic.ContainsKey(key)) return dic[key];
            return default(TU);
        }
    }
}
