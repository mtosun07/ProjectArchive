using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Things
{
    [Serializable] internal sealed class Reproducement : IEquatable<Reproducement>, ISerializable, IDeserializationCallback
    {
        internal Reproducement(Match spouses)
        {
            if (spouses.IsEmpty)
                throw new ArgumentException(ExceptionMessages.ARGUMENTEMPTY, nameof(spouses));
            _spouses = spouses;
            _children = new Dictionary<uint, HashSet<Guid>>();
        }
        private Reproducement(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var spouses = (Match)info.GetValue(FieldSpouses, typeof(Match));
            var children = (Dictionary<uint, HashSet<Guid>>)info.GetValue(FieldChildren, typeof(Dictionary<uint, HashSet<Guid>>));
            _spouses = spouses;
            _children = children;
        }


        private const string FieldSpouses = "Spouses";
        private const string FieldChildren = "Children";

        private readonly Match _spouses;
        private readonly Dictionary<uint, HashSet<Guid>> _children;

        internal Match Spouses => _spouses;
        internal IEnumerable<uint> Generations => _children.Keys;



        internal int GetChildrenCount() => _children.Sum(x => x.Value.Count);
        internal int GetChildrenCountAt(uint generation) => _children.ContainsKey(generation) ? _children[generation].Count : 0;
        internal IEnumerable<Guid> GetChildren() => _children.SelectMany(x => x.Value);
        internal IEnumerable<(uint Generation, IEnumerable<Guid> Children)> GetChildren(Predicate<int> predicateChildrenCount) => predicateChildrenCount == null ? throw new ArgumentNullException(nameof(predicateChildrenCount)) : _children.Where(x => predicateChildrenCount(x.Value.Count)).Select(x => (x.Key, (IEnumerable<Guid>)x.Value));
        internal bool TryGetChildrenAt(uint generation, out IReadOnlyCollection<Guid> children)
        {
            if (_children.TryGetValue(generation, out var set))
            {
                children = set;
                return true;
            }
            children = null;
            return false;
        }
        internal void AddChild(uint generation, Guid id)
        {
            if (_children.ContainsKey(generation))
                _children[generation].Add(id);
            else
                _children.Add(generation, new HashSet<Guid>() { id });
        }
        internal bool RevokeChangesAt(uint generation) => _children.Remove(generation) && _children.Count > 0;

        public override string ToString() => $"{SPOUSES}: {_spouses} | {CHILDRENCOUNT}: {GetChildrenCount()} | {GENERATIONS}: {{{string.Join(", ", Generations)}}}";
        public override int GetHashCode() => _spouses.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Reproducement);

        public bool Equals(Reproducement other) => other != null && _spouses.Equals(other._spouses);
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldSpouses, _spouses, typeof(Match));
            info.AddValue(FieldChildren, _children, typeof(Dictionary<uint, HashSet<Guid>>));
        }
        void IDeserializationCallback.OnDeserialization(object sender) => _children.OnDeserialization(sender);
    }
}