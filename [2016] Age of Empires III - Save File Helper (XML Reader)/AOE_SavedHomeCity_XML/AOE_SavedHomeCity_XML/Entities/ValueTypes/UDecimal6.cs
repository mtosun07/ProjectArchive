using System;
using System.Globalization;

namespace AOE3_HomeCity.Entities.ValueTypes
{
    public struct UDecimal6 : IEquatable<UDecimal6>
    {
        public UDecimal6(decimal value)
        {
            _value = value < 0 ? 0 : (value > 1 ? 1 : decimal.Round(value, 6));
        }


        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set { _value = value < 0 ? 0 : (value > 1 ? 1 : decimal.Round(value, 6)); }
        }



        public static implicit operator UDecimal6(decimal value)
        {
            return new UDecimal6(value);
        }
        public static bool operator ==(UDecimal6 value1, UDecimal6 value2)
        {
            return value1.Equals(value2);
        }
        public static bool operator !=(UDecimal6 value1, UDecimal6 value2)
        {
            return !value1.Equals(value2);
        }
        public bool Equals(UDecimal6 other)
        {
            return Value.Equals(other.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj");
            return GetHashCode().Equals(obj.GetHashCode());
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value.ToString("F6", CultureInfo.InvariantCulture);
        }
    }
}