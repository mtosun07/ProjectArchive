using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary_StringKey_StructValue<TValue> : SpecificReadOnlyDictionaryBase<string, TValue>
        where TValue : struct, IEquatable<TValue>
    {
        public SpecificReadOnlyDictionary_StringKey_StructValue(ICollection<KeyValuePair<string, TValue>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary_StringKey_StructValue(IEnumerable<Tuple<string, TValue>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary_StringKey_StructValue(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected sealed override string CloneKey(string key) => key.Clone<string>();
        protected override TValue CloneValue(TValue value) => value;
        public override object Clone() => new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(KeyValuePair<string, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(List<KeyValuePair<string, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(ReadOnlyCollection<KeyValuePair<string, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(Tuple<string, TValue>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(array);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(List<Tuple<string, TValue>> list) => list == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(list);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(ReadOnlyCollection<Tuple<string, TValue>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(Dictionary<string, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(ReadOnlyDictionary<string, TValue> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StringKey_StructValue<TValue>(dictionary);
    }
}