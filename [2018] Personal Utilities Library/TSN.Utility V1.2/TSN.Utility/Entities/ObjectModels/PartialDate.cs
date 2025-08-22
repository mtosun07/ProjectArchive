using System;
using System.Globalization;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.Entities.ObjectModels
{
    [Serializable()] [NativeHashable()]
    public sealed class PartialDate : EntityBase<PartialDate>, IComparable, IComparable<PartialDate>
    {
        public PartialDate(int year)
        {
            ValidateParameters(year);
            _year = year;
            _month = null;
            _day = null;
        }
        public PartialDate(int year, int month)
        {
            ValidateParameters(year, month);
            _year = year;
            _month = month;
            _day = null;
        }
        public PartialDate(int year, int month, int day)
        {
            ValidateParameters(year, month, day);
            _year = year;
            _month = month;
            _day = day;
        }
        private PartialDate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            int year = info.GetInt32(YearField);
            int? month = info.GetValue<int?>(MonthField);
            int? day = info.GetValue<int?>(DayField);
            ValidateParameters(year, month, day);
            _year = year;
            _month = month;
            _day = day;
        }


        private const string YearField = "Year";
        private const string MonthField = "Month";
        private const string DayField = "Day";

        private readonly int _year;
        private readonly int? _month;
        private readonly int? _day;

        public int Year
        {
            get { return _year; }
        }
        public int? Month
        {
            get { return _month; }
        }
        public int? Day
        {
            get { return _day; }
        }



        private static void ValidateParameters(int year, int? month = null, int? day = null)
        {
            if (!month.HasValue && day.HasValue)
                throw new ArgumentContradictionException(nameof(month), "was null", nameof(day), "had value.", false);
            try
            {
                var temp = new DateTime(year, month ?? 1, day ?? 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected sealed override object[] GetHashCodesOf()
        {
            return new object[] { Year, (Month ?? 0), (Day ?? 0) };
        }
        protected sealed override bool EqualsMemberwise(PartialDate other)
        {
            return Year == other.Year && Month.Equals(other.Month) && Day.Equals(other.Day);
        }
        public sealed override string ToString()
        {
            if (Day.HasValue)
                return ((DateTime)this).ToString();
            if (Month.HasValue)
                return $"{Month.Value}{CultureInfo.InvariantCulture.DateTimeFormat.DateSeparator}{Year}";
            return Year.ToString();
        }
        public sealed override object Clone()
        {
            if (Day.HasValue)
                return new PartialDate(Year, Month.Value, Day.Value);
            if (Month.HasValue)
                return new PartialDate(Year, Month.Value);
            return new PartialDate(Year);
        }
        public sealed override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(YearField, Year);
            info.AddValue<int?>(MonthField, Month);
            info.AddValue<int?>(DayField, Day);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as PartialDate);
        }
        public int CompareTo(PartialDate other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Year != other.Year || !Month.HasValue || !other.Month.HasValue)
                return Year.CompareTo(other.Year);
            if (Month.Value != other.Month.Value || !Day.HasValue || !other.Day.HasValue)
                return Month.Value.CompareTo(other.Month.Value);
            return Day.Value.CompareTo(other.Day.Value);
        }

        public static bool operator <(PartialDate left, PartialDate right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            return left.CompareTo(right) < 0;
        }
        public static bool operator >(PartialDate left, PartialDate right)
        {
            if (left == null || right == null)
                throw new ArgumentNullException();
            return left.CompareTo(right) > 0;
        }

        public static implicit operator PartialDate(DateTime value)
        {
            return new PartialDate(value.Year, value.Month, value.Day);
        }
        public static explicit operator DateTime(PartialDate value)
        {
            if (!value.Day.HasValue || !value.Month.HasValue)
                throw new FormatException("Some values (day, month and/or year) were null.");
            return new DateTime(value.Year, value.Month.Value, value.Day.Value);
        }
    }
}
