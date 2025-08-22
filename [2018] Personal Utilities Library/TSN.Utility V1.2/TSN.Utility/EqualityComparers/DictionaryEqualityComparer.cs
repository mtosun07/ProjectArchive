using System;
using System.Collections.Generic;
using TSN.Hashing.FNV;
using static TSN.Utility.Extensions.PrimeNumber;

namespace TSN.Utility.EqualityComparers
{
    public sealed class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IEnumerable<KeyValuePair<TKey, TValue>>>
    {
        static DictionaryEqualityComparer()
        {
            _default = new DictionaryEqualityComparer<TKey, TValue>(false);
            _defaultByFNV = new DictionaryEqualityComparer<TKey, TValue>(true);
        }
        public DictionaryEqualityComparer(bool calculateHashCodeByFNV, Func<TValue, TValue, bool> valueEquality = null, Func<KeyValuePair<TKey, TValue>, int> hashCodeToCalculate = null, IEqualityComparer<TKey> keyComparerForDefaultOfDictionary = null)
        {
            var _keyHashCode = keyComparerForDefaultOfDictionary != null ? new Func<TKey, int>(key => keyComparerForDefaultOfDictionary.GetHashCode(key)) : new Func<TKey, int>(key => key?.GetHashCode() ?? 0);
            var _valueEquality = valueEquality ?? new Func<TValue, TValue, bool>((value, other) => (value == null) == (other == null) && (value == null || value.Equals(other)));
            var _hashCodeToCalculate = hashCodeToCalculate ?? (calculateHashCodeByFNV ? new Func<KeyValuePair<TKey, TValue>, int>(kvp => new object[] { kvp.Key, kvp.Value }.GetHashCodeFNV32(kvp.GetType())) : new Func<KeyValuePair<TKey, TValue>, int>(kvp => TheFirstPrimes[7] * (kvp.Key?.GetHashCode() ?? 0) + TheFirstPrimes[11] * (kvp.Value?.GetHashCode() ?? 0)));
            _collEqComp = new CollectionEqualityComparer<KeyValuePair<TKey, TValue>>(calculateHashCodeByFNV,
                new Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>, bool>((o1, o2) => _keyHashCode(o1.Key) == _keyHashCode(o2.Key) && _valueEquality(o1.Value, o2.Value)),
                new Func<KeyValuePair<TKey, TValue>, int>(o => _hashCodeToCalculate(o)));
        }


        private static readonly DictionaryEqualityComparer<TKey, TValue> _default;
        private static readonly DictionaryEqualityComparer<TKey, TValue> _defaultByFNV;

        private readonly CollectionEqualityComparer<KeyValuePair<TKey, TValue>> _collEqComp;

        public static DictionaryEqualityComparer<TKey, TValue> Default
        {
            get { return _default; }
        }
        public static DictionaryEqualityComparer<TKey, TValue> DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(IEnumerable<KeyValuePair<TKey, TValue>> x, IEnumerable<KeyValuePair<TKey, TValue>> y)
        {
            return _collEqComp.Equals(x, y);
        }
        public int GetHashCode(IEnumerable<KeyValuePair<TKey, TValue>> obj)
        {
            return _collEqComp.GetHashCode(obj);
        }
    }
}