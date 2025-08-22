using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;

namespace TSN.Utility.Exceptions
{
    [Serializable()]
    public sealed class ArgumentContradictionException : ArgumentException
    {
        public ArgumentContradictionException()
            : base(ErrorMessage) { }
        public ArgumentContradictionException(Exception innerException)
            : base(ErrorMessage, innerException) { }
        public ArgumentContradictionException(string paramName1, string case1, string paramName2, string case2, bool viceVersa, string reason = null)
            : base(ValPrmAndGetMsg(paramName1, case1, paramName2, case2, viceVersa, reason)) { }
        public ArgumentContradictionException(string paramName1, string case1, string paramName2, string case2, bool viceVersa, Exception innerException, string reason = null)
            : base(ValPrmAndGetMsg(paramName1, case1, paramName2, case2, viceVersa, reason), innerException) { }
        public ArgumentContradictionException(NameValueCollection casesByParamNames, string reason = null)
            : base(ValPrmAndGetMsg(casesByParamNames, reason)) { }
        public ArgumentContradictionException(Exception innerException, NameValueCollection casesByParamNames, string reason = null)
            : base(ValPrmAndGetMsg(casesByParamNames, reason), innerException) { }
        private ArgumentContradictionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        private const string ErrorMessage = "A contradiction occurred on values of some specified parameters.";
        private const string ErrorMessageFormat = "While parameter {0} was {1}, parameter {2} was {3}{4}.{5}";



        private static string ValPrmAndGetMsg(string paramName1, string case1, string paramName2, string case2, bool viceVersa, string reason)
        {
            if (string.IsNullOrEmpty(paramName1))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(paramName1));
            if (string.IsNullOrEmpty(case1))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(case1));
            if (string.IsNullOrEmpty(paramName2))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(paramName2));
            if (string.IsNullOrEmpty(case2))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(case2));
            return string.Format(ErrorMessageFormat, paramName1, case1, paramName2, case2, viceVersa ? "; or vice versa." : string.Empty, string.IsNullOrEmpty(reason) ? string.Empty : $" {reason}");
        }
        private static string ValPrmAndGetMsg(NameValueCollection casesByParamNames, string message)
        {
            if ((casesByParamNames?.Count ?? 0) == 0)
                throw new ArgumentNullOrEmptyException(ArgumentTypes.Dictionary, nameof(casesByParamNames));
            var dic = casesByParamNames.AllKeys.ToDictionary(x => x, x => casesByParamNames[x]);
            if (dic.Any(kvp => string.IsNullOrEmpty(kvp.Key) || string.IsNullOrEmpty(kvp.Value)))
                throw new ArgumentInnerValueNullOrEmptyException(nameof(casesByParamNames), ArgumentTypes.Dictionary, "Keys and/or Values");
            return $"{ErrorMessage}{(string.IsNullOrEmpty(message) ? string.Empty : $" Reason: {message}")}{Environment.NewLine}Relevant parameters' names and their values:{Environment.NewLine}{string.Join(", ", dic.Select(x => $"[{x.Key}: {x.Value}]"))}";
        }
    }
}