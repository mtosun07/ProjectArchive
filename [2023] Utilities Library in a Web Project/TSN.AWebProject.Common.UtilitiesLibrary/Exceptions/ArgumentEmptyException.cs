using System;
using System.Runtime.Serialization;

namespace TSN.AWebProject.Common.UtilitiesLibrary.Exceptions
{
    [Serializable]
    public class ArgumentEmptyException : ArgumentException, ISerializable
    {
        public ArgumentEmptyException() : base() { }
        public ArgumentEmptyException(string paramName) : base(FormatMessage(paramName), paramName) { }
        public ArgumentEmptyException(string paramName, Exception innerException) : base(FormatMessage(paramName), paramName, innerException) { }
        public ArgumentEmptyException(Exception innerException) : base(FormatMessage(), string.Empty, innerException) { }
        public ArgumentEmptyException(string message, string paramName) : base(message, paramName) { }
        public ArgumentEmptyException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) { }
        protected ArgumentEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }


        private const string _messageFormat = "Argument{0} was empty.";



        private static string FormatMessage(string paramName = null) => string.Format(_messageFormat, paramName.TryToTrimDeeper(out var s) != StringStates.Valued ? string.Empty : (StringExtensions.SpaceCharacter + s));
    }
}