using System;
using System.Runtime.Serialization;
using TSN.Hashing;

namespace TSN.Utility.Entities.DataRecords
{
    [Serializable()] [NativeHashable()]
    public abstract class DataRecordWithEnabled<TDerived> : DataRecord<TDerived>, IEnabled
        where TDerived : DataRecordWithEnabled<TDerived>
    {
        public DataRecordWithEnabled(Guid ID, bool isEnabled)
            : base(ID)
        {
            _isEnabled = isEnabled;
        }
        protected DataRecordWithEnabled(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _isEnabled = info.GetBoolean(IsEnabledField);
        }


        private const string IsEnabledField = "IsEnabled";

        private readonly bool _isEnabled;

        public bool IsEnabled => _isEnabled;



        protected override bool EqualsMemberwise(TDerived other)
        {
            return base.EqualsMemberwise(other) && _isEnabled == other._isEnabled;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(IsEnabledField, _isEnabled);
        }
    }
}