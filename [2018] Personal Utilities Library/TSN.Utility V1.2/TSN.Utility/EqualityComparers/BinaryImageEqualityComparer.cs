using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using TSN.Hashing.FNV;
using TSN.Utility.Extensions;

namespace TSN.Utility.EqualityComparers
{
    public sealed class BinaryImageEqualityComparer<TImage> : IEqualityComparer<TImage>
        where TImage : Image
    {
        static BinaryImageEqualityComparer()
        {
            _default = new BinaryImageEqualityComparer<TImage>(false);
            _defaultByFNV = new BinaryImageEqualityComparer<TImage>(true);
            _converter = TypeDescriptor.GetConverter(typeof(Bitmap));
        }
        private BinaryImageEqualityComparer(bool calculateHashCodeByFNV)
        {
            _seqEqComp = calculateHashCodeByFNV ? SequenceEqualityComparer<byte>.DefaultByFNV : SequenceEqualityComparer<byte>.Default;
        }


        private static readonly BinaryImageEqualityComparer<TImage> _default;
        private static readonly BinaryImageEqualityComparer<TImage> _defaultByFNV;
        private static readonly TypeConverter _converter;

        private readonly SequenceEqualityComparer<byte> _seqEqComp;

        public static BinaryImageEqualityComparer<TImage> Default
        {
            get { return _default; }
        }
        public static BinaryImageEqualityComparer<TImage> DefaultByFNV
        {
            get { return _defaultByFNV; }
        }



        public bool Equals(TImage x, TImage y)
        {
            return _seqEqComp.Equals(x.Serialize(), y.Serialize());
        }
        public int GetHashCode(TImage obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return obj.GetHashCodeFNV32();
        }
    }
}