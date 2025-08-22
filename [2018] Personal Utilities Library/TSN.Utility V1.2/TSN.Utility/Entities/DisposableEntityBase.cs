using System;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Hashing.FNV;

namespace TSN.Utility.Entities
{
    [Serializable()] [NativeHashable()]
    public abstract class DisposableEntityBase<TDerived> : DisposableBase, IEntity<TDerived>
        where TDerived : DisposableEntityBase<TDerived>
    {
        protected DisposableEntityBase() { }
        protected DisposableEntityBase(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (!info.ObjectType.Equals(GetType()) && !info.ObjectType.IsSubclassOf(GetType()))
                throw new InvalidOperationException();
        }



        protected abstract object[] GetHashCodesOf();
        protected abstract bool EqualsMemberwise(TDerived other);
        public abstract object Clone();
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
        }

        public sealed override int GetHashCode()
        {
            return (GetHashCodesOf() ?? new object[0]).GetHashCodeFNV32(GetType());
        }
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as TDerived);
        }
        public bool Equals(TDerived other)
        {
            return other != null && (ReferenceEquals(this, other) || (GetType().Equals(other.GetType()) && EqualsMemberwise(other)));
        }
    }
}