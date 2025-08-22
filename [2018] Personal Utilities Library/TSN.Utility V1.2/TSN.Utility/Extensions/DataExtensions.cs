using System;
using System.IO;
using System.Net;
using TSN.Utility.Exceptions;

namespace TSN.Utility.Extensions
{
    public static class DataExtensions
    {
        public static bool TryReadData(string filePath, out byte[] buffer)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(filePath));
            buffer = null;
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[fs.Length];
                    for (int i = 0, b = fs.ReadByte(); b != -1; b = fs.ReadByte())
                        buffer[i++] = (byte)b;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool TryDownloadData(Uri uri, out byte[] buffer)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (!uri.IsAbsoluteUri || uri.IsFile)
                throw new ArgumentException("Specified string parameter was local.", nameof(uri));
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = "Other";
            request.AllowAutoRedirect = false;
            try
            {
                using (var memory = new MemoryStream())
                {
                    using (var response = (HttpWebResponse)(request.GetResponse()))
                    {
                        using (var imgStream = response.GetResponseStream())
                        {
                            imgStream.CopyTo(memory);
                            imgStream.Flush();
                            imgStream.Close();
                        }
                        response.Close();
                    }
                    memory.Flush();
                    buffer = memory.ToArray();
                    memory.Close();
                }
                return true;
            }
            catch
            {
                buffer = null;
                return false;
            }
        }
        public static bool TryDownloadData(string uri, out byte[] buffer)
        {
            if (string.IsNullOrEmpty(uri))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(uri));
            return TryDownloadData(new Uri(uri), out buffer);
        }
    }
}