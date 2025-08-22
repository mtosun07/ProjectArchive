using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using TSN.Hashing;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyStructCollection<TElement> : SpecificReadOnlyCollectionBase<TElement>
        where TElement: struct, IEquatable<TElement>
    {
        public SpecificReadOnlyStructCollection(ICollection<TElement> collection)
            : base(collection) { }
        protected SpecificReadOnlyStructCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }



        protected override TElement CloneElement(TElement element) => element;
        public override object Clone() => new SpecificReadOnlyStructCollection<TElement>(_collection);

        public static implicit operator SpecificReadOnlyStructCollection<TElement>(TElement[] array) => array == null ? null : new SpecificReadOnlyStructCollection<TElement>(array);
        public static implicit operator SpecificReadOnlyStructCollection<TElement>(List<TElement> list) => list == null ? null : new SpecificReadOnlyStructCollection<TElement>(list);
        public static implicit operator SpecificReadOnlyStructCollection<TElement>(ReadOnlyCollection<TElement> collection) => collection == null ? null : new SpecificReadOnlyStructCollection<TElement>(collection);
    }
}