using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary_StructValue<TKey, TValue> : SpecificReadOnlyDictionaryBase<TKey, TValue>
        where TKey : IEquatable<TKey>, ICloneable, ISerializable
        where TValue : struct, IEquatable<TValue>
    {
        public SpecificReadOnlyDictionary_StructValue(ICollection<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary_StructValue(IEnumerable<Tuple<TKey, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary_StructValue(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected sealed override TKey CloneKey(TKey key) => key == null ? (TKey)(object)null : key.Clone<TKey>();
        protected override TValue CloneValue(TValue value) => value;
        public override object Clone() => new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(List<KeyValuePair<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(ReadOnlyCollection<KeyValuePair<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(Tuple<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(List<Tuple<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(ReadOnlyCollection<Tuple<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary_StructValue<TKey, TValue>(ReadOnlyDictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructValue<TKey, TValue>(dictionary);
    }
}