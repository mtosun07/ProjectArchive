using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary_StructKey<TKey, TValue> : SpecificReadOnlyDictionaryBase<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
        where TValue : IEquatable<TValue>, ICloneable, ISerializable
    {
        public SpecificReadOnlyDictionary_StructKey(ICollection<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary_StructKey(IEnumerable<Tuple<TKey, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary_StructKey(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected override TKey CloneKey(TKey key) => key;
        protected sealed override TValue CloneValue(TValue value) => value == null ? (TValue)(object)null : value.Clone<TValue>();
        public override object Clone() => new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(KeyValuePair<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(List<KeyValuePair<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(ReadOnlyCollection<KeyValuePair<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(Tuple<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(List<Tuple<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(ReadOnlyCollection<Tuple<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary_StructKey<TKey, TValue>(ReadOnlyDictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructKey<TKey, TValue>(dictionary);
    }
}