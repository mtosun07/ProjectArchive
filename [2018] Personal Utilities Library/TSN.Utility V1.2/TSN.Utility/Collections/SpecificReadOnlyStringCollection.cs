using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using TSN.Hashing;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.Collections
{
    [Serializable()] [NativeHashable()]
    public class SpecificReadOnlyStringCollection : SpecificReadOnlyCollectionBase<string>
    {
        public SpecificReadOnlyStringCollection(ICollection<string> collection)
            : base(collection) { }
        protected SpecificReadOnlyStringCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        protected sealed override void ValidateCollection(ICollection<string> collection)
        {
            base.ValidateCollection(collection);
            if (collection.Any(e => e.Equals(string.Empty)))
                throw new ArgumentInnerValueEmptyException(nameof(collection), ArgumentTypes.String, "elements");
        }
        protected sealed override string CloneElement(string element) => element?.Clone<string>();
        public sealed override object Clone() => new SpecificReadOnlyStringCollection(_collection);

        public static implicit operator SpecificReadOnlyStringCollection(string[] array) => array == null ? null : new SpecificReadOnlyStringCollection(array);
        public static implicit operator SpecificReadOnlyStringCollection(List<string> list) => list == null ? null : new SpecificReadOnlyStringCollection(list);
        public static implicit operator SpecificReadOnlyStringCollection(StringCollection collection) => collection?.ToGenericEnumerable<string>().ToArray();
        public static implicit operator SpecificReadOnlyStringCollection(ReadOnlyCollection<string> collection) => collection == null ? null : new SpecificReadOnlyStringCollection(collection);
    }
}