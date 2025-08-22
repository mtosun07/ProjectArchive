using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyStructDictionary<TKey, TValue> : SpecificReadOnlyDictionaryBase<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
        where TValue : struct, IEquatable<TValue>
    {
        public SpecificReadOnlyStructDictionary(ICollection<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyStructDictionary(IEnumerable<Tuple<TKey, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyStructDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected override TKey CloneKey(TKey key) => key;
        protected override TValue CloneValue(TValue value) => value;
        public override object Clone() => new SpecificReadOnlyStructDictionary<TKey, TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(KeyValuePair<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(List<KeyValuePair<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(ReadOnlyCollection<KeyValuePair<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(Tuple<TKey, TValue>[] array) => array == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(array);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(List<Tuple<TKey, TValue>> list) => list == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(list);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(ReadOnlyCollection<Tuple<TKey, TValue>> collection) => collection == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(collection);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(dictionary);
        public static implicit operator SpecificReadOnlyStructDictionary<TKey, TValue>(ReadOnlyDictionary<TKey, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyStructDictionary<TKey, TValue>(dictionary);
    }
}