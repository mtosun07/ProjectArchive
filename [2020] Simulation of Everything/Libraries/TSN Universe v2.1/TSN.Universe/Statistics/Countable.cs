using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.Statistics
{
    [Serializable] public struct Countable : IStatisticsMagnitude, IEquatable<Countable>
    {
        static Countable()
        {
            ToStringFormat = "N";
        }
        internal Countable(uint? localCountNew, uint? localCountAll, uint? generalCount)
        {
            _localCountNew = localCountNew;
            _localCountAll = localCountAll;
            _generalCount = generalCount;
        }
        private Countable(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var localCountNew = (uint?)info.GetValue(Field_LocalCountNew, typeof(uint?));
            var localCountAll = (uint?)info.GetValue(Field_LocalCountAll, typeof(uint?));
            var generalCount = (uint?)info.GetValue(Field_GeneralCount, typeof(uint?));
            _localCountNew = localCountNew;
            _localCountAll = localCountAll;
            _generalCount = generalCount;
        }


        private const string Field_LocalCountNew = "LocalCountNew";
        private const string Field_LocalCountAll = "LocalCountAll";
        private const string Field_GeneralCount = "GeneralCount";

        private readonly uint? _localCountNew;
        private readonly uint? _localCountAll;
        private readonly uint? _generalCount;

        public static string ToStringFormat { get; set; }

        public uint? LocalCountNew => _localCountNew;
        public uint? LocalCountAll => _localCountAll;
        public uint? GeneralCount => _generalCount;
        MagnitudeTypes IStatisticsMagnitude.MagnitudeType => MagnitudeTypes.Summary;
        IConvertible IStatisticsMagnitude.LocalMagnitudeNew => _localCountNew ?? null;
        IConvertible IStatisticsMagnitude.LocalMagnitudeAll => _localCountAll ?? null;
        IConvertible IStatisticsMagnitude.GeneralMagnitude => _generalCount ?? null;



        public override string ToString() => $"({string.Join(" | ", (new[] { _localCountNew.HasValue ? $"{Field_LocalCountNew}: {_localCountNew.Value.ToString(ToStringFormat)}" : null, _localCountAll.HasValue ? $"{Field_LocalCountAll}: {_localCountAll.Value.ToString(ToStringFormat)}" : null, _generalCount.HasValue ? $"{Field_GeneralCount}: {_generalCount.Value.ToString(ToStringFormat)}" : null }).Where(x => x != null))})";
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 27;
                hash = (13 * hash) + (_localCountNew?.GetHashCode() ?? 0);
                hash = (13 * hash) + (_localCountAll?.GetHashCode() ?? 0);
                hash = (13 * hash) + (_generalCount?.GetHashCode() ?? 0);
                return hash;
            }
        }
        public override bool Equals(object obj) => obj is Countable c && Equals(c);

        public bool Equals(Countable other) => _localCountNew == other._localCountNew && _localCountAll == other._localCountAll && _generalCount == other._generalCount;
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(Field_LocalCountNew, _localCountNew, typeof(uint?));
            info.AddValue(Field_LocalCountAll, _localCountAll, typeof(uint?));
            info.AddValue(Field_GeneralCount, _generalCount, typeof(uint?));
        }

        public static implicit operator Countable((uint?, uint?, uint?) tuple) => new Countable(tuple.Item1, tuple.Item2, tuple.Item3);
        public static implicit operator (uint?, uint?, uint?)(Countable o) => (o._localCountNew, o._localCountAll, o._generalCount);
    }
}