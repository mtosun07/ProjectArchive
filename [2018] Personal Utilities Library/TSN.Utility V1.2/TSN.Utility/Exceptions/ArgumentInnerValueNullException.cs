using System;
using System.Runtime.Serialization;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentInnerValueNullException : ArgumentInnerValueException
    {
        public ArgumentInnerValueNullException(string paramName, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, arg, nameOfInnerValue, Situation, q) { }
        public ArgumentInnerValueNullException(string paramName, Exception innerException, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, arg, nameOfInnerValue, Situation, q) { }
        private ArgumentInnerValueNullException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string Situation = "null";
    }
}