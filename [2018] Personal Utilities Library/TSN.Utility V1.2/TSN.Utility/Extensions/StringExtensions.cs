using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace TSN.Utility.Extensions
{
    public static class StringExtensions
    {
        #region . Phone Numbers .
        private static string[] PhoneCodesTR = new[] { "+90", "0090" };
        private static string[] PhoneNumbersTR = new[]
        {
            "392", "510", "512", "522", "562", "564", "592", "594", "800", "811", "822", "850", "888", "898", "900",
            "501", "502", "503", "504", "505", "506", "507", "508", "509", "551", "552", "553", "554", "555", "556", "557", "558", "559",
            "530", "531", "532", "533", "534", "535", "536", "537", "538", "539", "561",
            "540", "541", "542", "543", "544", "545", "546", "547", "548", "549",
            "212", "213", "216", "217", "222", "223", "224", "225", "226", "227", "228", "229", "232", "233", "236", "237", "242", "243", "246", "247",
            "248", "249", "252", "253", "256", "257", "258", "259", "262", "263", "264", "265", "266", "267", "272", "273", "274", "275", "276", "277",
            "282", "283", "284", "285", "286", "287", "288", "289", "312", "313", "318", "319", "322", "323", "324", "325", "326", "327", "328", "329",
            "332", "333", "338", "339", "342", "343", "344", "345", "346", "347", "348", "349", "352", "353", "354", "355", "356", "357", "358", "359",
            "362", "363", "364", "365", "366", "367", "368", "369", "370", "371", "372", "373", "374", "375", "376", "377", "378", "379", "380", "381",
            "382", "383", "384", "385", "386", "387", "388", "389", "412", "413", "414", "415", "416", "417", "422", "423", "424", "425", "426", "427",
            "428", "429", "432", "433", "434", "435", "436", "437", "438", "439", "442", "443", "446", "447", "452", "453", "454", "455", "456", "457",
            "458", "459", "462", "463", "464", "465", "466", "467", "472", "473", "474", "475", "476", "477", "478", "479", "482", "483", "484", "485",
            "486", "487", "488", "489"
        };
        #endregion



        public static bool ParseToBool(this string value)
        {
            switch (value?.TrimDeeper().ToUpperInvariant() ?? throw new ArgumentException(nameof(value)))
            {
                case "TRUE":
                case "YES":
                case "ON":
                case "OK":
                    return true;
                case "FALSE":
                case "NO":
                case "OFF":
                case "":
                    return false;
                default:
                    throw new FormatException();
            }
        }
        public static string TrimDeeper(this string s)
        {
            return string.Join(" ",
               s?.Trim()
               .Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
               .Select(x => new string(
                   x.Where(y => !char.IsWhiteSpace(y))
                    .ToArray())) ?? throw new ArgumentException(nameof(s)));
        }
        public static string TrimDeeperAndEncode(this string s)
        {
            return s == null ? null : WebUtility.HtmlEncode(s.TrimDeeper());
        }
        public static bool IsEmailAddress(this string s)
        {
            return !string.IsNullOrEmpty(s) && Util.TryToDo(() => new MailAddress(s));
        }
        public static bool TryConvertToPhoneNumber(this string s, out string phoneNumber)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            var _phoneNumber = s.Replace(" ", "").Replace("-", "").Replace("/", "").Replace("\\", "").Replace(".", "").Replace("(", "").Replace(")", "").TrimDeeper();
            if (!_phoneNumber.Equals(string.Empty))
            {
                var value = _phoneNumber;
                bool isTr = false;
                foreach (var code in PhoneCodesTR)
                    if (value.Length >= code.Length && value.Substring(0, code.Length).Equals(code))
                    {
                        isTr = true;
                        value = value.Substring(code.Length);
                        break;
                    }
                bool isInternational = !isTr && (value[0].Equals('+') || (value.Length >= 2 && value.Substring(0, 2).Equals("00")));
                value = !isInternational ? value : value.Substring(value[0].Equals('+') ? 1 : 2);
                if (value.All(c => char.IsNumber(c)) && (isInternational ? value.Length >= 7 : (value[0].Equals('0') ? (value.Length == 11 && PhoneNumbersTR.Contains(value.Substring(1, 3))) : ((value.Length == 3 && value[0].Equals('1')) || (value.Length == 7 && value.Substring(0, 3).Equals("444")) || (value.Length == 10 && PhoneNumbersTR.Contains(value.Substring(0, 3)))))))
                {
                    phoneNumber = value;
                    return true;
                }
            }
            phoneNumber = null;
            return false;
        }
    }
}