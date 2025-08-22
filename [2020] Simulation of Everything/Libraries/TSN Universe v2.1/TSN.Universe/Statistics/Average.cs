using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.Statistics
{
    [Serializable] public struct Average : IEquatable<Average>, IComparable<Average>, IComparable, IConvertible, IFormattable, ISerializable
    {
        static Average()
        {
            ToStringFormat = "N4";
        }
        internal Average(decimal sum, uint count)
        {
            //var fraction = GetFraction(sum, count);
            _sum = sum;
            _count = count;
            //_numerator = fraction.Numerator;
            //_denominator = fraction.Denominator;
            //_calculated = _denominator == 0 ? 0 : ((decimal)_numerator / _denominator);
            _calculated = _count == 0 ? 0 : (_sum / _count);
        }
        private Average(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var sum = info.GetDecimal(Field_Sum);
            var count = info.GetUInt32(Field_Count);
            //var fraction = GetFraction(sum, count);
            _sum = sum;
            _count = count;
            //_numerator = fraction.Numerator;
            //_denominator = fraction.Denominator;
            //_calculated = _denominator == 0 ? 0 : ((decimal)_numerator / _denominator);
            _calculated = _count == 0 ? 0 : (_sum / _count);
        }


        private const string Field_Sum = "Sum";
        private const string Field_Count = "Count";

        private readonly decimal _sum;
        private readonly uint _count;
        //[NonSerialized] private readonly uint _numerator;
        //[NonSerialized] private readonly uint _denominator;
        [NonSerialized] private readonly decimal _calculated;

        public static string ToStringFormat { get; set; }

        public bool IsEmpty => _sum == 0 && _count == 0;



        //private static IEnumerable<uint> GetPrimeNumbers(uint limit, bool descending = true)
        //{
        //    var l = (ulong)limit + 1;
        //    var factor = (int)(l / int.MaxValue);
        //    var remainder = (int)(l % int.MaxValue);
        //    var array = new BitArray[factor + (remainder > 0 ? 1 : 0)];
        //    for (uint i = 0; i < factor; i++)
        //        array[i] = new BitArray(int.MaxValue, true);
        //    if (remainder > 0)
        //        array[factor] = new BitArray(remainder, true);
        //    array[0][0] = false;
        //    array[0][1] = false;
        //    bool GetBit(uint index) => array[(int)(index / int.MaxValue)][(int)(index % int.MaxValue)];
        //    void SetBit(uint index, bool value) => array[(int)(index / int.MaxValue)][(int)(index % int.MaxValue)] = value;
        //    for (uint i = 2, sq = 0; (sq = i * i) <= limit; i++)
        //        if (GetBit(i))
        //            for (uint j = sq; j <= limit; j += i)
        //                SetBit(j, false);
        //    if (descending)
        //    {
        //        for (uint i = limit - 1; i >= 3; i--)
        //            if (GetBit(i))
        //                yield return i;
        //        yield return 2;
        //    }
        //    else
        //        for (uint i = 0; i < limit; i++)
        //            if (GetBit(i))
        //                yield return i;
        //}
        //private static (uint Numerator, uint Denominator) GetFraction(decimal sum, uint count)
        //{
        //    if (sum == 0)
        //        return (0, (uint)(count == 0 ? 0 : 1));
        //    if (count == 0)
        //        throw new DivideByZeroException();
        //    if (sum == count)
        //        return (1, 1);
        //    checked
        //    {
        //        decimal _sum = sum;
        //        ulong factor = 1;
        //        while (_sum != (ulong)_sum)
        //        {
        //            _sum *= 10;
        //            factor *= 10;
        //        }
        //        ulong _count = count * factor;
        //        if (_count % _sum == 0M)
        //            return (1, (uint)(_count / _sum));
        //        if (_sum % _count == 0M)
        //            return ((uint)(_sum / _count), 1);
        //        uint numerator = (uint)_sum, denominator = (uint)_count;
        //        bool notSimplified;
        //        do
        //        {
        //            notSimplified = true;
        //            foreach (var prime in GetPrimeNumbers(numerator < denominator ? numerator : denominator))
        //                if (numerator % prime == 0 && denominator % prime == 0)
        //                {
        //                    numerator /= prime;
        //                    denominator /= prime;
        //                    notSimplified = false;
        //                    break;
        //                }
        //        } while (!notSimplified);
        //        return (numerator, denominator);
        //    }
        //}

        public string ToString(string format) => _calculated.ToString(format);

        public override string ToString() => _calculated.ToString(ToStringFormat);
        public override int GetHashCode() => _calculated.GetHashCode();
        public override bool Equals(object obj) => obj is Average avg && Equals(avg);

        public bool Equals(Average other) => _sum.Equals(other._sum) && _count.Equals(other._count);
        public int CompareTo(Average other) => _count == other._count ? _sum.CompareTo(other._sum) : (_sum == other._sum ? other._count.CompareTo(_count) : ((ulong)_sum * other._count).CompareTo((ulong)other._sum * _count));/*_denominator == other._denominator ? _numerator.CompareTo(other._numerator) : (_numerator == other._numerator ? other._denominator.CompareTo(_denominator) : ((ulong)_numerator * other._denominator).CompareTo((ulong)other._numerator * _denominator));*/
        public int CompareTo(object obj) => obj is Average avg ? CompareTo(avg) : throw new ArgumentException("Argument was not of the type expected.");
        public TypeCode GetTypeCode() => _calculated.GetTypeCode();
        public string ToString(IFormatProvider provider) => _calculated.ToString(provider);
        public string ToString(string format, IFormatProvider formatProvider) => _calculated.ToString(format, formatProvider);

        bool IConvertible.ToBoolean(IFormatProvider provider) => ((IConvertible)_calculated).ToBoolean(provider);
        byte IConvertible.ToByte(IFormatProvider provider) => ((IConvertible)_calculated).ToByte(provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => ((IConvertible)_calculated).ToSByte(provider);
        char IConvertible.ToChar(IFormatProvider provider) => ((IConvertible)_calculated).ToChar(provider);
        short IConvertible.ToInt16(IFormatProvider provider) => ((IConvertible)_calculated).ToInt16(provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => ((IConvertible)_calculated).ToUInt16(provider);
        float IConvertible.ToSingle(IFormatProvider provider) => ((IConvertible)_calculated).ToSingle(provider);
        int IConvertible.ToInt32(IFormatProvider provider) => ((IConvertible)_calculated).ToInt32(provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => ((IConvertible)_calculated).ToUInt32(provider);
        double IConvertible.ToDouble(IFormatProvider provider) => ((IConvertible)_calculated).ToDouble(provider);
        long IConvertible.ToInt64(IFormatProvider provider) => ((IConvertible)_calculated).ToInt64(provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => ((IConvertible)_calculated).ToUInt64(provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => ((IConvertible)_calculated).ToDecimal(provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)_calculated).ToDateTime(provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => ((IConvertible)_calculated).ToType(conversionType, provider);
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(Field_Sum, _sum);
            info.AddValue(Field_Count, _count);
        }

        public static implicit operator Average((uint, uint) tuple) => new Average(tuple.Item1, tuple.Item2);
        public static implicit operator decimal(Average value) => value._calculated;
        public static explicit operator uint(Average value) => (uint)value._calculated;
        public static explicit operator float(Average value) => (float)value._calculated;
        public static explicit operator ulong(Average value) => (ulong)value._calculated;
        public static explicit operator long(Average value) => (long)value._calculated;
        public static explicit operator int(Average value) => (int)value._calculated;
        public static explicit operator ushort(Average value) => (ushort)value._calculated;
        public static explicit operator short(Average value) => (short)value._calculated;
        public static explicit operator char(Average value) => (char)value._calculated;
        public static explicit operator sbyte(Average value) => (sbyte)value._calculated;
        public static explicit operator byte(Average value) => (byte)value._calculated;
        public static explicit operator double(Average value) => (double)value._calculated;

        public static Average operator +(Average d1, Average d2) => new Average(d1._sum + d2._sum, d1._count + d2._count);
        public static Average operator -(Average d1, Average d2) => new Average(d1._sum - d2._sum, d1._count - d2._count);
        public static bool operator ==(Average d1, Average d2) => d1.CompareTo(d2) == 0;
        public static bool operator !=(Average d1, Average d2) => d1.CompareTo(d2) != 0;
        public static bool operator <(Average d1, Average d2) => d1.CompareTo(d2) < 0;
        public static bool operator >(Average d1, Average d2) => d1.CompareTo(d2) > 0;
        public static bool operator <=(Average d1, Average d2) => d1.CompareTo(d2) <= 0;
        public static bool operator >=(Average d1, Average d2) => d1.CompareTo(d2) >= 0;
    }
}