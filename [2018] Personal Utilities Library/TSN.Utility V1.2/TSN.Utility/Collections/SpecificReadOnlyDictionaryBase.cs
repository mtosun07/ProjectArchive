using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Entities;
using TSN.Utility.EqualityComparers;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public abstract class SpecificReadOnlyDictionaryBase<TKey, TValue> : EntityBase<SpecificReadOnlyDictionaryBase<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
        where TKey: IEquatable<TKey>
        where TValue: IEquatable<TValue>
    {
        protected SpecificReadOnlyDictionaryBase(ICollection<KeyValuePair<TKey, TValue>> dictionary)
        {
            ValidateCollection(dictionary);
            _dictionary = new ReadOnlyDictionary<TKey, TValue>(dictionary.ToDictionary(kvp => CloneKey(kvp.Key), kvp => CloneValue(kvp.Value)));
        }
        protected SpecificReadOnlyDictionaryBase(IEnumerable<Tuple<TKey, TValue>> dictionary)
        {
            var dic = dictionary?.Select(t => new KeyValuePair<TKey, TValue>(t.Item1, t.Item2)).ToArray();
            ValidateCollection(dic);
            _dictionary = new ReadOnlyDictionary<TKey, TValue>(dic.ToDictionary(kvp => CloneKey(kvp.Key), kvp => CloneValue(kvp.Value)));
        }
        protected SpecificReadOnlyDictionaryBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var dictionary = info.GetCollectionValues<TKey, TValue>(DictionaryField);
            ValidateCollection(dictionary);
            _dictionary = new ReadOnlyDictionary<TKey, TValue>(_dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }



        private const string DictionaryField = "Dictionary";

        protected readonly ReadOnlyDictionary<TKey, TValue> _dictionary;

        public TValue this[TKey key] => _dictionary[key];
        public IEnumerable<TKey> Keys => _dictionary.Keys;
        public IEnumerable<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;



        protected virtual void ValidateCollection(ICollection<KeyValuePair<TKey, TValue>> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (collection.Any(e => e.Key == null))
                throw new ArgumentInnerValueNullException(nameof(collection), ArgumentTypes.Dictionary, "Keys");
            if (collection.Any(e => e.Value == null))
                throw new ArgumentInnerValueNullException(nameof(collection), ArgumentTypes.Dictionary, "Values");
        }

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        protected sealed override object[] GetHashCodesOf()
        {
            return DictionaryEqualityComparer<TKey, TValue>.DefaultByFNV.GetHashCode(_dictionary).MakeArray<object>();
        }
        protected sealed override bool EqualsMemberwise(SpecificReadOnlyDictionaryBase<TKey, TValue> other)
        {
            return DictionaryEqualityComparer<TKey, TValue>.Default.Equals(_dictionary, other._dictionary);
        }
        public sealed override string ToString()
        {
            return string.Join(Environment.NewLine, _dictionary.Select(kvp => $"[{kvp.Key}]: {kvp.Value}"));
        }
        public sealed override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddCollectionValues(DictionaryField, _dictionary);
        }

        protected abstract TKey CloneKey(TKey key);
        protected abstract TValue CloneValue(TValue value);
    }
}