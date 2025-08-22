using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace TSN.NumberTextifier
{
    public sealed class NumberToEnglish : NumberTextifier
    {
        static NumberToEnglish()
        {
            _dictionary = new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" }, { 6, "six" }, { 7, "seven" }, { 8, "eight" }, { 9, "nine" }, { 10, "ten" }, { 11, "eleven" }, { 12, "twelve" }, { 13, "thirteen" }, { 14, "fourteen" }, { 15, "fifteen" }, { 16, "sixteen" }, { 17, "seventeen" }, { 18, "eighteen" }, { 19, "nineteen" }, { 20, "twenty" }, { 30, "thirty" }, { 40, "forty" }, { 50, "fifty" }, { 60, "sixty" }, { 70, "seventy" }, { 80, "eighty" }, { 90, "ninety" }, { 100, "hundred" } });
            _shortScales = new ReadOnlyDictionary<BigInteger, string>(new Dictionary<BigInteger, string> { { BigIntegerConstants.Three, "thousand" }, { BigIntegerConstants.Six, "million" }, { 9, "billion" }, { 12, "trillion" }, { 15, "quadrillion" }, { 18, "quintillion" }, { 21, "sextillion" }, { 24, "septillion" }, { 27, "octillion" }, { 30, "nonillion" }, { 33, "decillion" }, { 36, "undecillion" }, { 39, "duodecillion" }, { 42, "tredecillion" }, { 45, "quattuordecillion" }, { 48, "quindecillion" }, { 51, "sexdecillion" }, { 54, "septendecillion" }, { 57, "octodecillion" }, { 60, "novemdecillion" }, { 63, "vigintillion" }, { BigIntegerConstants.Hundred, "googol" }, { 303, "centillion" }, { BigIntegerConstants.ExpGoogolplex, "googolplex" } });
            _longScales = new ReadOnlyDictionary<BigInteger, string>(new Dictionary<BigInteger, string> { { BigIntegerConstants.Three, "thousand" }, { BigIntegerConstants.Six, "million" }, { 9, "milliard" }, { 12, "billion" }, { 18, "trillion" }, { 24, "quadrillion" }, { 30, "quintillion" }, { 36, "sextillion" }, { 42, "septillion" }, { 48, "octillion" }, { 54, "nonillion" }, { 60, "decillion" }, { 66, "undecillion" }, { 72, "duodecillion" }, { 78, "tredecillion" }, { 84, "quattuordecillion" }, { 90, "quindecillion" }, { 96, "sexdecillion" }, { BigIntegerConstants.Hundred, "googol" }, { 102, "septendecillion" }, { 108, "octodecillion" }, { 114, "novemdecillion" }, { 120, "vigintillion" }, { 600, "centillion" }, { BigIntegerConstants.ExpGoogolplex, "googolplex" } });
        }
        public NumberToEnglish()
            : base(_shortScales) { }
        public NumberToEnglish(CultureInfo currentCulture)
            : base(_shortScales, currentCulture) { }
        public NumberToEnglish(IReadOnlyDictionary<BigInteger, string> scaleDictionary)
            : base(scaleDictionary) { }
        public NumberToEnglish(IReadOnlyDictionary<BigInteger, string> scaleDictionary, CultureInfo currentCulture)
            : base(scaleDictionary, currentCulture) { }


        private static readonly IReadOnlyDictionary<int, string> _dictionary;
        private static readonly IReadOnlyDictionary<BigInteger, string> _shortScales;
        private static readonly IReadOnlyDictionary<BigInteger, string> _longScales;

        public static IReadOnlyDictionary<BigInteger, string> ShortScales
        {
            get
            {
                return _shortScales;
            }
        }
        public static IReadOnlyDictionary<BigInteger, string> LongScales
        {
            get
            {
                return _longScales;
            }
        }



        private static string GetOnes(int i)
        {
            string x;
            return _dictionary.TryGetValue(i, out x) ? x : string.Empty;
        }
        private static string GetTens(int i)
        {
            string x;
            return _dictionary.TryGetValue(i * 10, out x) ? x : string.Empty;
        }

        protected sealed override string ReadHundreds(BigInteger i)
        {
            if (i.Sign == 0)
                return string.Empty;
            if (i >= BigIntegerConstants.OneThousand)
                throw new ArgumentOutOfRangeException("i");
            var words = new List<string>(3);
            BigInteger rem;
            i = BigInteger.DivRem(i, BigIntegerConstants.Hundred, out rem);
            if (i.Sign > 0)
            {
                words.Add(GetOnes((int)i));
                words.Add(_dictionary[100]);
                //if (rem.Sign > 0 && rem < ten)
                //    words.Add("and");
            }
            if (rem.Sign > 0)
            {
                if (rem < BigIntegerConstants.Twenty)
                    words.Add(GetOnes((int)rem));
                else
                {
                    i = BigInteger.DivRem(rem, BigIntegerConstants.Ten, out rem);
                    if (i.Sign > 0)
                        words.Add(GetTens((int)i));
                    if (rem.Sign > 0)
                        words.Add(GetOnes((int)rem));
                }
            }
            var sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(word);
                sb.Append(WordsSeperator);
            }
            return sb.ToString().TrimEnd();
        }
        protected sealed override string ReadPositiveInteger(BigInteger i)
        {
            return base.ReadPositiveInteger(i);
        }
        public sealed override string ConvertToText(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if ((input = input.Trim()).Equals(string.Empty))
                throw new ArgumentOutOfRangeException("input");
            if (input.Equals(Culture.NumberFormat.NaNSymbol))
                return "not a number";
            if (input.Equals(Culture.NumberFormat.PositiveInfinitySymbol))
                return "infinity";
            if (input.Equals(Culture.NumberFormat.NegativeInfinitySymbol))
                return "negative infinity";
            double o;
            if (double.TryParse(input, out o) && o == 0D)
                return _dictionary[0];
            bool isNegative;
            BigInteger integerPart, decNum, decDen;
            if (!IsNumber(input, out isNegative, out integerPart, out decNum, out decDen))
                throw new ArgumentOutOfRangeException("input");
            if (BigInteger.Max(integerPart, decNum).ToString("G", CultureInfo.InvariantCulture).Length > MaxLength)
                return input;
            var result = new StringBuilder();
            if (isNegative)
                result.Append("minus ");
            result.Append(ReadPositiveInteger(integerPart));
            if (decNum.Sign > 0)
            {
                result.Append(" and ");
                result.Append(ReadPositiveInteger(decNum));
                result.Append(",");
                result.Append(WordsSeperator);
                var strDecDen = decDen.ToString("G", CultureInfo.InvariantCulture);
                if (strDecDen.Length > MaxLength)
                {
                    var pow = strDecDen.Length - 1;
                    if (pow.ToString().Length > MaxLength)
                        return input;
                    result.Append("ten to the power ");
                    result.Append(ReadPositiveInteger(pow));
                }
                else
                {
                    result.Append(ReadPositiveInteger(decDen));
                    result.Append("th");
                    if (decNum > BigInteger.One)
                        result.Append("s");
                }
            }
            return result.ToString();
        }
    }
}