using System;
using System.Collections;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Hashing.FNV;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.Entities.ObjectModels
{
    [Serializable()] [NativeHashable()]
    public struct Interval<T> : IEquatable<Interval<T>>, IStructuralEquatable, ISerializable, ICloneable
        where T : IEquatable<T>, IComparable<T>
    {
        public Interval(T minimum, T maximum)
        {
            ValidateParameters(minimum, maximum);
            _minimum = minimum.TryForceToClone(out T min) ? min : minimum;
            _maximum = maximum.TryForceToClone(out T max) ? max : maximum;
        }
        private Interval(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var minimum = info.GetValue<T>(MinimumField);
            var maximum = info.GetValue<T>(MaximumField);
            ValidateParameters(minimum, maximum);
            _minimum = minimum;
            _maximum = maximum;
        }


        private const string MinimumField = "Minimum";
        private const string MaximumField = "Maximum";

        private readonly T _minimum;
        private readonly T _maximum;

        public T Minimum
        {
            get { return _minimum; }
        }
        public T Maximum
        {
            get { return _maximum; }
        }



        private static void ValidateParameters(T minimum, T maximum)
        {
            if (minimum == null)
                throw new ArgumentNullException(nameof(minimum));
            if (maximum == null)
                throw new ArgumentNullException(nameof(maximum));
            if (minimum.CompareTo(maximum) > 0)
                throw new ArgumentException($"The parameter named '{nameof(minimum)}' was greater than the parameter named '{nameof(maximum)}'.");
        }

        public bool IsSubsetOf(Interval<T> other)
        {
            return _minimum.CompareTo(other._minimum) >= 0 && _maximum.CompareTo(other._maximum) <= 0;
        }
        public bool IsSupersetOf(Interval<T> other)
        {
            return _minimum.CompareTo(other._minimum) <= 0 && _maximum.CompareTo(other._maximum) >= 0;
        }
        public bool Contains(T value)
        {
            return _minimum.CompareTo(value) <= 0 && _maximum.CompareTo(value) >= 0;
        }

        public override string ToString()
        {
            return $"I = {{ x is of '{typeof(T).Name}' | {_minimum} <= x <= {_maximum} }}";
        }
        public override int GetHashCode()
        {
            return new object[] { _minimum, _maximum }.GetHashCodeFNV32(GetType());
        }
        public int GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            return comparer.GetHashCode(this);
        }
        public override bool Equals(object obj)
        {
            return obj != null && obj is Interval<T> && Equals((Interval<T>)obj);
        }
        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            return comparer.Equals(this, other);
        }
        public bool Equals(Interval<T> other)
        {
            return _minimum.Equals(other._minimum) && _maximum.Equals(other._maximum);
        }
        public object Clone()
        {
            return new Interval<T>(_minimum, _maximum);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(MinimumField, _minimum);
            info.AddValue(MaximumField, _maximum);
        }

        public static implicit operator Interval<T>(T obj)
        {
            return new Interval<T>(obj, obj);
        }
        public static implicit operator Interval<T>(Tuple<T, T> obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            if (obj.Item1 == null || obj.Item2 == null)
                throw new ArgumentInnerValueNullException(nameof(obj), ArgumentTypes.NONE);
            T min = obj.Item1.CompareTo(obj.Item2) <= 0 ? obj.Item1 : obj.Item2;
            T max = obj.Item1.CompareTo(obj.Item2) >= 0 ? obj.Item2 : obj.Item1;
            return new Interval<T>(min, max);
        }
        public static bool operator ==(Interval<T> x, Interval<T> y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(Interval<T> x, Interval<T> y)
        {
            return !x.Equals(y);
        }
        public static bool operator <(T left, Interval<T> right)
        {
            return left.CompareTo(right._minimum) < 0;
        }
        public static bool operator <(Interval<T> left, T right)
        {
            return left._maximum.CompareTo(right) < 0;
        }
        public static bool operator >(T left, Interval<T> right)
        {
            return left.CompareTo(right._maximum) > 0;
        }
        public static bool operator >(Interval<T> left, T right)
        {
            return left._minimum.CompareTo(right) > 0;
        }
    }
}