using System;
using System.Collections.Generic;
using System.Linq;

namespace Wolfpack.Periscope.Core.Extensions
{
    public static class EnumerableExtensions
    {
        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _lambdaComparer;
            private readonly Func<T, int> _lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer) :
                this(lambdaComparer, o => 0)
            {
            }

            private LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
            {
                if (lambdaComparer == null)
                    throw new ArgumentNullException("lambdaComparer");
                if (lambdaHash == null)
                    throw new ArgumentNullException("lambdaHash");
                _lambdaComparer = lambdaComparer;
                _lambdaHash = lambdaHash;
            }
            public bool Equals(T x, T y)
            {
                return _lambdaComparer(x, y);
            }
            public int GetHashCode(T obj)
            {
                return _lambdaHash(obj);
            }
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> enumerable, TSource value, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Contains(value, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Intersect(second, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Except(second, new LambdaComparer<TSource>(comparer));
        }
    }
}