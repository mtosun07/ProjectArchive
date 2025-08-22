using System;
using System.Runtime.Serialization;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentInnerValueNullOrEmptyException : ArgumentInnerValueException
    {
        public ArgumentInnerValueNullOrEmptyException(string paramName, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, arg, nameOfInnerValue, Situation, q) { }
        public ArgumentInnerValueNullOrEmptyException(string paramName, Exception innerException, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, arg, nameOfInnerValue, Situation, q) { }
        private ArgumentInnerValueNullOrEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string Situation = "either null or empty";
    }
}