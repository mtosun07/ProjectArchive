using System;

namespace AOE3_HomeCity.Entities
{
    //public interface IHomeCityComponent<out T>
    //{
    //    bool IsEmpty { get; }
    //    bool ExactEquals(T other);
    //    bool UniqueIdEquals(T other);
    //}

    public abstract class HomeCityComponent<T> : IEquatable<HomeCityComponent<T>>
        where T : class
    {
        protected HomeCityComponent() { }


        protected virtual int HashCode { get { return base.GetHashCode(); } }

        public abstract bool IsEmpty { get; }



        public abstract bool ExactEquals(T other);
        public abstract bool UniqueIdEquals(T other);
        public bool Equals(HomeCityComponent<T> other)
        {
            return other.ExactEquals(other as T);
        }

        public override bool Equals(object obj)
        {
            return obj is HomeCityComponent<T> ? Equals(obj as HomeCityComponent<T>) : ReferenceEquals(this, obj);
        }
        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}