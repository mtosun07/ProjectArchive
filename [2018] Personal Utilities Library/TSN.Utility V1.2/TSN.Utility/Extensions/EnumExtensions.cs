using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TSN.Utility.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsFlagDefined(this Enum e)
        {
            return !decimal.TryParse(e.ToString(), out decimal d);
        }
        public static string GetDescription(this Enum e)
        {
            var memberInfo = e.GetType().GetMember(e.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return e.ToString();
        }
        public static string NameOf(this Enum e)
        {
            return $"{e.GetType().Name}.{e.ToString()}";
        }
        public static bool TryParseToEnum<TEnum>(this string value, out TEnum[] results)
            where TEnum : struct
        {
            if (value == null)
                throw new ArgumentException(nameof(value));
            if (Enum.TryParse(value, out TEnum result))
            {
                results = new[] { result };
                return true;
            }
            var type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException($"Argument was not an enum type array.", nameof(results));
            results = null;
            var members = type.GetMembers();
            if (members != null)
            {
                var resultList = new List<TEnum>();
                foreach (var memberInfo in members)
                {
                    var attrs = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string desc = null;
                    if (attrs != null && attrs.Length > 0 && (desc = ((DescriptionAttribute)attrs[0]).Description).Equals(value))
                        resultList.Add((TEnum)Enum.Parse(type, memberInfo.Name));
                }
                resultList.TrimExcess();
                if (resultList.Count > 0)
                {
                    results = resultList.OrderBy(e => e).ToArray();
                    return true;
                }
            }
            return false;
        }
        public static TEnum[] ParseToEnum<TEnum>(this string value)
            where TEnum : struct
        {
            if (value == null)
                throw new ArgumentException(nameof(value));
            if (value.TryParseToEnum(out TEnum[] results))
                return results;
            throw new FormatException();
        }
    }
}