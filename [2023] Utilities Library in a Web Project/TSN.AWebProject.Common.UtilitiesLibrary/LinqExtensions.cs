using System;
using System.Collections.Generic;
using System.Linq;

namespace TSN.AWebProject.Common.UtilitiesLibrary
{
    public static class LinqExtensions
    {
        public static IList<T> ShuffleList<T>(this IList<T> collection)
        {
            if (collection == null)
                return null;
            var temp = collection.ToArray();
            temp.Shuffle();
            return temp;
        }
        public static IEnumerable<T> ShuffleList<T>(this IList<T> collection, out IList<int> indices)
        {
            if (collection == null)
            {
                indices = Array.Empty<int>();
                return null;
            }
            indices = Enumerable.Range(0, collection.Count).ToList();
            indices.Shuffle();
            return indices.Select(x => collection[x]);
        }
        public static IEnumerable<T> UndoShuffle<T>(this IEnumerable<T> collection, IList<int> indices) => collection.Select((x, i) => new { Index = indices[i], Char = x }).OrderBy(x => x.Index).Select(x => x.Char);
        public static T[] MakeArray<T>(this T item) => new[] { item };
        public static IEnumerable<T> Concat<T>(this T item, IEnumerable<T> collection)
        {
            var col = collection ?? new T[0];
            return item == null ? collection : new[] { item }.Concat(col);
        }
        public static IEnumerable<T[]> Split<T>(this ICollection<T> collection, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            var supArray = collection.ToArray();
            var div = Math.DivRem(collection.Count, length, out var rem);
            for (int i = 0; i < div; i++)
            {
                var array = new T[length];
                for (int j = i * length, x = 0; x < length; j++)
                    array[x++] = supArray[j];
                yield return array;
            }
            if (rem > 0)
            {
                var array = new T[rem];
                for (int i = div, x = 0; x < rem; i++)
                    array[x++] = supArray[i];
                yield return array;
            }
        }
        public static IEnumerable<T> Union<T>(this T item, IEnumerable<T> collection)
        {
            var col = collection ?? new T[0];
            return item == null ? collection : new[] { item }.Union(col);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }
    }
}