using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyStringDictionary : SpecificReadOnlyDictionaryBase<string, string>
    {
        public SpecificReadOnlyStringDictionary(ICollection<KeyValuePair<string, string>> dictionary)
            : base(dictionary) { }
        public SpecificReadOnlyStringDictionary(IEnumerable<Tuple<string, string>> dictionary)
            : base(dictionary) { }
        protected SpecificReadOnlyStringDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected sealed override string CloneKey(string key) => key.Clone<string>();
        protected sealed override string CloneValue(string value) => value.Clone<string>();
        public override object Clone() => new SpecificReadOnlyStringDictionary(_dictionary);

        public static implicit operator SpecificReadOnlyStringDictionary(KeyValuePair<string, string>[] array) => array == null ? null : new SpecificReadOnlyStringDictionary(array);
        public static implicit operator SpecificReadOnlyStringDictionary(List<KeyValuePair<string, string>> list) => list == null ? null : new SpecificReadOnlyStringDictionary(list);
        public static implicit operator SpecificReadOnlyStringDictionary(ReadOnlyCollection<KeyValuePair<string, string>> collection) => collection == null ? null : new SpecificReadOnlyStringDictionary(collection);
        public static implicit operator SpecificReadOnlyStringDictionary(Tuple<string, string>[] array) => array == null ? null : new SpecificReadOnlyStringDictionary(array);
        public static implicit operator SpecificReadOnlyStringDictionary(List<Tuple<string, string>> list) => list == null ? null : new SpecificReadOnlyStringDictionary(list);
        public static implicit operator SpecificReadOnlyStringDictionary(ReadOnlyCollection<Tuple<string, string>> collection) => collection == null ? null : new SpecificReadOnlyStringDictionary(collection);
        public static implicit operator SpecificReadOnlyStringDictionary(Dictionary<string, string> dictionary) => dictionary == null ? null : new SpecificReadOnlyStringDictionary(dictionary);
        public static implicit operator SpecificReadOnlyStringDictionary(ReadOnlyDictionary<string, string> dictionary) => dictionary == null ? null : new SpecificReadOnlyStringDictionary(dictionary);
        public static implicit operator SpecificReadOnlyStringDictionary(StringDictionary dictionary) => dictionary?.ToGenericEnumerable<DictionaryEntry>().Select(de => new KeyValuePair<string, string>((string)de.Key, (string)de.Value)).ToArray();
        public static implicit operator SpecificReadOnlyStringDictionary(NameValueCollection collection) => collection?.AllKeys.Select(key => new KeyValuePair<string, string>(key, collection[key])).ToArray();
    }
}