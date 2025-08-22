using System;
using System.Collections.Generic;
using TSN.Hashing.FNV;
using TSN.Utility.Entities;

namespace TSN.Utility.EqualityComparers
{
    public sealed class EntityEqualityComparer<TEntity> : IEqualityComparer<TEntity>
        where TEntity : EntityBase<TEntity>
    {
        static EntityEqualityComparer()
        {
            _default = new EntityEqualityComparer<TEntity>(false);
            _defaultByFNV = new EntityEqualityComparer<TEntity>(true);
        }
        public EntityEqualityComparer(bool hashCodeByFNV, Func<TEntity, TEntity, bool> equals = null)
        {
            _equals = equals ?? new Func<TEntity, TEntity, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _getHashCode = !hashCodeByFNV ? new Func<TEntity, int>(obj => obj?.GetHashCode() ?? 0) : new Func<TEntity, int>(obj => HashingFNV32.GetHashCodeFNV32(obj));
        }
        public EntityEqualityComparer(Func<TEntity, TEntity, bool> equals = null, Func<TEntity, int> getHashCode = null)
        {
            _equals = equals ?? new Func<TEntity, TEntity, bool>((obj, other) => (obj == null) == (other == null) && (obj == null || obj.Equals(other)));
            _getHashCode = getHashCode ?? new Func<TEntity, int>(obj => obj?.GetHashCode() ?? 0);
        }


        private static readonly EntityEqualityComparer<TEntity> _default;
        private static readonly EntityEqualityComparer<TEntity> _defaultByFNV;

        private readonly Func<TEntity, TEntity, bool> _equals;
        private readonly Func<TEntity, int> _getHashCode;

        public static EntityEqualityComparer<TEntity> Default
        {
            get { return _default; }
        }
        public static EntityEqualityComparer<TEntity> DefaultByFNV
        {
            get { return _defaultByFNV; }
        }


        public bool Equals(TEntity x, TEntity y)
        {
            return _equals(x, y);
        }
        public int GetHashCode(TEntity obj)
        {
            return _getHashCode(obj);
        }
    }
}