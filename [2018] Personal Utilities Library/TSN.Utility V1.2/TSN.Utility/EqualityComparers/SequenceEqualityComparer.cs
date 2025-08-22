using System;
using System.Collections;
using System.Collections.Generic;
using TSN.Hashing.FNV;

namespace TSN.Utility.EqualityComparers
{
    public sealed class SequenceEqualityComparer : IEqualityComparer<IEnumerable>
    {
        static SequenceEqualityComparer()
        {
            _default = new SequenceEqualityComparer(false);
            _defaultByFNV = new SequenceEqualityComparer(true);
        }
        private SequenceEqualityComparer(bool calculateHashCodeByFNV)
        {
            _collEqComp = calculateHashCodeByFNV ? CollectionEqualityComparer.DefaultByFNV : CollectionEqualityComparer.Default;
        }


        private static readonly SequenceEqualityComparer _default;
        private static readonly SequenceEqualityComparer _defaultByFNV;

        private readonly CollectionEqualityComparer _collEqComp;

        public static SequenceEqualityComparer Default
        {
            get { return _default; }
        }
        public static SequenceEqualityComparer DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(IEnumerable x, IEnumerable y)
        {
            if ((x == null) != (y == null))
                return false;
            if (x == null)
                return true;
            var enumX = x.GetEnumerator();
            var enumY = y.GetEnumerator();
            while (enumX.MoveNext())
            {
                if (!enumY.MoveNext())
                    return false;
                var currX = enumX.Current;
                var currY = enumY.Current;
                if ((currX == null) != (currY == null) || (currX != null && !currX.Equals(currY)))
                    return false;
            }
            return !enumY.MoveNext();
        }
        public int GetHashCode(IEnumerable obj)
        {
            return _collEqComp.GetHashCode(obj);
        }
    }
    public sealed class SequenceEqualityComparer<TElement> : IEqualityComparer<IEnumerable<TElement>>
    {
        static SequenceEqualityComparer()
        {
            _default = new SequenceEqualityComparer<TElement>(false);
            _defaultByFNV = new SequenceEqualityComparer<TElement>(true);
        }
        public SequenceEqualityComparer(bool calculateHashCodeByFNV, IEqualityComparer<TElement> comparer = null)
        {
            _calculateHashCodeByFNV = calculateHashCodeByFNV;
            _comparer = comparer;
            _equality = _comparer != null ? new Func<TElement, TElement, bool>((obj, other) => _comparer.Equals(obj, other)) : new Func<TElement, TElement, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _hashCode = _comparer != null ? new Func<TElement, int>(obj => _comparer.GetHashCode(obj)) : new Func<TElement, int>(obj => calculateHashCodeByFNV ? HashingFNV32.GetHashCodeFNV32(obj) : (obj?.GetHashCode() ?? 0));
            _collEqComp = new CollectionEqualityComparer<TElement>(calculateHashCodeByFNV, comparer);
        }
        public SequenceEqualityComparer(bool calculateHashCodeByFNV, Func<TElement, TElement, bool> equality, Func<TElement, int> hashCode)
        {
            _comparer = null;
            _equality = equality ?? new Func<TElement, TElement, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _hashCode = hashCode ?? new Func<TElement, int>(obj => calculateHashCodeByFNV ? HashingFNV32.GetHashCodeFNV32(obj) : (obj?.GetHashCode() ?? 0));
            _collEqComp = new CollectionEqualityComparer<TElement>(calculateHashCodeByFNV, equality, hashCode);
        }


        private static readonly SequenceEqualityComparer<TElement> _default;
        private static readonly SequenceEqualityComparer<TElement> _defaultByFNV;

        private readonly CollectionEqualityComparer<TElement> _collEqComp;
        private readonly bool _calculateHashCodeByFNV;
        private readonly IEqualityComparer<TElement> _comparer;
        private readonly Func<TElement, TElement, bool> _equality;
        private readonly Func<TElement, int> _hashCode;

        public static SequenceEqualityComparer<TElement> Default
        {
            get { return _default; }
        }
        public static SequenceEqualityComparer<TElement> DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(IEnumerable<TElement> x, IEnumerable<TElement> y)
        {
            if ((x == null) != (y == null))
                return false;
            if (x == null)
                return true;
            using (var enumX = x.GetEnumerator())
            using (var enumY = y.GetEnumerator())
            {
                while (enumX.MoveNext())
                {
                    if (!enumY.MoveNext())
                        return false;
                    var currX = enumX.Current;
                    var currY = enumY.Current;
                    if (!_equality(currX, currY))
                        return false;
                }
                return !enumY.MoveNext();
            }
        }
        public int GetHashCode(IEnumerable<TElement> obj)
        {
            return _collEqComp.GetHashCode(obj);
        }
    }
}