using System;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Entities
{
    [Serializable()] [NativeHashable()]
    public abstract class EntityBaseWithIDAndName<TID, TDerived> : EntityBaseWithID<TID, TDerived>
        where TID : IEquatable<TID>, ICloneable, ISerializable
        where TDerived : EntityBaseWithIDAndName<TID, TDerived>
    {
        public EntityBaseWithIDAndName(TID ID, string name)
            : base(ID)
        {
            _name = name?.Clone<string>() ?? throw new ArgumentNullException(nameof(name));
        }
        private EntityBaseWithIDAndName(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var name = info.GetString(NameField);
            _name = name?.Clone<string>() ?? throw new InvalidOperationException();
        }


        private const string NameField = "Name";

        private readonly string _name;

        public string Name => _name;



        protected override bool EqualsMemberwise(TDerived other)
        {
            return base.EqualsMemberwise(other) && _name.Equals(other._name);
        }
        public override string ToString()
        {
            return $"{_name} ({base.ToString()})";
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue<string>(NameField, _name);
        }
    }
}