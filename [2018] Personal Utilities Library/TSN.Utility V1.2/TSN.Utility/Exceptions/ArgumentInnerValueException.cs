using System;
using System.Runtime.Serialization;
using TSN.Utility.Extensions;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public class ArgumentInnerValueException : ArgumentException
    {
        public ArgumentInnerValueException(string paramName, ArgumentTypes arg, string nameOfInnerValue = null, string situation = null, Quantity q = Quantity.Some, string extensionMessage = null)
            : base(ValPrmAndRetMsg(arg, nameOfInnerValue, situation, q, extensionMessage), paramName) { }
        public ArgumentInnerValueException(string paramName, Exception innerException, ArgumentTypes arg, string nameOfInnerValue = null, string situation = null, Quantity q = Quantity.Some, string extensionMessage = null)
            : base(ValPrmAndRetMsg(arg, nameOfInnerValue, situation, q, extensionMessage), paramName, innerException) { }
        protected ArgumentInnerValueException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        protected const string ErrorMessageFormat = "Specified {0}parameter's{1} {2} {3}.{4}";



        private static string ValPrmAndRetMsg(ArgumentTypes arg, string nameOfInnerValue = null, string situation = null, Quantity q = Quantity.Some, string extensionMessage = null)
        {
            if (!arg.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(arg));
            if (!q.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(q));
            string[] variants = new string[4];
            if (q == Quantity.Single)
            {
                variants[0] = string.Empty;
                variants[1] = string.Empty;
                variants[2] = "has";
                variants[3] = "was";
            }
            else
            {
                switch (q)
                {
                    case Quantity.Some:
                        variants[0] = " some";
                        break;
                    case Quantity.All:
                        variants[0] = " all";
                        break;
                    default:
                        variants[0] = string.Empty;
                        break;
                }
                variants[1] = "s";
                variants[2] = "have";
                variants[3] = "were";
            }
            return string.Format(ErrorMessageFormat,
                arg == ArgumentTypes.NONE ? string.Empty : $"{arg.GetDescription()} ",
                variants[0],
                string.IsNullOrEmpty(nameOfInnerValue) ? $"inner value{variants[1]}" : nameOfInnerValue,
                string.IsNullOrEmpty(situation) ? $"{variants[2]} been caused an Exception to be thrown." : $"{variants[3]} {situation}",
                extensionMessage == null ? string.Empty : $" {extensionMessage}");
        }
    }
}