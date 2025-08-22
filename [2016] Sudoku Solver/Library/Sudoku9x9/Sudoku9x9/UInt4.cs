using System;
using System.Collections;
using System.Linq;

namespace Sudoku9x9
{
    internal struct UInt4 : IEquatable<UInt4>, IComparable<UInt4>
    {
        public UInt4(int value)
        {
            if (value > MaxValue || value < MinValue)
                throw new ArgumentOutOfRangeException("value");
            _bits = ConvertToBitArray(value);
        }


        public const int MaxValue = 15;
        public const int MinValue = 0;

        private BitArray _bits;



        private static BitArray ConvertToBitArray(int value)
        {
            //var temp = BitConverter.GetBytes(value);
            //return new BitArray((BitConverter.IsLittleEndian ? temp.Reverse() : temp).Take(1).ToArray());

            return new BitArray(new BitArray(BitConverter.GetBytes((uint)value).Take(1).ToArray()).Cast<bool>().Take(4).ToArray());
        }
        public static int ConvertToInt32(UInt4 value)
        {
            int x = 0;
            for (int i = 0; i < value._bits.Length; i++)
                x += (value._bits[i] ? 1 : 0) * ((int)Math.Pow(2, i));
            return x;
        }

        public override string ToString()
        {
            return ((int)this).ToString();
        }
        public override int GetHashCode()
        {
            //var factors = new[] { 1, 11, 29, 31 };
            //int x = 0;
            //for (int i = 0; i < _bits.Length; i++)
            //    x += _bits[i].GetHashCode() * factors[i];
            //return x;
            return ((int)this).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (obj is IntSudoku)
                return ((IntSudoku)((int)this)).Equals((IntSudoku)obj);
            else if (obj is UInt4)
                return Equals((UInt4)obj);
            else if (obj is int)
                return ((int)this).Equals((int)obj);
            else
                return base.Equals(obj);
        }

        public bool Equals(UInt4 other)
        {
            return ((int)this).Equals((int)other);
        }
        public int CompareTo(UInt4 other)
        {
            return ((int)this).CompareTo(this);
        }

        public static implicit operator UInt4(int value)
        {
            return new UInt4(value);
        }
        public static implicit operator int(UInt4 value)
        {
            return ConvertToInt32(value);
        }
        public static bool operator ==(UInt4 left, UInt4 right)
        {
            int length = 0;
            if ((length = left._bits.Length) != right._bits.Length)
                return false;
            bool equals = true;
            for (int i = 0; i < length; i++)
                if (!(equals = left._bits[i] == right._bits[i]))
                    break;
            return equals;
        }
        public static bool operator !=(UInt4 left, UInt4 right)
        {
            return !(left == right);
        }
        public static bool operator <(UInt4 left, UInt4 right)
        {
            return (int)left < (int)right;
        }
        public static bool operator >(UInt4 left, UInt4 right)
        {
            return (int)left > (int)right;
        }
        public static int operator +(UInt4 left, UInt4 right)
        {
            return (int)left + (int)right;
        }
        public static int operator -(UInt4 left, UInt4 right)
        {
            return (int)left - (int)right;
        }
        public static int operator *(UInt4 left, UInt4 right)
        {
            return (int)left * (int)right;
        }
        public static int operator /(UInt4 left, UInt4 right)
        {
            return (int)left / (int)right;
        }
        public static int operator %(UInt4 left, UInt4 right)
        {
            return (int)left % (int)right;
        }
        public static UInt4 operator >>(UInt4 value, int x)
        {
            if (x > 0 && x <= 4)
            {
                var ui = new UInt4();
                for (int i = 0; i < x; i++)
                    ui = value.ShiftRightOnce();
                return ui;
            }
            var bits = new BitArray(4);
            if (x <= 0)
                bits = new BitArray(value._bits);
            else
                bits.SetAll(false);
            return new UInt4() { _bits = bits };
        }
        public static UInt4 operator <<(UInt4 value, int x)
        {
            if (x > 0 && x <= 4)
            {
                var ui = new UInt4();
                for (int i = 0; i < x; i++)
                    ui = value.ShiftLeftOnce();
                return ui;
            }
            var bits = new BitArray(4);
            if (x <= 0)
                bits = new BitArray(value._bits);
            else
                bits.SetAll(false);
            return new UInt4() { _bits = bits };
        }

        private UInt4 ShiftRightOnce()
        {
            var bits = new BitArray(4);
            bits[0] = false;
            for (int i = 1; i < 4; i++)
                bits[i] = _bits[i - 1];
            return new UInt4() { _bits = bits };
        }
        private UInt4 ShiftLeftOnce()
        {
            var bits = new BitArray(4);
            bits[3] = false;
            for (int i = 3; i > 0; i--)
                bits[i - 1] = _bits[i];
            return new UInt4() { _bits = bits };
        }

        public static UInt4 Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            UInt4 result = 1;
            if (!TryParse(s, out result))
                throw new FormatException();
            return result;
        }
        public static bool TryParse(string s, out UInt4 result)
        {
            result = 0;
            byte i = 0;
            if (string.IsNullOrEmpty(s) || !byte.TryParse(s, out i) || i < MinValue || i > MaxValue)
                return false;
            result = i;
            return true;
        }
    }
}