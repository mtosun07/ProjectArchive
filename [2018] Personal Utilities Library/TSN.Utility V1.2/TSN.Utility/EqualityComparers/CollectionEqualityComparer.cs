using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSN.Hashing.FNV;
using TSN.Utility.Extensions;
using static TSN.Utility.Extensions.PrimeNumber;

namespace TSN.Utility.EqualityComparers
{
    public sealed class CollectionEqualityComparer : IEqualityComparer<IEnumerable>
    {
        static CollectionEqualityComparer()
        {
            _default = new CollectionEqualityComparer(false);
            _defaultByFNV = new CollectionEqualityComparer(true);
        }
        private CollectionEqualityComparer(bool calculateHashCodeByFNV)
        {
            _calculateHashCodeByFNV = calculateHashCodeByFNV;
            _genericCollEqComp = calculateHashCodeByFNV ? CollectionEqualityComparer<object>.DefaultByFNV : CollectionEqualityComparer<object>.Default;
        }


        private static readonly CollectionEqualityComparer _default;
        private static readonly CollectionEqualityComparer _defaultByFNV;

        private readonly bool _calculateHashCodeByFNV;
        private readonly CollectionEqualityComparer<object> _genericCollEqComp;

        public static CollectionEqualityComparer Default
        {
            get { return _default; }
        }
        public static CollectionEqualityComparer DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(IEnumerable x, IEnumerable y)
        {
            return _genericCollEqComp.Equals(x?.ToGenericEnumerable<object>(), y?.ToGenericEnumerable<object>());
        }
        public int GetHashCode(IEnumerable obj)
        {
            return _genericCollEqComp.GetHashCode(obj?.ToGenericEnumerable<object>());
        }
    }
    public sealed class CollectionEqualityComparer<TElement> : IEqualityComparer<IEnumerable<TElement>>
    {
        static CollectionEqualityComparer()
        {
            _default = new CollectionEqualityComparer<TElement>(false);
            _defaultByFNV = new CollectionEqualityComparer<TElement>(true);
        }
        public CollectionEqualityComparer(bool calculateHashCodeByFNV, IEqualityComparer<TElement> comparer = null)
        {
            _calculateHashCodeByFNV = calculateHashCodeByFNV;
            _comparer = comparer;
            _equality = _comparer != null ? new Func<TElement, TElement, bool>((obj, other) => _comparer.Equals(obj, other)) : new Func<TElement, TElement, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _hashCode = _comparer != null ? new Func<TElement, int>(obj => _comparer.GetHashCode(obj)) : new Func<TElement, int>(obj => calculateHashCodeByFNV ? HashingFNV32.GetHashCodeFNV32(obj) : (obj?.GetHashCode() ?? 0));
        }
        public CollectionEqualityComparer(bool calculateHashCodeByFNV, Func<TElement, TElement, bool> equality, Func<TElement, int> hashCode)
        {
            _comparer = null;
            _equality = equality ?? new Func<TElement, TElement, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _hashCode = hashCode ?? new Func<TElement, int>(obj => calculateHashCodeByFNV ? HashingFNV32.GetHashCodeFNV32(obj) : (obj?.GetHashCode() ?? 0));
        }


        private static readonly CollectionEqualityComparer<TElement> _default;
        private static readonly CollectionEqualityComparer<TElement> _defaultByFNV;

        private readonly bool _calculateHashCodeByFNV;
        private readonly IEqualityComparer<TElement> _comparer;
        private readonly Func<TElement, TElement, bool> _equality;
        private readonly Func<TElement, int> _hashCode;

        public static CollectionEqualityComparer<TElement> Default
        {
            get { return _default; }
        }
        public static CollectionEqualityComparer<TElement> DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(IEnumerable<TElement> x, IEnumerable<TElement> y)
        {
            if ((x == null) != (y == null))
                return false;
            if (x == null)
                return true;
            var arrX = x.ToArray();
            var arrY = y.ToArray();
            if (arrX.Length != arrY.Length)
                return false;
            var yFounds = new BitArray(arrX.Length, false);
            bool found;
            for (int i = 0; i < arrX.Length; i++)
            {
                found = false;
                object currX = arrX[i];
                for (int j = 0; j < arrY.Length; j++)
                {
                    if (yFounds[j])
                        continue;
                    object currY = arrY[j];
                    if ((currX == null) == (currY == null) && (currX == null || currX.Equals(currY)))
                    {
                        found = true;
                        yFounds.Set(j, true);
                        break;
                    }
                }
                if (!found)
                    return false;
            }
            return true;
        }
        public int GetHashCode(IEnumerable<TElement> obj)
        {
            if (_calculateHashCodeByFNV)
                return HashingFNV32.GetHashCodeFNV32(obj);
            if (obj == null)
                return 0;
            var hashes = obj.Select(o => _hashCode(o)).ToArray();
            return GeneratePrimesSieveOfEratosthenes(hashes.Length + 1).Skip(1).Select((p, i) => p * hashes[i]).Sum();
        }
    }
}