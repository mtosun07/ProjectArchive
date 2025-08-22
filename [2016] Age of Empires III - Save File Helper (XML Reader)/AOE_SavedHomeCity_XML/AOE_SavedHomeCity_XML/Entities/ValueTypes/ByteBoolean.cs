using System;

namespace AOE3_HomeCity.Entities.ValueTypes
{
    public struct ByteBoolean : IEquatable<ByteBoolean>
    {
        public ByteBoolean(byte value)
        {
            _value = value <= 0 ? (byte)0 : (byte)1;
        }
        public ByteBoolean(bool value)
        {
            _value = value ? (byte)1 : (byte)0;
        }


        private byte _value;

        public bool BooleanValue
        {
            get { return _value == 1; }
            set { _value = value ? (byte)1 : (byte)0; }
        }
        public byte ByteValue
        {
            get { return _value <= (byte)0 ? (byte)0 : (byte)1; }
            set { _value = value <= (byte)0 ? (byte)0 : (byte)1;  }
        }


        public static implicit operator ByteBoolean(bool value)
        {
            return new ByteBoolean(value);
        }
        public static implicit operator ByteBoolean(byte value)
        {
            return new ByteBoolean(value);
        }
        public static bool operator ==(ByteBoolean value1, ByteBoolean value2)
        {
            return value1.Equals(value2);
        }
        public static bool operator !=(ByteBoolean value1, ByteBoolean value2)
        {
            return !value1.Equals(value2);
        }
        public bool Equals(ByteBoolean other)
        {
            return BooleanValue.Equals(other.BooleanValue);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj");
            //return obj is bool ? Equals((bool)obj) : (obj is byte ? Equals((byte)obj) : (obj is ByteBoolean ? Equals((ByteBoolean)obj) : false));
            return GetHashCode().Equals(obj.GetHashCode());
        }
        public override int GetHashCode()
        {
            return BooleanValue.GetHashCode();
        }
        public override string ToString()
        {
            return ByteValue.ToString();
        }
    }
}