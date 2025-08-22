using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TSN.Utility.Exceptions;

namespace TSN.Utility.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> ToGenericEnumerable<T>(this IEnumerable collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
                yield return (T)enumerator.Current;
        }
        public static ArrayList ToArrayList(this IEnumerable collection)
        {
            if (collection == null)
                return null;
            var list = new ArrayList();
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);
            list.TrimToSize();
            return list;
        }
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> collection)
        {
            return collection?.ToList().AsReadOnly();
        }
        public static TResult Maximum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
            where TValue : IComparable<TValue>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || _item.CompareTo(max) > 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || _item.CompareTo(max) >= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static TResult Maximum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, IComparer<TValue> comparer, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer.Compare(_item, max) > 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer.Compare(_item, max) >= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static TResult Maximum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, Func<TValue, TValue, int> comparer, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer(_item, max) > 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer(_item, max) >= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static TResult Minimum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
            where TValue : IComparable<TValue>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || _item.CompareTo(max) < 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || _item.CompareTo(max) <= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static TResult Minimum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, IComparer<TValue> comparer, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer.Compare(_item, max) < 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer.Compare(_item, max) <= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static TResult Minimum<TElement, TValue, TResult>(this IEnumerable<TElement> collection, Func<TElement, TValue> innerValueSelectorToCompare, Func<TElement, TResult> resultSelector, Func<TValue, TValue, int> comparer, out int index, MinMaxConfliction ifEquals = MinMaxConfliction.TakeFirst)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (!ifEquals.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(ifEquals));
            if (innerValueSelectorToCompare == null)
                throw new ArgumentNullException(nameof(innerValueSelectorToCompare));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            TValue max = default(TValue);
            TResult result = default(TResult);
            index = -1;
            int count = -1;
            switch (ifEquals)
            {
                case MinMaxConfliction.TakeFirst:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer(_item, max) < 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
                case MinMaxConfliction.TakeLast:
                    foreach (var item in collection)
                    {
                        var _item = innerValueSelectorToCompare(item);
                        if (++count == 0 || comparer(_item, max) <= 1)
                        {
                            max = _item;
                            result = resultSelector(item);
                            index = count;
                        }
                    }
                    break;
            }
            if (count == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Collection, nameof(collection));
            return result;
        }
        public static IEnumerable<TResult> Distinct<TElement, TResult>(this IEnumerable<TElement> collection, Func<TElement, TResult> selector)
        {
            return collection.Select(selector).Distinct();
        }
        public static IEnumerable<TResult> Distinct<TElement, TResult>(this IEnumerable<TElement> collection, Func<TElement, TResult> selector, IEqualityComparer<TResult> comparer)
        {
            return collection.Select(selector).Distinct(comparer);
        }
        public static IEnumerable<TElement> DistinctByHashCode<TElement>(this IEnumerable<TElement> collection)
        {
            var hashCodes = new HashSet<int>();
            foreach (var item in collection)
                if (hashCodes.Add(item?.GetHashCode() ?? 0))
                    yield return item;
        }
        public static IEnumerable<TResult> DistinctByHashCode<TElement, TResult>(this IEnumerable<TElement> collection, Func<TElement, TResult> selector)
        {
            var hashCodes = new HashSet<int>();
            foreach (var item in collection.Select(selector))
                if (hashCodes.Add(item?.GetHashCode() ?? 0))
                    yield return item;
        }
        public static bool IsDistinct<TElement>(this ICollection<TElement> collection)
        {
            return collection.Count == collection.Distinct().Count();
        }
        public static bool IsDistinct<TElement, TResult>(this ICollection<TElement> collection, Func<TElement, TResult> selector)
        {
            return collection.Count == collection.Distinct(selector).Count();
        }
        public static bool IsDistinct<TElement, TResult>(this ICollection<TElement> collection, Func<TElement, TResult> selector, IEqualityComparer<TResult> comparer)
        {
            return collection.Count == collection.Distinct(selector, comparer).Count();
        }
        public static bool IsDistinctByHashCode<TElement>(this ICollection<TElement> collection)
        {
            return collection.Count == collection.Select(e => e?.GetHashCode() ?? 0).Distinct().Count();
        }
        public static bool IsDistinctByHashCode<TElement, TResult>(this ICollection<TElement> collection, Func<TElement, TResult> selector)
        {
            return collection.Count == collection.Distinct(selector).Select(e => e?.GetHashCode() ?? 0).Count();
        }
        public static bool Contains<TElement>(this IEnumerable<TElement> collection, TElement value, Func<TElement, TElement, bool> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            return collection.Any(e => comparer(value, e));
        }
        public static bool Contains<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return collection.Any(e =>
            {
                var _item = transform(e);
                return (value == null) == (_item == null) && (value == null || value.Equals(_item));
            });
        }
        public static bool Contains<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform, IEqualityComparer<TValue> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            return collection.Any(e => comparer.Equals(value, transform(e)));
        }
        public static bool Contains<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform, Func<TValue, TValue, bool> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            return collection.Any(e => comparer(value, transform(e)));
        }
        public static bool ContainsByHashCode<TElement>(this IEnumerable<TElement> collection, TElement value)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            int hash = value?.GetHashCode() ?? 0;
            return collection.Any(e => hash == (e?.GetHashCode() ?? 0));
        }
        public static bool ContainsByHashCode<TElement>(this IEnumerable<TElement> collection, TElement value, IEqualityComparer<TElement> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            int hash = comparer.GetHashCode(value);
            return collection.Any(e => hash == comparer.GetHashCode(e));
        }
        public static bool ContainsByHashCode<TElement>(this IEnumerable<TElement> collection, TElement value, Func<TElement, int> getHashCode)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (getHashCode == null)
                throw new ArgumentNullException(nameof(getHashCode));
            int hash = getHashCode(value);
            return collection.Any(e => hash == getHashCode(e));
        }
        public static bool ContainsByHashCode<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            int hash = value?.GetHashCode() ?? 0;
            return collection.Any(e => hash == (transform(e)?.GetHashCode() ?? 0));
        }
        public static bool ContainsByHashCode<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform, IEqualityComparer<TValue> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            int hash = comparer.GetHashCode(value);
            return collection.Any(e => hash == comparer.GetHashCode(transform(e)));
        }
        public static bool ContainsByHashCode<TElement, TValue>(this IEnumerable<TElement> collection, TValue value, Func<TElement, TValue> transform, Func<TValue, int> getHashCode)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (getHashCode == null)
                throw new ArgumentNullException(nameof(getHashCode));
            int hash = getHashCode(value);
            return collection.Any(e => hash == getHashCode(transform(e)));
        }
        public static T[] MakeArray<T>(this T obj)
        {
            return new T[] { obj };
        }
        public static TResult[] MakeArray<TObject, TResult>(this TObject obj)
        {
            return new TResult[] { (TResult)(object)obj };
        }
        public static void ForEach<TElement>(this IEnumerable<TElement> elements, Action<TElement> action)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var item in elements)
                action(item);
        }
        public static void DisposeAll<T>(this IEnumerable<T> collection)
            where T : IDisposable
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            collection.ForEach(e => e?.Dispose());
        }
        public static IEnumerable<T> GetHierarchy<T>(this T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));
            if (childrenSelector == null)
                throw new ArgumentNullException(nameof(childrenSelector));
            var queue = new Queue<T>();
            queue.Enqueue(root);
            do
            {
                var parent = queue.Dequeue();
                if (parent == null)
                    continue;
                yield return parent;
                childrenSelector(parent).ForEach(child => queue.Enqueue(child));
            } while (queue.Count > 0);
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            if (k < 0)
                throw new ArgumentOutOfRangeException(nameof(k));
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
        public static IEnumerable<IEnumerable<T>> CombinationsWithRepition<T>(this IEnumerable<T> elements, int k)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            if (k < 0)
                throw new ArgumentOutOfRangeException(nameof(k));
            return k == 0 ? new[] { new T[0] } : elements.SelectMany(e => elements.CombinationsWithRepition(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
        public static IEnumerable<T> Randomise<T>(this IEnumerable<T> elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            var rnd = new Random();
            return elements.OrderBy(e => rnd.Next());
        }



        public enum MinMaxConfliction : byte
        {
            TakeFirst = 0,
            TakeLast = 1
        }
    }
}