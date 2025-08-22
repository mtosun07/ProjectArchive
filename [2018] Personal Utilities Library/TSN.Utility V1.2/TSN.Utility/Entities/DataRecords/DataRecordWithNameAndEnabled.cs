using System;
using System.Runtime.Serialization;
using TSN.Hashing;

namespace TSN.Utility.Entities.DataRecords
{
    [Serializable()] [NativeHashable()]
    public abstract class DataRecordWithNameAndEnabled<TDerived> : DataRecordWithName<TDerived>, IEnabled
        where TDerived : DataRecordWithNameAndEnabled<TDerived>
    {
        public DataRecordWithNameAndEnabled(Guid ID, string name, bool isEnabled)
            : base(ID, name)
        {
            _isEnabled = isEnabled;
        }
        protected DataRecordWithNameAndEnabled(SerializationInfo info, StreamingContext context)
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