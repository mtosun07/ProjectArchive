using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Things
{
    [Serializable] public sealed class MatchInfo : IEquatable<MatchInfo>, ISerializable
    {
        internal MatchInfo(uint generation, Thing otherParent, int childrenCount)
        {
            if (otherParent == null)
                throw new ArgumentNullException(nameof(otherParent));
            if (!otherParent.IsParent)
                throw new ArgumentException(ExceptionMessages.ARGUMENTPARENTNOT, nameof(otherParent));
            if (childrenCount < 0)
                throw new ArgumentOutOfRangeException(nameof(childrenCount));
            _generation = generation;
            _otherParent = otherParent;
            _childrenCount = childrenCount;
        }
        private MatchInfo(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var generation = info.GetUInt32(FieldGeneration);
            var otherParent = (Thing)info.GetValue(FieldOtherParent, typeof(Thing));
            var childrenCount = info.GetInt32(FieldChildrenCount);
            _generation = generation;
            _otherParent = otherParent;
            _childrenCount = childrenCount;
        }


        private const string FieldGeneration = "Generation";
        private const string FieldOtherParent = "OtherParent";
        private const string FieldChildrenCount = "ChildrenCount";

        private readonly uint _generation;
        private readonly Thing _otherParent;
        private readonly int _childrenCount;

        public uint Generation => _generation;
        public Thing OtherParent => _otherParent;
        public int ChildrenCount => _childrenCount;



        public override string ToString() => $"{GENERATION}: {_generation}, {CHILDRENCOUNT}: {_childrenCount}, {OTHERPARENT}: {_otherParent}";
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 27;
                hash = (13 * hash) + _generation.GetHashCode();
                hash = (13 * hash) + _otherParent.GetHashCode();
                hash = (13 * hash) + _childrenCount.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj) => Equals(obj as MatchInfo);

        public bool Equals(MatchInfo other) => other != null && _generation == other._generation && _otherParent.Equals(other._otherParent) && _childrenCount == other._childrenCount;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldGeneration, _generation);
            info.AddValue(FieldOtherParent, _otherParent, typeof(Thing));
            info.AddValue(FieldChildrenCount, _childrenCount);
        }
    }
}