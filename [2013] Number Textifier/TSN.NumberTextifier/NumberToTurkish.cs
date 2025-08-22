using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace TSN.NumberTextifier
{
    public sealed class NumberToTurkish : NumberTextifier
    {
        static NumberToTurkish()
        {
            _dictionary = new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 0, "sıfır" }, { 1, "bir" }, { 2, "iki" }, { 3, "üç" }, { 4, "dört" }, { 5, "beş" }, { 6, "altı" }, { 7, "yedi" }, { 8, "sekiz" }, { 9, "dokuz" }, { 10, "on" }, { 20, "yirmi" }, { 30, "otuz" }, { 40, "kırk" }, { 50, "elli" }, { 60, "altmış" }, { 70, "yetmiş" }, { 80, "seksen" }, { 90, "doksan" }, { 100, "yüz" } });
            _shortScales = new ReadOnlyDictionary<BigInteger, string>(new Dictionary<BigInteger, string> { { BigIntegerConstants.Three, _thousandText }, { BigIntegerConstants.Six, "milyon" }, { 9, "milyar" }, { 12, "trilyon" }, { 15, "katrilyon" }, { 18, "kentilyon" }, { 21, "sekstilyon" }, { 24, "septilyon" }, { 27, "oktilyon" }, { 30, "nonilyon" }, { 33, "desilyon" }, { 36, "undesilyon" }, { 39, "dodesilyon" }, { 42, "tredesilyon" }, { 45, "katordesilyon" }, { 48, "kendesilyon" }, { 51, "seksdesilyon" }, { 54, "septendesilyon" }, { 57, "oktodesilyon" }, { 60, "novemdesilyon" }, { 63, "vigintilyon" }, { BigIntegerConstants.Hundred, "googol" }, { 303, "sentilyon" }, { BigIntegerConstants.ExpGoogolplex, "googolplex" } });
            _longScales = new ReadOnlyDictionary<BigInteger, string>(new Dictionary<BigInteger, string> { { BigIntegerConstants.Three, _thousandText }, { BigIntegerConstants.Six, "milyon" }, { 9, "milyar" }, { 12, "bilyar" }, { 18, "trilyar" }, { 24, "katrilyar" }, { 30, "kentilyar" }, { 36, "sekstilyar" }, { 42, "septilyar" }, { 48, "oktilyar" }, { 54, "nonilyar" }, { 60, "desilyar" }, { 66, "undesilyar" }, { 72, "dodesilyar" }, { 78, "tredesilyar" }, { 84, "katordesilyar" }, { 90, "kendesilyar" }, { 96, "seksdesilyar" }, { BigIntegerConstants.Hundred, "googol" }, { 102, "septendesilyar" }, { 108, "oktodesilyar" }, { 114, "novemdesilyar" }, { 120, "vigintilyar" }, { 600, "sentilyar" }, { BigIntegerConstants.ExpGoogolplex, "googolplex" } });
        }
        public NumberToTurkish()
            : base(_shortScales) { }
        public NumberToTurkish(CultureInfo currentCulture)
            : base(_shortScales, currentCulture) { }
        public NumberToTurkish(IReadOnlyDictionary<BigInteger, string> scaleDictionary)
            : base(scaleDictionary) { }
        public NumberToTurkish(IReadOnlyDictionary<BigInteger, string> scaleDictionary, CultureInfo currentCulture)
            : base(scaleDictionary, currentCulture) { }


        private const string _consonantsHard = "fstkçşhp";
        private const string _vowels = "aeıioöuü";
        private const string _vowelsBack = "aıou";
        private const string _thousandText = "bin";

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



        private static string GetDenominatorExtension(string word)
        {
            var lastVowel = word.Reverse().FirstOrDefault(x => _vowels.Contains(x));
            return $"{(_consonantsHard.Contains(word[word.Length - 1]) ? "t" : "d")}{(lastVowel == '\0' || _vowelsBack.Contains(lastVowel) ? "a" : "e")}";
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
            i = BigInteger.DivRem(i, BigIntegerConstants.Ten, out var rem);
            if (rem.Sign > 0)
                words.Add(GetOnes((int)rem));
            if (i.Sign > 0)
            {
                i = BigInteger.DivRem(i, BigIntegerConstants.Ten, out rem);
                if (rem.Sign > 0)
                    words.Add(GetTens((int)rem));
                if (i.Sign > 0)
                {
                    words.Add(_dictionary[100]);
                    if (i > BigInteger.One)
                        words.Add(GetOnes((int)i));
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
        protected sealed override string ReadPositiveInteger(BigInteger i)
        {
            if (i == BigIntegerConstants.OneThousand)
                return _thousandText;
            var result = base.ReadPositiveInteger(i);
            if (i > BigIntegerConstants.OneThousand && i < BigIntegerConstants.TwoThousand)
                return result.Substring(_thousandText.Length + WordsSeperator.Length);
            return result;
        }

        public sealed override string ConvertToText(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if ((input = input.Trim()).Equals(string.Empty))
                throw new ArgumentOutOfRangeException("input");
            if (input.Equals(Culture.NumberFormat.NaNSymbol))
                return "sayı değil";
            if (input.Equals(Culture.NumberFormat.PositiveInfinitySymbol))
                return "sonsuz";
            if (input.Equals(Culture.NumberFormat.NegativeInfinitySymbol))
                return "negatif sonsuz";
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
                result.Append("eksi ");
            result.Append(ReadPositiveInteger(integerPart));
            if (decNum.Sign > 0)
            {
                if (integerPart.Sign == 0)
                    result.Append(_dictionary[0]);
                result.Append(" tam ");
                var strDecDen = decDen.ToString("G", CultureInfo.InvariantCulture);
                string word;
                if (strDecDen.Length > MaxLength)
                {
                    var pow = strDecDen.Length - 1;
                    if (pow.ToString().Length > MaxLength)
                        return input;
                    word = ReadPositiveInteger(pow);
                    result.Append("on üzeri ");
                    result.Append(word);
                }
                else
                {
                    word = ReadPositiveInteger(decDen);
                    result.Append(word);
                }
                result.Append(GetDenominatorExtension(word));
                result.Append(WordsSeperator);
                result.Append(ReadPositiveInteger(decNum));
            }
            return result.ToString();
        }
    }
}