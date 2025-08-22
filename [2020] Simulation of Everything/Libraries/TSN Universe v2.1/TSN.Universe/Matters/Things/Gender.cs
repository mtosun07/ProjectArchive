using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Things
{
    [Serializable] public struct Gender : IEquatable<Gender>, IComparable<Gender>, IComparable, ISerializable
    {
        static Gender()
        {
            _neutral = new Gender(NeutralValue);
            _maxFeminine = new Gender(-MaxValue);
            _maxMasculine = new Gender(MaxValue);
        }
        internal Gender(double value)
        {
            if (value < MaxFeminine._value || value > MaxMasculine._value || double.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(_value));
            _value = value;
        }
        private Gender(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            _value = info.GetDouble(FieldValue);
        }


        private const double NeutralValue = 0D;
        private const double MaxValue = 1D;
        private const string FieldValue = "Value";
        
        private static readonly Gender _neutral;
        private static readonly Gender _maxFeminine;
        private static readonly Gender _maxMasculine;

        private readonly double _value;

        public static Gender Neutral => _neutral;
        public static Gender MaxFeminine => _maxFeminine;
        public static Gender MaxMasculine => _maxMasculine;


        public static bool CanReproduceWith(Gender g1, Gender g2, bool ignoreGender) => ignoreGender ? (!g1.Equals(_neutral) && !g2.Equals(_neutral)) : (g1._value * g2._value < 0D);
        public static bool IsMax(Gender g) => g == _neutral || g == _maxFeminine || g == _maxMasculine;
        public static bool IsFeminine(Gender g) => g._value < 0D;
        public static bool IsMasculine(Gender g) => g._value > 0D;
        public static double GetFemininityRate(Gender g) => g._value >= 0D ? 0D : -g._value;
        public static double GetMasculinityRate(Gender g) => g._value <= 0D ? 0D : g._value;
        public static double GetReproductivityRate(Gender g) => Math.Abs(g._value);
        public static double GetReproductivityRate(Gender g1, Gender g2, bool ignoreGender)
        {
            var prd = g1._value * g2._value;
            if (!ignoreGender && prd >= 0D)
                return 0D;
            return Math.Sqrt(-prd);
        }

        public override string ToString() => _value == 0D ? GENDER_NEUTRAL : (_value < 0D ? GENDER_FEMININE : (_value > 0D ? GENDER_MASCULINE : string.Empty));
        public override int GetHashCode() => _value.GetHashCode();
        public override bool Equals(object obj) => obj is Gender g && Equals(g);

        public bool Equals(Gender other) => _value.Equals(other._value);
        public int CompareTo(object obj) => obj is Gender g ? CompareTo(g) : throw new ArgumentException(ExceptionMessages.ARGUMENTTYPEINVALID, nameof(obj));
        public int CompareTo(Gender other) => _value.CompareTo(other._value);
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldValue, _value);
        }

        public static Gender operator +(Gender g) => new Gender(+g._value);
        public static Gender operator +(Gender g1, Gender g2)
        {
            var result = g1._value + g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator -(Gender g) => new Gender(-g._value);
        public static Gender operator -(Gender g1, Gender g2)
        {
            var result = g1._value - g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator ++(Gender g)
        {
            if (IsMax(g))
                return g;
            var sum = Math.Abs(g._value) + .01;
            return new Gender(Math.Sign(g._value) * (sum > MaxValue ? MaxValue : sum));
        }
        public static Gender operator --(Gender g)
        {
            if (IsMax(g))
                return g;
            var sum = Math.Abs(g._value) - .01;
            return new Gender(Math.Sign(g._value) * (sum > MaxValue ? MaxValue : sum));
        }
        public static Gender operator *(Gender g1, Gender g2)
        {
            var result = g1._value * g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator /(Gender g1, Gender g2)
        {
            var result = g1._value / g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator %(Gender g1, Gender g2)
        {
            var result = g1._value % g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator +(Gender g1, double g2)
        {
            var result = g1._value + g2;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator -(Gender g1, double g2)
        {
            var result = g1._value - g2;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator *(Gender g1, double g2)
        {
            var result = g1._value * g2;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator /(Gender g1, double g2)
        {
            var result = g1._value / g2;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator %(Gender g1, double g2)
        {
            var result = g1._value % g2;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator +(double g1, Gender g2)
        {
            var result = g1 + g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator -(double g1, Gender g2)
        {
            var result = g1 - g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator *(double g1, Gender g2)
        {
            var result = g1 * g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator /(double g1, Gender g2)
        {
            var result = g1 / g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static Gender operator %(double g1, Gender g2)
        {
            var result = g1 % g2._value;
            return result < _maxFeminine._value ? _maxFeminine : (result > _maxMasculine._value ? _maxMasculine : new Gender(result));
        }
        public static bool operator ==(Gender left, Gender right) => left._value == right._value;
        public static bool operator !=(Gender left, Gender right) => left._value != right._value;
        public static bool operator <(Gender left, Gender right) => left._value < right._value;
        public static bool operator >(Gender left, Gender right) => left._value > right._value;
        public static bool operator <=(Gender left, Gender right) => left._value <= right._value;
        public static bool operator >=(Gender left, Gender right) => left._value >= right._value;
    }
}