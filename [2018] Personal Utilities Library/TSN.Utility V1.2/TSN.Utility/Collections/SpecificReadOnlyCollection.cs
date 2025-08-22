using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyCollection<TElement> : SpecificReadOnlyCollectionBase<TElement>
        where TElement: IEquatable<TElement>, ICloneable, ISerializable
    {
        public SpecificReadOnlyCollection(ICollection<TElement> collection)
            : base(collection) { }
        protected SpecificReadOnlyCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        protected sealed override TElement CloneElement(TElement element) => element == null ? (TElement)(object)null : element.Clone<TElement>();
        public override object Clone() => new SpecificReadOnlyCollection<TElement>(_collection);

        public static implicit operator SpecificReadOnlyCollection<TElement>(TElement[] array) => array == null ? null : new SpecificReadOnlyCollection<TElement>(array);
        public static implicit operator SpecificReadOnlyCollection<TElement>(List<TElement> list) => list == null ? null : new SpecificReadOnlyCollection<TElement>(list);
        public static implicit operator SpecificReadOnlyCollection<TElement>(ReadOnlyCollection<TElement> collection) => collection == null ? null : new SpecificReadOnlyCollection<TElement>(collection);
    }
}