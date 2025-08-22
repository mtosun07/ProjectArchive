using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace TSN.AWebProject.Common.UtilitiesLibrary.Exceptions
{
    [Serializable]
    public class UnauthorizedOperationException : InvalidOperationException, ISerializable
    {
        public UnauthorizedOperationException(string message, IEnumerable<string> reasons, Exception innerException)
            : base(message ?? _message, innerException)
        {
            _reasons = new ReadOnlyCollection<string>(reasons == null ? Array.Empty<string>() : reasons.ToArray());
        }
        public UnauthorizedOperationException() : this(null, null, null) { }
        public UnauthorizedOperationException(string message) : this(message, null, null) { }
        public UnauthorizedOperationException(string message, IEnumerable<string> reasons) : this(message, reasons, null) { }
        public UnauthorizedOperationException(string message, Exception innerException) : this(message, null, innerException) { }
        public UnauthorizedOperationException(IEnumerable<string> reasons) : this(null, reasons, null) { }
        public UnauthorizedOperationException(IEnumerable<string> reasons, Exception innerException) : this(null, reasons, innerException) { }
        public UnauthorizedOperationException(Exception innerException) : this(null, null, innerException) { }
        protected UnauthorizedOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _reasons = new ReadOnlyCollection<string>(info.GetCollection<string>(nameof(_reasons))?.ToArray() ?? Array.Empty<string>());
        }


        private const string _message = "Operation was unauthorized.";

        private readonly IReadOnlyCollection<string> _reasons;

        public IReadOnlyCollection<string> Reasons => _reasons;


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddCollection(nameof(_reasons), _reasons.ToArray());
        }
    }
}