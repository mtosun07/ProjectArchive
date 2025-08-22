using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku9x9
{
    public struct IntSudoku : IEquatable<IntSudoku>, IComparable<IntSudoku>
    {
        public IntSudoku(int value)
        {
            if (value > MaxValue || value < MinValue)
                throw new ArgumentOutOfRangeException("value");
            _value = value;
        }


        public const int MaxValue = 9;
        public const int MinValue = 1;

        private UInt4 _value;

        public static IEnumerable<IntSudoku> ValidNumbers
        {
            get { return Enumerable.Range(MinValue, MaxValue - MinValue + 1).Cast<IntSudoku>(); }
        }



        public override string ToString()
        {
            return _value.ToString();
        }
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (obj is IntSudoku)
                return Equals((IntSudoku)obj);
            else if (obj is UInt4)
                return _value.Equals((UInt4)obj);
            else if (obj is int)
                return ((int)_value).Equals((int)obj);
            else
                return base.Equals(obj);
        }

        public bool Equals(IntSudoku other)
        {
            return ((int)this).Equals(other);
        }
        public int CompareTo(IntSudoku other)
        {
            return _value.CompareTo(other._value);
        }

        public static implicit operator int(IntSudoku value)
        {
            return value._value;
        }
        public static implicit operator IntSudoku(int value)
        {
            return new IntSudoku(value);
        }

        public static IntSudoku Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            IntSudoku result = 1;
            if (!TryParse(s, out result))
                throw new FormatException();
            return result;
        }
        public static bool TryParse(string s, out IntSudoku result)
        {
            result = 1;
            UInt4 i = 0;
            if (string.IsNullOrEmpty(s) || !UInt4.TryParse(s, out i) || i < MinValue || i > MaxValue)
                return false;
            result = (int)i;
            return true;
        }
    }
}