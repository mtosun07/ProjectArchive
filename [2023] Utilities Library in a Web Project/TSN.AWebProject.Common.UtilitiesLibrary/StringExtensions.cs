using TSN.AWebProject.Common.UtilitiesLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TSN.AWebProject.Common.UtilitiesLibrary
{
    public static class StringExtensions
    {
        static StringExtensions()
        {
            _regexFalse = new Regex("^(false|f|no|0)$");
            _regexTrue = new Regex("^(true|t|yes|1)$");
            _regexWhiteSpace = new Regex(@"\s");
            _regexEmailAddress = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            _regexIpAddress = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        }


        private const string _randomStringCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string SpaceCharacter = " ";
        public const string SpaceCharacterHtml = "%20";
        
        private static readonly Regex _regexTrue;
        private static readonly Regex _regexFalse;
        private static readonly Regex _regexWhiteSpace;
        private static readonly Regex _regexIpAddress;
        private static readonly Regex _regexEmailAddress;



        public static string GenerateRandomString(int length)
        {
            if (length < 0)
                return null;
            if (length == 0)
                return string.Empty;
            var str = string.Empty;
            for (int i = 0; i < length; i++)
                str += _randomStringCharacters[Shared.Random.Next(_randomStringCharacters.Length)];
            return str;
        }
        public static string GenerateRandomString(int minLengthInclusive, int maxLengthInclusive) => GenerateRandomString(Shared.Random.Next(minLengthInclusive, maxLengthInclusive + 1));
        public static string GenerateRandomPassword(int minLengthInclusive, int maxLengthInclusive)
        {
            if (minLengthInclusive < 4)
                throw new ArgumentOutOfRangeException(nameof(minLengthInclusive));
            const string alphaLower = "abcdefghijklmnopqrstuvwxyz"; // 45%
            const string alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // 25%
            const string numeric = "0123456789";                    // 20%
            const string special = @".+#$%&/*-,\!=@_|<>";           // 10%
            double length = Shared.Random.Next(minLengthInclusive, maxLengthInclusive + 1);
            var a = alphaLower.Shuffle().Take((int)Math.Ceiling(length * 45D / 100D)).ToArray();
            var b = alphaUpper.Shuffle().Take((int)Math.Ceiling(length * 25D / 100D)).ToArray();
            var c = numeric.Shuffle().Take((int)Math.Ceiling(length * 20D / 100D)).ToArray();
            var d = special.Shuffle().Take((int)Math.Ceiling(length * 10D / 100D)).ToArray();
            var password = new StringBuilder();
            password.Append(a[0]);
            password.Append(b[0]);
            password.Append(c[0]);
            password.Append(d[0]);
            if (length > 4)
                password.Append(a.Skip(1).Union(b.Skip(1)).Union(c.Skip(1)).Union(d.Skip(1)).ToArray().ShuffleList().Take((int)length - 4));
            return password.ToString().Shuffle();
        }

        public static bool IsEmptyWhiteSpace(this string s) => s.Equals(_regexWhiteSpace.Replace(s, string.Empty));
        public static string Capitalize(this string s, CultureInfo culture) => culture == null ? throw new ArgumentNullException(nameof(culture)) : s == null ? null : string.Join(SpaceCharacter, _regexWhiteSpace.Split(s.Trim()).Select(x => x.Trim()).Where(x => !x.Equals(string.Empty)).Select(x => char.ToUpper(x[0], culture) + (x.Length == 1 ? string.Empty : x.Substring(1).ToLower(culture))));
        public static string CapitalizeInvariant(this string s) => Capitalize(s, CultureInfo.InvariantCulture);
        public static string Capitalize(this string s) => Capitalize(s, CultureInfo.CurrentCulture);
        public static string TrimDeeper(this string s) => s == null ? null : string.Join(SpaceCharacter, _regexWhiteSpace.Split(s.Trim()).Where(x => !x.Equals(string.Empty)));
        public static string TrimEvenIfNull(this string s) => s?.Trim() ?? string.Empty;
        public static string TrimDeeperEvenIfNull(this string s) => s?.TrimDeeper() ?? string.Empty;
        public static StringStates TryToTrim(this string s, out string trimmed)
        {
            if (s == null)
            {
                trimmed = null;
                return StringStates.Null;
            }
            return (trimmed = s.Trim()).Equals(string.Empty) ? StringStates.Empty : StringStates.Valued;
        }
        public static StringStates TryToTrimDeeper(this string s, out string trimmed)
        {
            if (s == null)
            {
                trimmed = null;
                return StringStates.Null;
            }
            return (trimmed = s.TrimDeeper()).Equals(string.Empty) ? StringStates.Empty : StringStates.Valued;
        }
        public static StringStates GetState(this string s, bool considerWhiteSpaces) => s == null ? StringStates.Null : ((considerWhiteSpaces ? s.Trim() : s).Equals(string.Empty) ? StringStates.Empty : StringStates.Valued);
        public static void ThrowExceptionByState(this StringStates state, string argumentName = null)
        {
            switch (state)
            {
                case StringStates.Null:
                    throw new ArgumentNullException(argumentName);
                case StringStates.Empty:
                    throw new ArgumentEmptyException(argumentName);
                default:
                    break;
            }
        }
        public static string RemoveDiacritics(this string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            var srcEncoding = Encoding.UTF8;
            var destEncoding = Encoding.GetEncoding(1252);
            var normalizedString = destEncoding.GetString(Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(s))).Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
                if (!CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]).Equals(UnicodeCategory.NonSpacingMark))
                    result.Append(normalizedString[i]);
            return result.ToString();
        }
        public static string ToUpperLatin(this string s) => s.ToUpperInvariant().RemoveDiacritics();
        public static string ToLowerLatin(this string s) => s.ToLowerInvariant().RemoveDiacritics();
        public static string ToCulture(this string s, CultureInfo culture) => culture == null ? throw new ArgumentNullException(nameof(culture)) : (s == null ? null : new string(s.Select(c => char.IsLetter(c) ? (char.IsLower(c) ? char.ToLower(c, culture) : char.ToUpper(c, culture)) : c).ToArray()));

        public static string Shuffle(this string s) => new string(s.ToCharArray().ShuffleList().ToArray());
        public static string Shuffle(this string s, out IList<int> indices) => new string(s.ToCharArray().ShuffleList(out indices).ToArray());

        public static bool IsValidIpAddressV4(this string ipAddress)
        {
#if DEBUG
            return true;
#endif
            return _regexIpAddress.IsMatch(ipAddress);
        }
        public static bool IsValidEmailAddress(this string emailAddress) => _regexEmailAddress.IsMatch(emailAddress);

        public static bool TryParseToBool(this string s, out bool value)
        {
            var str = TrimEvenIfNull(s).ToLowerInvariant();
            return (value = _regexTrue.IsMatch(str)) || _regexFalse.IsMatch(str);
        }
    }
}