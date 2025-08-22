using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace TSN.NumberTextifier
{
    public abstract class NumberTextifier
    {
        public NumberTextifier(IReadOnlyDictionary<BigInteger, string> scaleDictionary)
        {
            _culture = CultureInfo.CurrentCulture;
            _scalesList = scaleDictionary == null ? new LinkedList<KeyValuePair<BigInteger, string>>() : new LinkedList<KeyValuePair<BigInteger, string>>(scaleDictionary.Where(x => x.Key >= BigIntegerConstants.Three).OrderBy(x => x.Key));
            _maxLength = BigIntegerConstants.Three + (_scalesList.Count == 0 ? BigInteger.Zero : _scalesList.Reverse().Take(2).Select(x => x.Key).Aggregate((a, b) => a + b));
        }
        public NumberTextifier(IReadOnlyDictionary<BigInteger, string> scaleDictionary, CultureInfo currentCulture)
            : this(scaleDictionary)
        {
            _culture = currentCulture;
        }


        private const string _wordsSeperator = " ";

        private readonly LinkedList<KeyValuePair<BigInteger, string>> _scalesList;
        private readonly BigInteger _maxLength;
        private readonly CultureInfo _culture;

        protected LinkedList<KeyValuePair<BigInteger, string>> ScalesList
        {
            get
            {
                return _scalesList;
            }
        }
        protected BigInteger MaxLength
        {
            get
            {
                return _maxLength;
            }
        }
        protected CultureInfo Culture
        {
            get
            {
                return _culture;
            }
        }
        protected virtual string WordsSeperator
        {
            get
            {
                return _wordsSeperator;
            }
        }



        protected abstract string ReadHundreds(BigInteger i);
        public abstract string ConvertToText(string input);
        protected virtual string ReadPositiveInteger(BigInteger i)
        {
            var words = new List<string>();
            BigInteger rem;
            i = BigInteger.DivRem(i, BigIntegerConstants.OneThousand, out rem);
            if (rem.Sign > 0)
                words.Add(ReadHundreds(rem));
            if (i.Sign > 0 && ScalesList.Count > 0)
            {
                BigInteger diff;
                for (var node = ScalesList.First; node != null; node = node.Next)
                    if (node.Next != null && (diff = node.Next.Value.Key - node.Value.Key) < int.MaxValue)
                    {

                        i = BigInteger.DivRem(i, BigInteger.Pow(BigIntegerConstants.Ten, (int)diff), out rem);
                        if (rem.Sign > 0)
                        {
                            words.Add(node.Value.Value);
                            words.Add(ReadPositiveInteger(rem));
                        }
                        if (i.Sign == 0)
                            break;
                    }
                    else if (i.Sign > 0)
                    {
                        words.Add(node.Value.Value);
                        words.Add(ReadPositiveInteger(i));
                    }
            }
            var sb = new StringBuilder();
            foreach (var word in words.Reverse<string>())
            {
                sb.Append(word);
                sb.Append(WordsSeperator);
            }
            return sb.ToString().TrimEnd();
        }

        protected bool IsNumber(string input, out bool isNegative, out BigInteger integerPart, out BigInteger decimalPartNumerator, out BigInteger decimalPartDenominator)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            var s = input.Trim().Replace(Culture.NumberFormat.NumberGroupSeparator, string.Empty);
            switch (Culture.NumberFormat.NumberNegativePattern)
            {
                case 0:
                    if (isNegative = s.StartsWith("(") && s.EndsWith(")"))
                        s = s.Substring(1, s.Length - 2).Trim();
                    break;
                case 1:
                case 2:
                    if (isNegative = s.StartsWith(Culture.NumberFormat.NegativeSign))
                        s = s.Substring(Culture.NumberFormat.NegativeSign.Length).TrimStart();
                    break;
                case 3:
                case 4:
                    if (isNegative = s.EndsWith(Culture.NumberFormat.NegativeSign))
                        s = s.Substring(0, s.Length - Culture.NumberFormat.NegativeSign.Length).TrimEnd();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            var parts = s.Split(new[] { Culture.NumberFormat.NumberDecimalSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && parts.All(x => x.All(y => char.IsDigit(y))))
            {
                integerPart = BigInteger.Parse(parts[0]);
                if (parts.Length == 2)
                {
                    decimalPartNumerator = BigInteger.Parse(parts[1]);
                    decimalPartDenominator = BigInteger.Pow(10, parts[1].Length);
                }
                else
                {
                    decimalPartNumerator = BigInteger.Zero;
                    decimalPartDenominator = BigInteger.One;
                }
                return true;
            }
            integerPart = BigInteger.Zero;
            decimalPartNumerator = BigInteger.Zero;
            decimalPartDenominator = BigInteger.One;
            return false;
        }

        public sealed override string ToString()
        {
            return base.ToString();
        }
        public sealed override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public sealed override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected static class BigIntegerConstants
        {
            static BigIntegerConstants()
            {
                _three = 3;
                _six = 6;
                _ten = 10;
                _twenty = 20;
                _hundred = 100;
                _oneThousand = 1000;
                _twoThousand = 2000;
                _expGoogolplex = new Lazy<BigInteger>(() => BigInteger.Pow(Ten, 100));
            }


            private readonly static BigInteger _three;
            private static readonly BigInteger _six;
            private readonly static BigInteger _ten;
            private static readonly BigInteger _twenty;
            private static readonly BigInteger _hundred;
            private static readonly BigInteger _oneThousand;
            private static readonly BigInteger _twoThousand;
            private static readonly Lazy<BigInteger> _expGoogolplex;

            public static BigInteger Three
            {
                get
                {
                    return _three;
                }
            }
            public static BigInteger Six
            {
                get
                {
                    return _six;
                }
            }
            public static BigInteger Ten
            {
                get
                {
                    return _ten;
                }
            }
            public static BigInteger Twenty
            {
                get
                {
                    return _twenty;
                }
            }
            public static BigInteger Hundred
            {
                get
                {
                    return _hundred;
                }
            }
            public static BigInteger OneThousand
            {
                get
                {
                    return _oneThousand;
                }
            }
            public static BigInteger TwoThousand
            {
                get
                {
                    return _twoThousand;
                }
            }
            public static BigInteger ExpGoogolplex
            {
                get
                {
                    return _expGoogolplex.Value;
                }
            }
        }
    }
}