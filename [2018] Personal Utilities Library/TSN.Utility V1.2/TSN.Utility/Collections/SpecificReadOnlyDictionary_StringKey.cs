using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary_StringKey<TValue> : SpecificReadOnlyDictionaryBase<string, TValue>
        where TValue : IEquatable<TValue>, ICloneable, ISerializable
    {
        public SpecificReadOnlyDictionary_StringKey(ICollection<KeyValuePair<string, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary_StringKey(IEnumerable<Tuple<string, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary_StringKey(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected sealed override string CloneKey(string key) => key.Clone<string>();
        protected sealed override TValue CloneValue(TValue value) => value.Clone<TValue>();
        public override object Clone() => new SpecificReadOnlyDictionary_StringKey<TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(KeyValuePair<string, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(List<KeyValuePair<string, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(ReadOnlyCollection<KeyValuePair<string, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(Tuple<string, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(List<Tuple<string, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(ReadOnlyCollection<Tuple<string, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(Dictionary<string, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary_StringKey<TValue>(ReadOnlyDictionary<string, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StringKey<TValue>(dictionary);
    }
}