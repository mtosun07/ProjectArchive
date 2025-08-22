using System;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Entities
{
    [Serializable()] [NativeHashable()]
    public abstract class EntityBaseWithID<TID, TDerived> : EntityBase<TDerived>
        where TID: IEquatable<TID>, ICloneable, ISerializable
        where TDerived: EntityBaseWithID<TID, TDerived>
    {
        protected EntityBaseWithID(TID ID)
        {
            if (_ID == null)
                throw new ArgumentNullException(nameof(ID));
            _ID = ID.Clone<TID>();
        }
        protected EntityBaseWithID(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var ID = info.GetValue<TID>(IDField);
            if (ID == null)
                throw new InvalidOperationException();
            _ID = ID;
        }


        private const string IDField = "ID";

        private readonly TID _ID;

        public TID ID => _ID;



        protected sealed override object[] GetHashCodesOf()
        {
            return _ID.MakeArray<object>();
        }
        protected override bool EqualsMemberwise(TDerived other)
        {
            return _ID.Equals(other._ID);
        }
        public override string ToString()
        {
            return $"ID: {_ID}";
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue<TID>(IDField, _ID);
        }
    }
}