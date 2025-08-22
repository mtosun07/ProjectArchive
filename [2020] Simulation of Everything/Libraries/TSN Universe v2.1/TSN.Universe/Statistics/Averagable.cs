using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.Statistics
{
    [Serializable] public partial struct Averagable : IStatisticsMagnitude, IEquatable<Averagable>
    {
        internal Averagable(Average? localCountNew, Average? localCountAll, Average? generalCount)
        {
            _localAverageNew = localCountNew;
            _localAverageAll = localCountAll;
            _generalAverage = generalCount;
        }
        private Averagable(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var localCountNew = (Average?)info.GetValue(Field_LocalAverageNew, typeof(Average?));
            var localCountAll = (Average?)info.GetValue(Field_LocalAverageAll, typeof(Average?));
            var generalCount = (Average?)info.GetValue(Field_GeneralAverage, typeof(Average?));
            _localAverageNew = localCountNew;
            _localAverageAll = localCountAll;
            _generalAverage = generalCount;
        }


        private const string Field_LocalAverageNew = "LocalAverageNew";
        private const string Field_LocalAverageAll = "LocalAverageAll";
        private const string Field_GeneralAverage = "GeneralAverage";

        private readonly Average? _localAverageNew;
        private readonly Average? _localAverageAll;
        private readonly Average? _generalAverage;

        public Average? LocalAverageNew => _localAverageNew;
        public Average? LocalAverageAll => _localAverageAll;
        public Average? GeneralAverage => _generalAverage;
        MagnitudeTypes IStatisticsMagnitude.MagnitudeType => MagnitudeTypes.Average;
        IConvertible IStatisticsMagnitude.LocalMagnitudeNew => _localAverageNew ?? null;
        IConvertible IStatisticsMagnitude.LocalMagnitudeAll => _localAverageAll ?? null;
        IConvertible IStatisticsMagnitude.GeneralMagnitude => _generalAverage ?? null;



        public override string ToString() => $"({string.Join(" | ", new[] { _localAverageNew.HasValue ? $"{Field_LocalAverageNew}: {_localAverageNew.Value.ToString(Average.ToStringFormat)}" : null, _localAverageAll.HasValue ? $"{Field_LocalAverageAll}: {_localAverageAll.Value.ToString(Average.ToStringFormat)}" : null, _generalAverage.HasValue ? $"{Field_GeneralAverage}: {_generalAverage.Value.ToString(Average.ToStringFormat)}" : null }.Where(x => x != null))})";
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 27;
                hash = (13 * hash) + (_localAverageNew?.GetHashCode() ?? 0);
                hash = (13 * hash) + (_localAverageAll?.GetHashCode() ?? 0);
                hash = (13 * hash) + (_generalAverage?.GetHashCode() ?? 0);
                return hash;
            }
        }
        public override bool Equals(object obj) => obj is Countable c && Equals(c);

        public bool Equals(Averagable other) => _localAverageNew.Equals(other._localAverageNew) && _localAverageAll.Equals(other._localAverageAll) && _generalAverage.Equals(other._generalAverage);
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(Field_LocalAverageNew, _localAverageNew, typeof(Average?));
            info.AddValue(Field_LocalAverageAll, _localAverageAll, typeof(Average?));
            info.AddValue(Field_GeneralAverage, _generalAverage, typeof(Average?));
        }

        public static implicit operator Averagable((Average?, Average?, Average?) tuple) => new Averagable(tuple.Item1, tuple.Item2, tuple.Item3);
        public static implicit operator (Average?, Average?, Average?)(Averagable o) => (o._localAverageNew, o._localAverageAll, o._generalAverage);
    }
}