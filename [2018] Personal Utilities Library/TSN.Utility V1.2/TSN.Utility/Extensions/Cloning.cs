using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace TSN.Utility.Extensions
{
    public static class Cloning
    {
        public static T Clone<T>(this T obj)
            where T : ICloneable
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return (T)obj.Clone();
        }
        public static T BinaryClone<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            T result = default(T);
            var bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ms.Position = 0;
                result = (T)bf.Deserialize(ms);
                ms.Close();
            }
            return result;
        }
        public static bool TryForceToClone<T>(this T obj, out T cloned)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (obj is ICloneable cloneable)
            {
                cloned = (T)(cloneable.Clone());
                return true;
            }
            if (obj.GetType().IsSerializable)
            {
                cloned = obj.BinaryClone();
                return true;
            }
            cloned = default(T);
            return false;
        }
        public static IEnumerable<T> CloneSequence<T>(this IEnumerable<T> sequence)
            where T : ICloneable
        {
            return sequence?.Select(e => e == null ? (T)(object)null : e.Clone<T>()) ?? throw new ArgumentNullException(nameof(sequence));
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> CloneSequence<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> sequence)
            where TKey : ICloneable
            where TValue : ICloneable
        {
            return sequence?.Select(e => new KeyValuePair<TKey, TValue>(e.Key.Clone<TKey>(), e.Value.Clone<TValue>())) ?? throw new ArgumentNullException(nameof(sequence));
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> CloneSequence_StructKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> sequence)
            where TKey : struct
            where TValue : ICloneable
        {
            return sequence?.Select(e => new KeyValuePair<TKey, TValue>(e.Key.TryForceToClone(out TKey key) ? key : e.Key, e.Value.Clone<TValue>())) ?? throw new ArgumentNullException(nameof(sequence));
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> CloneSequence_StructValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> sequence)
            where TKey : ICloneable
            where TValue : struct
        {
            return sequence?.Select(e => new KeyValuePair<TKey, TValue>(e.Key.Clone<TKey>(), e.Value.TryForceToClone(out TValue value) ? value : e.Value)) ?? throw new ArgumentNullException(nameof(sequence));
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> CloneStructSequence<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> sequence)
            where TKey : struct
            where TValue : struct
        {
            return sequence?.Select(e => new KeyValuePair<TKey, TValue>(e.Key.TryForceToClone(out TKey key) ? key : e.Key, e.Value.TryForceToClone(out TValue value) ? value : e.Value)) ?? throw new ArgumentNullException(nameof(sequence));
        }
        public static IEnumerable<T> CloneStructSequence<T>(this IEnumerable<T> sequence)
            where T : struct
        {
            return sequence?.Select(e => e.TryForceToClone(out T item) ? item : e) ?? throw new ArgumentNullException(nameof(sequence));
        }
    }
}