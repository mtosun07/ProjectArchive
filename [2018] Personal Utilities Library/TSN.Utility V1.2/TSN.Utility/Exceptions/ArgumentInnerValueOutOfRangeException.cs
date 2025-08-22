using System;
using System.Runtime.Serialization;
using TSN.Utility.Extensions;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentInnerValueOutOfRangeException : ArgumentInnerValueException
    {
        public ArgumentInnerValueOutOfRangeException(string paramName, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, arg, nameOfInnerValue, Situation, q)
        {
            _actualValue = null;
            _range = null;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, Exception innerException, ArgumentTypes arg, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, arg, nameOfInnerValue, Situation, q)
        {
            _actualValue = null;
            _range = null;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, object actualValue, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, ArgumentTypes.NONE, nameOfInnerValue, Situation, q, GetExtensionString(actualValue))
        {
            _actualValue = actualValue;
            _range = null;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, Exception innerException, object actualValue, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, ArgumentTypes.NONE, nameOfInnerValue, Situation, q, GetExtensionString(actualValue))
        {
            _actualValue = actualValue;
            _range = null;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, object actualValue, string range, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, ArgumentTypes.NONE, nameOfInnerValue, ValPrmAndRetSituation(range), q, GetExtensionString(actualValue))
        {
            _actualValue = actualValue;
            _range = range;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, Exception innerException, object actualValue, string range, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, ArgumentTypes.NONE, nameOfInnerValue, ValPrmAndRetSituation(range), q, GetExtensionString(actualValue))
        {
            _actualValue = actualValue;
            _range = range;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, string range, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, ArgumentTypes.NONE, nameOfInnerValue, ValPrmAndRetSituation(range), q)
        {
            _actualValue = null;
            _range = range;
        }
        public ArgumentInnerValueOutOfRangeException(string paramName, Exception innerException, string range, string nameOfInnerValue = null, Quantity q = Quantity.Some)
            : base(paramName, innerException, ArgumentTypes.NONE, nameOfInnerValue, ValPrmAndRetSituation(range), q)
        {
            _actualValue = null;
            _range = range;
        }
        private ArgumentInnerValueOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _actualValue = info.GetValue(ActualValueField, typeof(object));
            _range = info.GetString(RangeField);
        }


        private const string ActualValueField = "ActualValue";
        private const string RangeField = "Range";
        private const string NullString = "((null))";
        private const string Situation = "out of range";
        private const string SituationFormat = "out of the range of {0}";
        private const string ErrorMessageExtensionFormat = " (Actual Value: {0})";

        private readonly object _actualValue;
        private readonly string _range;

        public object ActualValue
        {
            get { return _actualValue; }
        }
        public string Range
        {
            get { return _range.Clone<string>(); }
        }



        private static string ValPrmAndRetSituation(string range)
        {
            if (string.IsNullOrEmpty(range))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(range));
            return string.Format(SituationFormat, range);
        }
        private static string GetExtensionString(object actualValue)
        {
            return string.Format(ErrorMessageExtensionFormat, actualValue?.ToString() ?? NullString);
        }
        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue<object>(ActualValueField, _actualValue);
            info.AddValue<string>(RangeField, _range);
        }
    }
}