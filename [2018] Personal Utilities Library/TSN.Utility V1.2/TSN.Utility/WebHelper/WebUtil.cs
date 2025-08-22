using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using TSN.Utility.Exceptions;
using TSN.Utility.Extensions;

namespace TSN.Utility.WebHelper
{
    public static class WebUtil
    {
        public static string GetClientIpAddress()
        {
            var sv = HttpContext.Current.Request.ServerVariables;
            string ip = sv["HTTP_X_FORWARDED_FOR"];
            return string.IsNullOrEmpty(ip) ? sv["REMOTE_ADDR"] : ip;
        }
        public static IEnumerable<string> GetImageUrls(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullOrEmptyException(ArgumentTypes.String, nameof(directory));
            var baseUri = directory.Length >= 2 && directory.Substring(0, 2).Equals("~/") ? directory.Substring(2) : (directory.Length >= 1 && directory[0] == '/' ? directory.Substring(1) : directory);
            var path = HttpContext.Current.Server.MapPath($"~/{baseUri}");
            return Directory.Exists(path) ? Directory.GetFiles(path, "*", SearchOption.AllDirectories).Select(x =>
            {
                try
                {
                    byte[] buffer = null;
                    using (var img = Image.FromFile(x))
                        buffer = img.GetBytes();
                    return new { Buffer = buffer, FilePath = x };
                }
                catch
                {
                    return null;
                }
            }).Where(x => x != null && x.Buffer.GetImageFormat() != ImageFormat.ImageFormats.Unknown).Select(x =>
            {
                var _x = x.FilePath.Replace('\\', '/');
                return $"/{_x.Substring(_x.IndexOf(baseUri))}";
            }) : new string[] { };
        }
    }
}