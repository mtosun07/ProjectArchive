using System;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Entities.ObjectModels;

namespace TSN.Utility.Entities.DataRecords
{
    [Serializable()] [NativeHashable()]
    public abstract class DataRecord<TDerived> : EntityBaseWithID<Wrapper<Guid>, TDerived>, IID
        where TDerived : DataRecord<TDerived>
    {
        public DataRecord(Guid ID)
            : base(ID) { }
        protected DataRecord(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        public new Guid ID => base.ID;
    }
}