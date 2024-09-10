using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Differencial.Domain.Util.ExtensionMethods
{
    public static class IEnumerableExtension
    {
        public static IEnumerable Append(this IEnumerable first, params object[] second)
        {
            return first.OfType<object>().Concat(second);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second)
        {
            return first.Concat(second);
        }

        public static int Count(this IEnumerable source)
        {
            return Enumerable.Count(source.Cast<object>());
        }

        //public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> query, ref Filter filter)
        //{
        //    filter.TotalRecords = query.Count();
        //    return query.Skip(filter.Skip).Take(filter.Take);
        //}

        public static IEnumerable<TSource> DistinctByTakeLastElements<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            HashSet<TSource> distincHashSet = new HashSet<TSource>();

            for (int i = source.Count() - 1; i >= 0; i--)
            {
                var element = source.ElementAt(i);
                if (seenKeys.Add(keySelector(element)))
                {
                    distincHashSet.Add(element);
                }
            }

            Func<TSource, int> teste = x => (int)x.GetType().GetProperty("Id").GetValue(x, null);

            return distincHashSet.OrderBy(teste).ToList();
        }
    }
}
