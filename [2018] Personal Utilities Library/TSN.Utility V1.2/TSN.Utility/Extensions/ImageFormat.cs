using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using TSN.Utility.Exceptions;

namespace TSN.Utility.Extensions
{
    public static class ImageFormat
    {
        static ImageFormat()
        {
            _headersDictionary = new Dictionary<ImageFormats, byte[][]>()
                {
                    { ImageFormats.bmp, new[] { /*BMP*/Encoding.ASCII.GetBytes("BM") } },
                    { ImageFormats.gif, new[] { /*GIF*/Encoding.ASCII.GetBytes("GIF") } },
                    { ImageFormats.png, new[] { /*PNG*/new byte[] { 137, 80, 78, 71 } } },
                    { ImageFormats.tiff, new[] { /*TIFF*/new byte[] { 73, 73, 42 }, /*TIFF*/new byte[] { 77, 77, 42 } } },
                    { ImageFormats.jpeg, new[] { /*JPEG*/new byte[] { 255, 216, 255, 224 }, /*JPEG CANON*/new byte[] { 255, 216, 255, 225 } } }
                };
        }


        private static readonly IReadOnlyDictionary<ImageFormats, byte[][]> _headersDictionary;



        public static ImageFormats GetImageFormat(this byte[] buffer)
        {
            if ((buffer?.Length ?? throw new ArgumentNullException(nameof(buffer))) == 0)
                throw new ArgumentEmptyException(ArgumentTypes.Array, nameof(buffer));
            foreach (var kvp in _headersDictionary)
                foreach (var header in kvp.Value)
                    if (header.SequenceEqual(buffer.Take(header.Length)))
                        return kvp.Key;
            return ImageFormats.Unknown;
        }
        public static ImageFormats GetImageFormat(this MemoryStream imageStream)
        {
            return GetImageFormat(imageStream?.ToArray() ?? throw new ArgumentNullException(nameof(imageStream)));
        }
        public static ImageFormats GetImageFormat(this Image image)
        {
            return GetImageFormat(image?.GetBytes() ?? throw new ArgumentNullException(nameof(image)));
        }



        public enum ImageFormats : byte
        {
            Unknown = 0,
            bmp = 1,
            jpeg = 2,
            gif = 3,
            tiff = 4,
            png = 5
        }
    }
}