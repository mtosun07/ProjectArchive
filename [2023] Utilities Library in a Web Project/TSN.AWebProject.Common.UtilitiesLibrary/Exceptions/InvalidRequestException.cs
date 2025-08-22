using System;
using System.Runtime.Serialization;

namespace TSN.AWebProject.Common.UtilitiesLibrary.Exceptions
{
    [Serializable]
    public class InvalidRequestException : InvalidOperationException, ISerializable
    {
        public InvalidRequestException(string message, object requestInfo, Exception innerException)
            : base(message ?? _message, innerException)
        {
            _requestInfo = requestInfo;
        }
        public InvalidRequestException() : this(null, null, null) { }
        public InvalidRequestException(string message) : this(message, null, null) { }
        public InvalidRequestException(string message, object requestInfo) : this(message, requestInfo, null) { }
        public InvalidRequestException(string message, Exception innerException) : this(message, null, innerException) { }
        public InvalidRequestException(object requestInfo) : this(null, requestInfo, null) { }
        public InvalidRequestException(object requestInfo, Exception innerException) : this(null, requestInfo, innerException) { }
        public InvalidRequestException(Exception innerException) : this(null, null, innerException) { }
        protected InvalidRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _requestInfo = info.GetValue(nameof(_requestInfo), typeof(object));
        }


        private const string _message = "Request was invalid to operate.";

        private readonly object _requestInfo;

        public object RequestInfo => _requestInfo;



        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue<object>(nameof(_requestInfo), _requestInfo);
        }
    }
}