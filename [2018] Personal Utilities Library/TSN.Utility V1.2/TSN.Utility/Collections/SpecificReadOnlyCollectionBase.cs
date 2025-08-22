using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Entities;
using TSN.Utility.EqualityComparers;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public abstract class SpecificReadOnlyCollectionBase<TElement> : EntityBase<SpecificReadOnlyCollectionBase<TElement>>, IReadOnlyCollection<TElement>
        where TElement: IEquatable<TElement>
    {
        protected SpecificReadOnlyCollectionBase(ICollection<TElement> collection)
        {
            ValidateCollection(collection);
            _collection = collection.Select(e => CloneElement(e)).ToList().AsReadOnly();
        }
        protected SpecificReadOnlyCollectionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var collection = info.GetCollectionValues<TElement>(CollectionField);
            ValidateCollection(collection);
            _collection = new ReadOnlyCollection<TElement>(collection);
        }


        private const string CollectionField = "Collection";

        protected readonly ReadOnlyCollection<TElement> _collection;

        public TElement this[int index] => _collection[index];
        public int Count => _collection.Count;



        protected virtual void ValidateCollection(ICollection<TElement> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (collection.Any(e => e == null))
                throw new ArgumentInnerValueNullException(nameof(collection), ArgumentTypes.Collection, "elements");
        }

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
        public IEnumerator<TElement> GetEnumerator() => _collection.GetEnumerator();

        protected sealed override object[] GetHashCodesOf()
        {
            return CollectionEqualityComparer<TElement>.DefaultByFNV.GetHashCode(_collection).MakeArray<object>();
        }
        protected sealed override bool EqualsMemberwise(SpecificReadOnlyCollectionBase<TElement> other)
        {
            return CollectionEqualityComparer<TElement>.Default.Equals(_collection, other._collection);
        }
        public sealed override string ToString()
        {
            int padding = _collection.Count.ToString().Length;
            return string.Join(Environment.NewLine, _collection.Select((e, i) => $"#{i.ToString().PadLeft(padding, '0')}: {e}"));
        }
        public sealed override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddCollectionValues(CollectionField, _collection);
        }

        protected abstract TElement CloneElement(TElement element);
    }
}