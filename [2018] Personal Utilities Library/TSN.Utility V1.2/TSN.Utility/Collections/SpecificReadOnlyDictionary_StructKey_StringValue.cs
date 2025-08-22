using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyDictionary_StructKey_StringValue<TKey> : SpecificReadOnlyDictionaryBase<TKey, string>
        where TKey : struct, IEquatable<TKey>
    {
        public SpecificReadOnlyDictionary_StructKey_StringValue(ICollection<KeyValuePair<TKey, string>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyDictionary_StructKey_StringValue(IEnumerable<Tuple<TKey, string>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyDictionary_StructKey_StringValue(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected override TKey CloneKey(TKey value) => value;
        protected sealed override string CloneValue(string key) => key.Clone<string>();
        public override object Clone() => new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(_dictionary);

        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(KeyValuePair<TKey, string>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(List<KeyValuePair<TKey, string>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(ReadOnlyCollection<KeyValuePair<TKey, string>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(Tuple<TKey, string>[] array) => array == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(array);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(List<Tuple<TKey, string>> list) => list == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(list);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(ReadOnlyCollection<Tuple<TKey, string>> collection) => collection == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(collection);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(Dictionary<TKey, string> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(dictionary);
        public static implicit operator SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(ReadOnlyDictionary<TKey, string> dictionary) => dictionary == null ? null : new SpecificReadOnlyDictionary_StructKey_StringValue<TKey>(dictionary);
    }
}