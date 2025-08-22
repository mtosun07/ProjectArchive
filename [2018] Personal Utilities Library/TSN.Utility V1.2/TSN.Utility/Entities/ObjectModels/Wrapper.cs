using System;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Hashing.FNV;
using TSN.Utility.Extensions;

namespace TSN.Utility.Entities.ObjectModels
{
    [Serializable()] [NativeHashable()]
    public sealed class Wrapper<T> : IEquatable<Wrapper<T>>, ICloneable, ISerializable
            where T : struct
    {
        public Wrapper(T obj)
        {
            _obj = obj;
        }
        private Wrapper(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (!info.ObjectType.Equals(GetType()) && !info.ObjectType.IsSubclassOf(GetType()))
                throw new InvalidOperationException();
            _obj = info.GetValue<T>(ObjField);
        }


        private const string ObjField = "Obj";

        private readonly T _obj;

        public T Obj => _obj;



        public override string ToString()
        {
            return _obj.ToString();
        }
        public override int GetHashCode()
        {
            return _obj.GetHashCodeFNV32();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Wrapper<T>);
        }
        public bool Equals(Wrapper<T> other)
        {
            return other != null && _obj.Equals(other._obj);
        }
        public object Clone()
        {
            return _obj.TryForceToClone(out T tmp) ? tmp : _obj;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue<T>(ObjField, _obj);
        }

        public static implicit operator T(Wrapper<T> obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return obj._obj;
        }
        public static implicit operator Wrapper<T>(T obj)
        {
            return new Wrapper<T>(obj);
        }
    }
}