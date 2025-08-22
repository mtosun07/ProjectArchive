using System;
using System.Runtime.Serialization;

namespace TSN.Utility.Entities
{
    public interface IEntity<TDerived> : IEquatable<TDerived>, ICloneable, ISerializable
        where TDerived : IEntity<TDerived>
    { }
}