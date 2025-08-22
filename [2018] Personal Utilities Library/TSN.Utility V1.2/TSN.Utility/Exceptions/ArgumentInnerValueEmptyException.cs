using System;
using System.Runtime.Serialization;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentInnerValueEmptyException : ArgumentInnerValueException
    {
        public ArgumentInnerValueEmptyException(string paramName, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, arg, nameOfInnerValue, Situation, q) { }
        public ArgumentInnerValueEmptyException(string paramName, Exception innerException, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, arg, nameOfInnerValue, Situation, q) { }
        private ArgumentInnerValueEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string Situation = "empty";
    }
}