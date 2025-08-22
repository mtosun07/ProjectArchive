using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters
{
    [Serializable] public sealed class MatterDictionary : IDictionary<Guid, Matter>, IReadOnlyDictionary<Guid, Matter>, IReadOnlyDictionary<uint, IReadOnlyCollection<Guid>>, ISerializable, IDeserializationCallback
    {
        internal MatterDictionary()
        {
            _guidDictionary = new Dictionary<Guid, Matter>();
            _uintDictionary = new Dictionary<uint, HashSet<Guid>>();
        }
        private MatterDictionary(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var guidDictionary = (Dictionary<Guid, Matter>)info.GetValue(FieldGuidDictionary, typeof(Dictionary<Guid, Matter>));
            var uintDictionary = (Dictionary<uint, HashSet<Guid>>)info.GetValue(FieldUintDictionary, typeof(Dictionary<uint, HashSet<Guid>>));
            _guidDictionary = guidDictionary;
            _uintDictionary = uintDictionary;
        }


        private const string FieldGuidDictionary = "GuidDictionary";
        private const string FieldUintDictionary = "UintDictionary";

        private readonly Dictionary<Guid, Matter> _guidDictionary;
        private readonly Dictionary<uint, HashSet<Guid>> _uintDictionary;

        Matter IDictionary<Guid, Matter>.this[Guid key] { get => _guidDictionary[key]; set => throw new NotImplementedException(); }
        public Matter this[Guid id] => _guidDictionary[id];
        public IReadOnlyCollection<Guid> this[uint generation] => _uintDictionary.TryGetValue(generation, out var ids) ? ids : (IReadOnlyCollection<Guid>)new Guid[0];
        public int Count => _guidDictionary.Count;
        public bool IsReadOnly => false;
        public ICollection<Guid> Ids => _guidDictionary.Keys;
        public ICollection<Matter> Matters => _guidDictionary.Values;
        public ICollection<uint> Generations => _uintDictionary.Keys;
        ICollection<Guid> IDictionary<Guid, Matter>.Keys => _guidDictionary.Keys;
        ICollection<Matter> IDictionary<Guid, Matter>.Values => _guidDictionary.Values;
        IEnumerable<Guid> IReadOnlyDictionary<Guid, Matter>.Keys => _guidDictionary.Keys;
        IEnumerable<Matter> IReadOnlyDictionary<Guid, Matter>.Values => _guidDictionary.Values;
        IEnumerable<uint> IReadOnlyDictionary<uint, IReadOnlyCollection<Guid>>.Keys => _uintDictionary.Keys;
        IEnumerable<IReadOnlyCollection<Guid>> IReadOnlyDictionary<uint, IReadOnlyCollection<Guid>>.Values => _uintDictionary.Values;



        internal void Add(Matter matter)
        {
            if (matter == null)
                throw new ArgumentNullException(nameof(matter));
            if (matter.Id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(matter));
            if (matter.VanishingGeneration.HasValue)
                throw new ArgumentException(string.Format(ExceptionMessages.ARGUMENTVANISHED, MATTER), nameof(matter));
            _guidDictionary.Add(matter.Id, matter);
            if (_uintDictionary.ContainsKey(matter.Generation))
                _uintDictionary[matter.Generation].Add(matter.Id);
            else
                _uintDictionary.Add(matter.Generation, new HashSet<Guid>() { matter.Id });
        }
        internal void RemoveGeneration(uint generation)
        {
            foreach (var id in _uintDictionary[generation])
                _guidDictionary.Remove(id);
            _uintDictionary.Remove(generation);
        }
        public IEnumerable<Matter> GetMattersOf(uint generation) => this[generation].Select(x => this[x]);

        public override int GetHashCode() => _guidDictionary.GetHashCode();
        public override bool Equals(object obj) => _guidDictionary.Equals(obj);
        public override string ToString() => _guidDictionary.ToString();

        public bool Contains(KeyValuePair<Guid, Matter> item) => _guidDictionary.Contains(item);
        public bool ContainsKey(Guid key) => _guidDictionary.ContainsKey(key);
        public bool ContainsKey(uint generation) => _uintDictionary.ContainsKey(generation);
        public bool TryGetValue(Guid key, out Matter value) => _guidDictionary.TryGetValue(key, out value);
        public bool TryGetValue(uint generation, out IReadOnlyCollection<Guid> matterIds)
        {
            if (_uintDictionary.TryGetValue(generation, out var ids))
            {
                matterIds = ids.ToList().AsReadOnly();
                return true;
            }
            matterIds = null;
            return false;
        }
        public IEnumerator<KeyValuePair<Guid, Matter>> GetEnumerator() => _guidDictionary.GetEnumerator();

        void IDictionary<Guid, Matter>.Add(Guid key, Matter value) => throw new NotImplementedException();
        void ICollection<KeyValuePair<Guid, Matter>>.Add(KeyValuePair<Guid, Matter> item) => throw new NotImplementedException();
        void ICollection<KeyValuePair<Guid, Matter>>.Clear() => throw new NotImplementedException();
        void ICollection<KeyValuePair<Guid, Matter>>.CopyTo(KeyValuePair<Guid, Matter>[] array, int arrayIndex) => throw new NotImplementedException();
        bool IDictionary<Guid, Matter>.Remove(Guid key) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<Guid, Matter>>.Remove(KeyValuePair<Guid, Matter> item) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => _guidDictionary.GetEnumerator();
        IEnumerator<KeyValuePair<uint, IReadOnlyCollection<Guid>>> IEnumerable<KeyValuePair<uint, IReadOnlyCollection<Guid>>>.GetEnumerator() => ((IEnumerable<KeyValuePair<uint, HashSet<Guid>>>)_uintDictionary).Select(x => new KeyValuePair<uint, IReadOnlyCollection<Guid>>(x.Key, x.Value)).GetEnumerator();
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldGuidDictionary, _guidDictionary, typeof(Dictionary<Guid, Matter>));
            info.AddValue(FieldUintDictionary, _uintDictionary, typeof(Dictionary<uint, HashSet<Guid>>));
        }
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            _guidDictionary.OnDeserialization(sender);
            _uintDictionary.OnDeserialization(sender);
        }
    }
}