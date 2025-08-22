using System;
using System.Collections;
using System.Collections.Generic;

namespace TSN.Universe.EqualityComparers
{
    internal class MutableEqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
    {
        static MutableEqualityComparer()
        {
            _equalsDefault = (x, y) => x.Equals(y);
            _getHashCodeDefault = obj => obj.GetHashCode();
            _default = new MutableEqualityComparer<T>();
        }
        public MutableEqualityComparer() : this(_equalsDefault, _getHashCodeDefault) { }
        public MutableEqualityComparer(Func<T, T, bool> equals) : this(equals, _getHashCodeDefault) { }
        public MutableEqualityComparer(Func<T, int> getHashCode) : this(_equalsDefault, getHashCode) { }
        public MutableEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            _equals = equals ?? _equalsDefault;
            _getHashCode = getHashCode ?? _getHashCodeDefault;
        }


        private static readonly MutableEqualityComparer<T> _default;
        private static readonly Func<T, T, bool> _equalsDefault;
        private static readonly Func<T, int> _getHashCodeDefault;


        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashCode;

        public static MutableEqualityComparer<T> Default => _default;



        public virtual bool Equals(T x, T y) => _equals(x, y);
        public virtual int GetHashCode(T obj) => _getHashCode(obj);
        bool IEqualityComparer.Equals(object x, object y) => Equals((T)x, (T)y);
        int IEqualityComparer.GetHashCode(object obj) => GetHashCode((T)obj);
    }
}