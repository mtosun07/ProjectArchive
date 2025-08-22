using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.Matters
{
    [Serializable] public struct Location : IEquatable<Location>, ISerializable
    {
        public Location(ushort x, ushort y)
        {
            _x = x;
            _y = y;
        }
        private Location(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var x = info.GetUInt16(FieldX);
            var y = info.GetUInt16(FieldY);
            _x = x;
            _y = y;
        }


        private const string FieldX = "X";
        private const string FieldY = "Y";
        
        private readonly ushort _x;
        private readonly ushort _y;

        public ushort X => _x;
        public ushort Y => _y;




        public bool IsAdjacentTo(Location location, ushort distance) => distance > 0 && _x <= location._x + distance && _y <= location._y + distance;

        public override int GetHashCode() => _x + (31 * _y);
        public override bool Equals(object obj) => obj is Location && Equals((Location)obj);
        public override string ToString() => $"({_x}, {_y})";

        public bool Equals(Location other) => _x == other._x && _y == other._y;
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldX, _x);
            info.AddValue(FieldY, _y);
        }

        public static implicit operator Location((ushort, ushort) tuple) => new Location(tuple.Item1, tuple.Item2);
        public static implicit operator (ushort X, ushort Y)(Location location) => (location.X, location.Y);
    }
}