using System;
using System.Runtime.Serialization;

namespace Sudoku9x9
{
    [Serializable]
    public class LengthNotValidException : ArgumentOutOfRangeException
    {
        private const string _message = "Length did not equal to general fixed lenght of Sudoku.";

        public LengthNotValidException() : base(string.Empty, _message) { }
        public LengthNotValidException(string paramName) : base(paramName, _message) { }
        public LengthNotValidException(string paramName, string message) : base(paramName, message) { }
        public LengthNotValidException(string paramName, object actualValue, string message) : base(paramName, actualValue, message) { }
        public LengthNotValidException(string message, Exception inner) : base(message, inner) { }
        protected LengthNotValidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidSubsetException : ArgumentException
    {
        private const string _message = "Subset of Sudoku was not valid.";

        public InvalidSubsetException() : base(_message) { }
        public InvalidSubsetException(string paramName) : base(_message, paramName) { }
        public InvalidSubsetException(string message, string paramName) : base(message, paramName) { }
        public InvalidSubsetException(string message, Exception inner) : base(message, inner) { }
        public InvalidSubsetException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidSubsetException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidMatrixException : ArgumentException
    {
        private const string _message = "Matrix was not a valid sudoku.";

        public InvalidMatrixException() : base(_message) { }
        public InvalidMatrixException(string paramName) : base(_message, paramName) { }
        public InvalidMatrixException(string message, string paramName) : base(message, paramName) { }
        public InvalidMatrixException(string message, Exception inner) : base(message, inner) { }
        public InvalidMatrixException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidMatrixException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SolutionInfiniteException : Exception
    {
        private const string _message = "Count of solutions was almost infinite.";

        public SolutionInfiniteException() : base(_message) { }
        public SolutionInfiniteException(string message) : base(message) { }
        public SolutionInfiniteException(string message, Exception inner) : base(message, inner) { }
        protected SolutionInfiniteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}