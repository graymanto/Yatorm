using System;
using System.Collections.Concurrent;

namespace YatORM.Extensions
{
    public static class FuncExtensions
    {
        public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> instance)
        {
            var cache = new ConcurrentDictionary<Tuple<T1>, TResult>();

            return t1 =>
                {
                    var key = Tuple.Create(t1);

                    return cache.GetOrAdd(key, x => instance(t1));
                };
        }

        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> instance)
        {
            var cache = new ConcurrentDictionary<Tuple<T1, T2>, TResult>();

            return (t1, t2) =>
                {
                    var key = Tuple.Create(t1, t2);

                    return cache.GetOrAdd(key, x => instance(t1, t2));
                };
        }
    }
}