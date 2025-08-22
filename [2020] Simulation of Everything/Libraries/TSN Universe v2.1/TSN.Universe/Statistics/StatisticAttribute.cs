using System;

namespace TSN.Universe.Statistics
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class StatisticAttribute : Attribute
    {
        internal StatisticAttribute(byte titleOrder, string title)
        {
            var t = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
            _title = !t.Equals(string.Empty) ? t : throw new ArgumentException("Argument was having empty value.", nameof(title));
            _titleOrder = titleOrder;
        }


        private readonly byte _titleOrder;
        private readonly string _title;

        public byte TitleOrder => _titleOrder;
        public string Title => _title;
        public byte GroupOrder { get; set; }
        public string Group { get; set; }
        public byte SubTitleOrder { get; set; }
        public string SubTitle { get; set; }
    }
}