using System;
using System.Runtime.Serialization;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class AlreadyDisposedException : InvalidOperationException
    {
        public AlreadyDisposedException()
            : base(ErrorMessageFormat) { }
        private AlreadyDisposedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string ErrorMessageFormat = "Relevant data have already been freed from memory.";
    }
}