using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.DesktopApp
{
    [Serializable] public struct ChartOptions : ISerializable, IEquatable<ChartOptions>
    {
        public ChartOptions(ChartTypes chartType, Magnitudes magnitude, params Statistics.StatisticsTitle[] titles)
        {
            if (!Enum.TryParse(chartType.ToString(), out ChartTypes ct))
                throw new ArgumentOutOfRangeException(nameof(chartType));
            if (!Enum.TryParse(magnitude.ToString(), out Magnitudes m))
                throw new ArgumentOutOfRangeException(nameof(magnitude));
            if (titles == null)
                throw new ArgumentNullException(nameof(titles));
            if (titles.Length == 0)
                throw new ArgumentException("Argument was empty.", nameof(titles));
            _chartType = ct;
            _magnitude = m;
            _titles = new SortedSet<Statistics.StatisticsTitle>(titles);
        }
        public ChartOptions(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var chartType = (ChartTypes)info.GetValue(FieldChartType, typeof(ChartTypes));
            var magnitude = (Magnitudes)info.GetValue(FieldMagnitude, typeof(Magnitudes));
            var titles = (SortedSet<Statistics.StatisticsTitle>)info.GetValue(FieldTitles, typeof(SortedSet<Statistics.StatisticsTitle>));
            _chartType = chartType;
            _magnitude = magnitude;
            _titles = titles;
        }


        private const string FieldChartType = "ChartType";
        private const string FieldMagnitude = "Magnitude";
        private const string FieldTitles = "Titles";

        private readonly ChartTypes _chartType;
        private readonly Magnitudes _magnitude;
        private readonly SortedSet<Statistics.StatisticsTitle> _titles;

        public ChartTypes ChartType => _chartType;
        public Magnitudes Magnitude => _magnitude;
        public bool IsEmpty => (_titles?.Count ?? 0) == 0;



        public IReadOnlyList<Statistics.StatisticsTitle> GetSelectedTitles() => _titles.ToList().AsReadOnly();

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 27;
                hash = 13 * hash + _chartType.GetHashCode();
                foreach (var t in _titles)
                    hash = 13 * hash + t.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj) => obj is ChartOptions s && Equals(s);

        public bool Equals(ChartOptions other) => _chartType.Equals(other._chartType) && (_titles ?? new SortedSet<Statistics.StatisticsTitle>()).SetEquals(other._titles ?? new SortedSet<Statistics.StatisticsTitle>());
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldChartType, _chartType, typeof(ChartTypes));
            info.AddValue(FieldMagnitude, _magnitude, typeof(Magnitudes));
            info.AddValue(FieldTitles, _titles, typeof(SortedSet<Statistics.StatisticsTitle>));
        }



        public enum ChartTypes : byte
        {
            SingleGeneration = 0,
            MultiGeneration = 1
        }
        public enum Magnitudes : byte
        {
            NewBorns = 0,
            CurrentSimulation = 1,
            AmongstAllSimulations = 2
        }
    }
}