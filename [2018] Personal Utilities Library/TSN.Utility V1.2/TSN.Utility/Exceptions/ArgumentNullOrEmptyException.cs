using System;
using System.Runtime.Serialization;
using TSN.Utility.Extensions;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentNullOrEmptyException : ArgumentException
    {
        public ArgumentNullOrEmptyException(ArgumentTypes arg, string paramName)
            : base(ValPrmAndRetMsg(arg), paramName) { }
        public ArgumentNullOrEmptyException(ArgumentTypes arg, string paramName, Exception innerException)
            : base(ValPrmAndRetMsg(arg), paramName, innerException) { }
        private ArgumentNullOrEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string ErrorMessageFormat = "Specified {0}parameter's value was either null or empty.";



        private static string ValPrmAndRetMsg(ArgumentTypes arg)
        {
            if (!arg.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(arg));
            return string.Format(ErrorMessageFormat, $"{arg.GetDescription()} ");
        }
    }
}