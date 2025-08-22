using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters
{
    [Serializable] public abstract class Matter : IEquatable<Matter>, ISerializable, IDeserializationCallback
    {
        private protected Matter(Universe universe, Location location, bool isSpawned)
        {
            if (universe == null)
                throw new ArgumentNullException(nameof(universe));
            if (location.X >= universe.M || location.Y >= universe.N)
                throw new ArgumentOutOfRangeException(nameof(location));
            do
            {
                _id = Guid.NewGuid();
            } while (_id.Equals(Guid.Empty) || universe.AllMattersEver.ContainsKey(_id));
            _ownerUniverse = universe;
            _isSpawned = isSpawned;
            _generation = _ownerUniverse.CurrentGeneration.Value;
            _locations = new Dictionary<uint, Location> { { _generation, (_location = location) } };
            _vanishingGeneration = null;
            _replacedAndVanished = null;
        }
        private protected Matter(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Guid id = (Guid)info.GetValue(FieldId, typeof(Guid));
            uint generation = info.GetUInt32(FieldGeneration);
            uint? vanishingGeneration = (uint?)info.GetValue(FieldVanishingGeneration, typeof(uint?));
            var locations = (Dictionary<uint, Location>)info.GetValue(FieldLocations, typeof(Dictionary<uint, Location>));
            if (!Enum.TryParse(((DeathReasons)info.GetValue(FieldDeathReason, typeof(DeathReasons))).ToString(), out DeathReasons deathReason) ||
                (vanishingGeneration.HasValue && (generation > vanishingGeneration || deathReason == DeathReasons.NONE)))
                throw new SerializationException();
            var isSpawned = info.GetBoolean(FieldIsSpawned);
            var replacedAndVanished = (Guid?)info.GetValue(FieldReplacedAndVanished, typeof(Guid?));
            _id = id;
            _isSpawned = isSpawned;
            _generation = generation;
            _locations = locations;
            _vanishingGeneration = vanishingGeneration;
            _deathReason = deathReason;
            _replacedAndVanished = replacedAndVanished;
        }


        private const string FieldId = "Id";
        private const string FieldIsSpawned = "IsSpawned";
        private const string FieldGeneration = "Generation";
        private const string FieldLocations = "Locations";
        private const string FieldVanishingGeneration = "VanishingGeneration";
        private const string FieldDeathReason = "DeathReason";
        private const string FieldReplacedAndVanished = "ReplacedAndVanished";
        public const string FieldOwnerUniverse = nameof(_ownerUniverse);

        [NonSerialized] private Universe _ownerUniverse;
        private readonly Guid _id;
        private readonly bool _isSpawned;
        private readonly uint _generation;
        private readonly Dictionary<uint, Location> _locations;
        private uint? _vanishingGeneration;
        private DeathReasons _deathReason;
        private Guid? _replacedAndVanished;
        [NonSerialized] private Location _location;

        public DeathReasons DeathReason => _deathReason;
        public Universe OwnerUniverse => _ownerUniverse;
        public Guid Id => _id;
        public bool IsSpawned => _isSpawned;
        public uint Generation => _generation;
        public uint? VanishingGeneration => _vanishingGeneration;
        public Location Location => _location;
        public int LocationsCount => _locations.Count;

        public event EventHandler<SimulationEventArgs> Revoking;
        public event EventHandler<SimulationEventArgs> Revoked;
        public event EventHandler<MovingEventArgs> Moving;
        public event EventHandler<MovingEventArgs> Moved;
        public event EventHandler<VanishingEventArgs> Vanishing;
        public event EventHandler<VanishingEventArgs> Vanished;



        private protected abstract string TraitsToString();
        private protected abstract bool ValidateDeathReason(DeathReasons deathReason, Matter replacedTo);
        private protected abstract void RevokeChanges(uint generation);

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(FieldId, _id, typeof(Guid));
            info.AddValue(FieldIsSpawned, _isSpawned);
            info.AddValue(FieldGeneration, _generation);
            info.AddValue(FieldLocations, _locations, typeof(Dictionary<uint, Location>));
            info.AddValue(FieldVanishingGeneration, _vanishingGeneration, typeof(uint?));
            info.AddValue(FieldDeathReason, _deathReason, typeof(DeathReasons));
            info.AddValue(FieldReplacedAndVanished, _replacedAndVanished, typeof(Guid?));
        }
        private protected virtual void OnDeserialization(object sender)
        {
            _locations.OnDeserialization(sender);
            if (_locations.Count == 0)
                throw new SerializationException();
            _location = _locations[_locations.Keys.Max()];
        }
        private protected virtual void OnRevoking(SimulationEventArgs e) => Revoking?.Invoke(this, e);
        private protected virtual void OnMoving(MovingEventArgs e) => Moving?.Invoke(this, e);
        private protected virtual void OnMoved(MovingEventArgs e) => Moved?.Invoke(this, e);
        private protected virtual void OnVanishing(VanishingEventArgs e) => Vanishing?.Invoke(this, e);
        private protected virtual void OnVanished(VanishingEventArgs e) => Vanished?.Invoke(this, e);

        private protected void ThrowIfVanished()
        {
            if (!_ownerUniverse.AllMattersEver.ContainsKey(_id))
                throw new InvalidOperationException(ExceptionMessages.NOTEXISTS);
            if (_vanishingGeneration.HasValue)
                throw new InvalidOperationException(ExceptionMessages.VANISHED);
        }
        internal void ChangeLocation(Location targetLocation)
        {
            ThrowIfVanished();
            var initialLocation = Location;
            if (initialLocation.Equals(targetLocation))
                throw new ArgumentOutOfRangeException(nameof(targetLocation));
            OnMoving(new MovingEventArgs(initialLocation, targetLocation));
            _ownerUniverse.Move(this, targetLocation);
            _locations.Add(_ownerUniverse.CurrentGeneration.Value, targetLocation);
            _location = targetLocation;
            OnMoved(new MovingEventArgs(initialLocation, targetLocation));
        }
        internal void Vanish(DeathReasons deathReason, Matter replacedTo = null)
        {
            ThrowIfVanished();
            if (!ValidateDeathReason(deathReason, replacedTo))
                throw new ArgumentOutOfRangeException(nameof(deathReason));
            OnVanishing(new VanishingEventArgs(_ownerUniverse.CurrentGeneration));
            _replacedAndVanished = replacedTo?.Id;
            _ownerUniverse.Remove(this);
            _vanishingGeneration = _ownerUniverse.CurrentGeneration.Value;
            _deathReason = deathReason;
            OnVanished(new VanishingEventArgs(_ownerUniverse.CurrentGeneration));
        }
        internal void RevokeChangesAt(uint generation)
        {
            if (generation > _ownerUniverse.CurrentGeneration.Value)
                throw new ArgumentOutOfRangeException(nameof(generation));
            OnRevoking(new SimulationEventArgs(generation));
            if (_locations.Count > 1 && _locations.Remove(generation))
                _location = _locations[_locations.Keys.Max()];
            if (_vanishingGeneration == generation)
            {
                _vanishingGeneration = null;
                _deathReason = DeathReasons.NONE;
                _replacedAndVanished = null;
            }
            RevokeChanges(generation);
            Revoked?.Invoke(this, new SimulationEventArgs(generation));
        }
        public Location GetLocationAt(uint generation) => (_vanishingGeneration.HasValue && generation > _vanishingGeneration.Value) ? throw new ArgumentOutOfRangeException(nameof(generation)) : (_locations.TryGetValue(generation, out var location) ? location : _locations[_locations.Keys.Where(x => x < generation).Max()]);
        public IEnumerable<(uint Generation, Location Location)> GetLocations()
        {
            for (uint generation = _generation, maxGeneration = _vanishingGeneration ?? _ownerUniverse.CurrentGeneration.Value; generation <= maxGeneration; generation++)
                if (_locations.TryGetValue(generation, out var location))
                    yield return (generation, location);
        }
        public bool TryGetFirstMove(uint startingGeneration, out uint generation, out Location location)
        {
            var curr = OwnerUniverse.CurrentGeneration.Value;
            if (startingGeneration > curr || startingGeneration < _generation)
                throw new ArgumentOutOfRangeException(nameof(startingGeneration));
            for (uint gen = startingGeneration + 1; gen < curr; gen++)
                if (_locations.TryGetValue(gen, out var loc))
                {
                    generation = gen;
                    location = loc;
                    return true;
                }
            generation = 0;
            location = new Location();
            return false;
        }
        public bool TryGetReplacedAndVanished(out Matter replacedTo)
        {
            if (_replacedAndVanished.HasValue)
            {
                replacedTo = _ownerUniverse.AllMattersEver[_replacedAndVanished.Value];
                return true;
            }
            replacedTo = null;
            return false;
        }

        public sealed override string ToString()
        {
            var traits = TraitsToString();
            return $"[{GetType().Name} | {MATTER_ID}: {_id}, {GENERATION}: {_generation}, {LOCATION}: {Location}, {MATTER_VANISHING}: {_vanishingGeneration?.ToString() ?? "-"}{(string.IsNullOrEmpty(traits) ? string.Empty : $" | {traits}")}]";
        }
        public sealed override int GetHashCode() => _id.GetHashCode();
        public sealed override bool Equals(object obj) => Equals(obj as Matter);

        public virtual bool Equals(Matter other) => other != null && _id == other._id && _generation == other._generation && _location.Equals(other._location) && _vanishingGeneration == other._vanishingGeneration;
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            GetObjectData(info, context);
        }
        void IDeserializationCallback.OnDeserialization(object sender) => OnDeserialization(sender);
    }
}