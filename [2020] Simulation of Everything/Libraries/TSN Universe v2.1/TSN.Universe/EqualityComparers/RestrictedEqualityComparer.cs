using System;

namespace TSN.Universe.EqualityComparers
{
    internal class RestrictedEqualityComparer<T> : MutableEqualityComparer<T>
    {
        static RestrictedEqualityComparer() => _default = new RestrictedEqualityComparer<T>();
        private RestrictedEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode, bool nullControl, bool throwIfEquals, bool throwIfNull)
            : base(equals, getHashCode)
        {
            _nullControl = nullControl;
            _throwIfEquals = throwIfEquals;
            _throwIfNull = throwIfNull;
        }
        public RestrictedEqualityComparer() : this(null, null, true, false, false) { }
        public RestrictedEqualityComparer(bool nullControl) : this(null, null, nullControl, false, false) { }
        public RestrictedEqualityComparer(Func<T, T, bool> equals) : this(equals, null, true, false, false) { }
        public RestrictedEqualityComparer(Func<T, T, bool> equals, bool nullControl) : this(equals, null, nullControl, false, false) { }
        public RestrictedEqualityComparer(Func<T, int> getHashCode) : this(null, getHashCode, true, false, false) { }
        public RestrictedEqualityComparer(Func<T, int> getHashCode, bool nullControl) : this(null, getHashCode, nullControl, false, false) { }
        public RestrictedEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode) : this(equals, getHashCode, true, false, false) { }
        public RestrictedEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode, bool nullControl) : this(equals, getHashCode, nullControl, false, false) { }
        public RestrictedEqualityComparer(Func<T, T, bool> equals, bool nullControl, bool throwIfEquals, bool throwIfNull) : this(equals, null, nullControl, throwIfEquals, throwIfNull) { }


        private static readonly RestrictedEqualityComparer<T> _default;

        private readonly bool _nullControl;
        private readonly bool _throwIfEquals;
        private readonly bool _throwIfNull;

        public static new RestrictedEqualityComparer<T> Default => _default;

        public bool NullControl => _nullControl;
        public bool ThrowIfEqueals => _throwIfEquals;
        public bool ThrowIfNull => _throwIfNull;



        public override bool Equals(T x, T y)
        {
            if (_throwIfNull)
            {
                if (x == null || y == null)
                    throw new InvalidOperationException();
            }
            else if (_nullControl)
            {
                if (x == null)
                    return y == null;
                if (y == null)
                    return false;
            }
            if (base.Equals(x, y))
            {
                if (_throwIfEquals)
                    throw new InvalidOperationException();
                return true;
            }
            return false;
        }
        public override int GetHashCode(T obj)
        {
            if (_throwIfNull)
            {
                if (obj == null)
                    throw new InvalidOperationException();
            }
            else if (_nullControl && obj == null)
                return 0;
            return _throwIfEquals ? 0 : base.GetHashCode(obj);
        }
    }
}