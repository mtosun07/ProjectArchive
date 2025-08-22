using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary<TKey, TValue> : SpecificReadOnlyDictionaryBase<TKey, TValue>
        where TKey: IEquatable<TKey>, ICloneable, ISerializable
        where TValue: IEquatable<TValue>, ICloneable, ISerializable
    {
        public SpecificReadOnlyDictionary(ICollection<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary(IEnumerable<Tuple<TKey, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected sealed override TKey CloneKey(TKey key) => key.Clone<TKey>();
        protected sealed override TValue CloneValue(TValue value) => value.Clone<TValue>();
        public override object Clone() => new SpecificReadOnlyDictionary<TKey, TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(KeyValuePair<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(List<KeyValuePair<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(ReadOnlyCollection<KeyValuePair<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(Tuple<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(List<Tuple<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(ReadOnlyCollection<Tuple<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary<TKey, TValue>(ReadOnlyDictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}