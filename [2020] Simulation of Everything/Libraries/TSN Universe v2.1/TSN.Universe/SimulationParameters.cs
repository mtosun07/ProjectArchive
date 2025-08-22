using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using TSN.Universe.Localization;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe
{
    [Serializable] public struct SimulationParameters : IEquatable<SimulationParameters>, ISerializable
    {
        static SimulationParameters()
        {
            _default = new SimulationParameters(options: SimulationOptions.Default);
        }
        public SimulationParameters(double spawnRate_Thing = 0.01D, double spawnRate_Food = 0.19D, double feminineRate = .5, double reproduceRate_Min = 0D, double reproduceRate_Max = 1D, double deathRate_Min = 0D, double deathRate_Max = 1D, ushort sense_Min = 1, ushort sense_Max = 5, SimulationOptions options = SimulationOptions.Default)
        {
            VaidateParameters(spawnRate_Thing, spawnRate_Food, feminineRate, reproduceRate_Min, reproduceRate_Max, deathRate_Min, deathRate_Max, sense_Min, sense_Max, options);
            _spawnRate_Thing = Math.Round(spawnRate_Thing, 2);
            _spawnRate_Food = Math.Round(spawnRate_Food, 2);
            _feminineRate = Math.Round(feminineRate, 2);
            _reproduceRate_Min = Math.Round(reproduceRate_Min, 2);
            _reproduceRate_Max = Math.Round(reproduceRate_Max, 2);
            _deathRate_Min = Math.Round(deathRate_Min, 2);
            _deathRate_Max = Math.Round(deathRate_Max, 2);
            _sense_Min = sense_Min;
            _sense_Max = sense_Max;
            _options = options;
        }
        private SimulationParameters(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var spawnRate_Thing = info.GetDouble(FieldSpawnRate_Thing);
            var spawnRate_Food = info.GetDouble(FieldSpawnRate_Food);
            var feminineRate = info.GetDouble(FieldFeminineRate);
            var reproduceRate_Min = info.GetDouble(FieldReproduceRate_Min);
            var reproduceRate_Max = info.GetDouble(FieldReproduceRate_Max);
            var deathRate_Min = info.GetDouble(FieldDeathRate_Min);
            var deathRate_Max = info.GetDouble(FieldDeathRate_Max);
            var sense_Min = info.GetUInt16(FieldSense_Min);
            var sense_Max = info.GetUInt16(FieldSense_Max);
            var options = (SimulationOptions)info.GetValue(FieldOptions, typeof(SimulationOptions));
            try
            {
                VaidateParameters(spawnRate_Thing, spawnRate_Food, feminineRate, reproduceRate_Min, reproduceRate_Max, deathRate_Min, deathRate_Max, sense_Min, sense_Max, options);
            }
            catch (ArgumentException ex)
            {
                throw new SerializationException(new SerializationException().Message, ex);
            }
            _spawnRate_Thing = spawnRate_Thing;
            _spawnRate_Food = spawnRate_Food;
            _feminineRate = feminineRate;
            _reproduceRate_Min = reproduceRate_Min;
            _reproduceRate_Max = reproduceRate_Max;
            _deathRate_Min = deathRate_Min;
            _deathRate_Max = deathRate_Max;
            _sense_Min = sense_Min;
            _sense_Max = sense_Max;
            _options = options;
        }



        private const string FieldSpawnRate_Thing = "SpawnRate_Thing";
        private const string FieldSpawnRate_Food = "SpawnRate_Food";
        private const string FieldFeminineRate = "FeminineRate";
        private const string FieldReproduceRate_Min = "ReproduceRate_Min";
        private const string FieldReproduceRate_Max = "ReproduceRate_Max";
        private const string FieldDeathRate_Min = "DeathRate_Min";
        private const string FieldDeathRate_Max = "DeathRate_Max";
        private const string FieldSense_Min = "Sense_Min";
        private const string FieldSense_Max = "Sense_Max";
        private const string FieldOptions = "Options";

        private static readonly SimulationParameters _default;
        
        private readonly double _spawnRate_Thing;
        private readonly double _spawnRate_Food;
        private readonly double _feminineRate;
        private readonly double _reproduceRate_Min;
        private readonly double _reproduceRate_Max;
        private readonly double _deathRate_Min;
        private readonly double _deathRate_Max;
        private readonly ushort _sense_Min;
        private readonly ushort _sense_Max;
        private readonly SimulationOptions _options;

        public static SimulationParameters Default => _default;

        public double SpawnRate_Thing => _spawnRate_Thing;
        public double SpawnRate_Food => _spawnRate_Food;
        public double FeminineRate => _feminineRate;
        public double ReproduceRate_Min => _reproduceRate_Min;
        public double ReproduceRate_Max => _reproduceRate_Max;
        public double DeathRate_Min => _deathRate_Min;
        public double DeathRate_Max => _deathRate_Max;
        public ushort Sense_Min => _sense_Min;
        public ushort Sense_Max => _sense_Max;
        public SimulationOptions Options => _options;



        private static void VaidateParameters(double spawnRate_Thing, double spawnRate_Food, double feminineRate, double reproduceRate_Min, double reproduceRate_Max, double deathRate_Min, double deathRate_Max, ushort sense_Min, ushort sense_Max, SimulationOptions options)
        {
            if (!Enum.TryParse<SimulationOptions>(options.ToString(), out _))
                throw new ArgumentOutOfRangeException(nameof(options));
            if (spawnRate_Thing < 0 || spawnRate_Thing > 1)
                throw new ArgumentOutOfRangeException(nameof(spawnRate_Thing));
            if (spawnRate_Food < 0 || spawnRate_Food > 1)
                throw new ArgumentOutOfRangeException(nameof(spawnRate_Food));
            if (feminineRate < 0 || feminineRate > 1)
                throw new ArgumentOutOfRangeException(nameof(feminineRate));
            if ((options & SimulationOptions.SpawnAtTheSameTime) != 0 && (spawnRate_Thing + spawnRate_Food) > 1)
                throw new ArgumentOutOfRangeException(nameof(spawnRate_Thing), string.Format(ExceptionMessages.ARGUMENTGREATERSUM, 1.ToString(Message.CurrentCulture), nameof(spawnRate_Food)));
            if (reproduceRate_Min < 0 || reproduceRate_Min > 1)
                throw new ArgumentOutOfRangeException(nameof(reproduceRate_Min));
            if (reproduceRate_Max < 0 || reproduceRate_Max > 1)
                throw new ArgumentOutOfRangeException(nameof(reproduceRate_Max));
            if (reproduceRate_Min > reproduceRate_Max)
                throw new ArgumentOutOfRangeException(nameof(reproduceRate_Min), string.Format(ExceptionMessages.ARGUMENTGREATER, reproduceRate_Max.ToString(Message.CurrentCulture), nameof(reproduceRate_Max)));
            if (deathRate_Min < 0 || deathRate_Min > 1)
                throw new ArgumentOutOfRangeException(nameof(deathRate_Min));
            if (deathRate_Max < 0 || deathRate_Max > 1)
                throw new ArgumentOutOfRangeException(nameof(deathRate_Max));
            if (deathRate_Min > deathRate_Max)
                throw new ArgumentOutOfRangeException(nameof(deathRate_Min), string.Format(ExceptionMessages.ARGUMENTGREATER, deathRate_Max.ToString(Message.CurrentCulture), nameof(deathRate_Max)));
            if (sense_Min < 1)
                throw new ArgumentOutOfRangeException(nameof(sense_Min));
            if (sense_Min > sense_Max)
                throw new ArgumentOutOfRangeException(nameof(sense_Min), string.Format(ExceptionMessages.ARGUMENTGREATER, sense_Max.ToString(Message.CurrentCulture), nameof(sense_Max)));
        }

        public override string ToString() => $"[{PARAMETERS_SPAWNRATE_THING}: {_spawnRate_Thing} | {PARAMETERS_SPAWNRATE_FOOD}: {_spawnRate_Food} | {PARAMETERS_FEMININERATE} : {_feminineRate} | {PARAMETERS_REPRODUCERATE_MIN}: {_reproduceRate_Min} | {PARAMETERS_REPRODUCERATE_MAX}: {_reproduceRate_Max} | {PARAMETERS_DEATHRATE_MIN}: {_deathRate_Min} | {PARAMETERS_DEATHRATE_MAX}: {_deathRate_Max} | {PARAMETERS_SENSE_MIN}: {_sense_Min} | {PARAMETERS_SENSE_MAX}: {_sense_Max} | {OPTIONS} : {((ulong)_options).ToString(Message.CurrentCulture)}]";
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 27;
                hash = (13 * hash) + _spawnRate_Thing.GetHashCode();
                hash = (13 * hash) + _spawnRate_Food.GetHashCode();
                hash = (13 * hash) + _reproduceRate_Min.GetHashCode();
                hash = (13 * hash) + _reproduceRate_Max.GetHashCode();
                hash = (13 * hash) + _deathRate_Min.GetHashCode();
                hash = (13 * hash) + _deathRate_Max.GetHashCode();
                hash = (13 * hash) + _sense_Min.GetHashCode();
                hash = (13 * hash) + _sense_Max.GetHashCode();
                hash = (13 * hash) + _feminineRate.GetHashCode();
                hash = (13 * hash) + ((uint)_options).GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj) => obj != null && obj is SimulationParameters && Equals((SimulationParameters)obj);

        public bool Equals(SimulationParameters other) => _spawnRate_Thing == other._spawnRate_Thing && _spawnRate_Food == other._spawnRate_Food && _feminineRate == other._feminineRate && _reproduceRate_Min == other._reproduceRate_Min && _reproduceRate_Max == other._reproduceRate_Max && _deathRate_Min == other._deathRate_Min && _deathRate_Max == other._deathRate_Max && _sense_Min == other._sense_Min && _sense_Max == other._sense_Max && _options.Equals(other._options);
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldSpawnRate_Thing, _spawnRate_Thing);
            info.AddValue(FieldSpawnRate_Food, _spawnRate_Food);
            info.AddValue(FieldReproduceRate_Min, _reproduceRate_Min);
            info.AddValue(FieldReproduceRate_Max, _reproduceRate_Max);
            info.AddValue(FieldDeathRate_Min, _deathRate_Min);
            info.AddValue(FieldDeathRate_Max, _deathRate_Max);
            info.AddValue(FieldSense_Min, _sense_Min);
            info.AddValue(FieldSense_Max, _sense_Max);
            info.AddValue(FieldFeminineRate, _feminineRate);
            info.AddValue(FieldOptions, _options, typeof(SimulationOptions));
        }
    }
}