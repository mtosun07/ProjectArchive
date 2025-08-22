using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TSN.Utility.Extensions
{
    public static class SerializationExtensions
    {
        public static MemoryStream ToMemoryStream(this IEnumerable<byte> buffer)
        {
            return new MemoryStream(buffer?.ToArray() ?? throw new ArgumentNullException(nameof(buffer)));
        }
        public static byte[] Serialize<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            byte[] buffer = null;
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, model);
                buffer = ms.ToArray();
                ms.Flush();
                ms.Close();
            }
            return buffer;
        }
        public static MemoryStream SerializeToMemory<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, model);
            ms.Flush();
            return ms;
        }
        public static T Deserialize<T>(this byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            T model = default(T);
            if (buffer.Length > 0)
                using (var ms = new MemoryStream(buffer))
                {
                    ms.Flush();
                    ms.Position = 0;
                    model = (T)new BinaryFormatter().Deserialize(ms);
                    ms.Close();
                }
            return model;
        }
        public static T Deserialize<T>(this MemoryStream graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            T model = default(T);
            if (graph.Length > 0)
            {
                graph.Flush();
                graph.Position = 0;
                model = (T)new BinaryFormatter().Deserialize(graph);
            }
            return model;
        }
        public static byte[] Compress(this byte[] buffer)
        {
            if (buffer == null)
                return null;
            if (buffer.Length == 0)
                return new byte[0];
            byte[] zip = null;
            using (var ms = new MemoryStream())
            {
                using (var zs = new GZipStream(ms, CompressionMode.Compress))
                {
                    zs.Write(buffer, 0, buffer.Length);
                    zs.Close();
                }
                zip = ms.ToArray();
                ms.Close();
            }
            return zip;
        }
        public static byte[] Compress(this MemoryStream graph)
        {
            if (graph == null)
                return null;
            if (graph.Length == 0)
                return new byte[0];
            byte[] zip = null;
            using (var ms = new MemoryStream())
            {
                using (var zs = new GZipStream(ms, CompressionMode.Compress))
                {
                    graph.Position = 0;
                    graph.WriteTo(zs);
                    zs.Close();
                }
                zip = ms.ToArray();
                ms.Close();
            }
            return zip;
        }
        public static MemoryStream CompressToMemory(this byte[] buffer)
        {
            if (buffer == null)
                return null;
            var ms = new MemoryStream();
            using (var zs = new GZipStream(ms, CompressionMode.Compress))
            {
                zs.Write(buffer, 0, buffer.Length);
                zs.Close();
            }
            return ms;
        }
        public static MemoryStream CompressToMemory(this MemoryStream graph)
        {
            if (graph == null)
                return null;
            var ms = new MemoryStream();
            using (var zs = new GZipStream(ms, CompressionMode.Compress))
            {
                graph.WriteTo(zs);
                zs.Close();
            }
            return ms;
        }
        public static byte[] Decompress(this byte[] zipBuffer)
        {
            if (zipBuffer == null)
                return null;
            if (zipBuffer.Length == 0)
                return new byte[0];
            byte[] buffer = null;
            using (var zipStream = new MemoryStream())
            {
                using (var ms = new MemoryStream(zipBuffer))
                using (var zs = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zs.CopyTo(zipStream);
                    zs.Close();
                    ms.Close();
                }
                buffer = zipStream.ToArray();
                zipStream.Close();
            }
            return buffer;
        }
        public static byte[] Decompress(this MemoryStream zipStream)
        {
            if (zipStream == null)
                return null;
            if (zipStream.Length == 0)
                return new byte[0];
            byte[] buffer = null;
            zipStream.Position = 0;
            using (var ms = new MemoryStream())
            {
                using (var zs = new GZipStream(zipStream, CompressionMode.Decompress))
                {
                    zs.CopyTo(ms);
                    zs.Close();
                }
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }
        public static MemoryStream DecompressToMemory(this byte[] zipBuffer)
        {
            if (zipBuffer == null)
                return null;
            var zipStream = new MemoryStream();
            using (var ms = new MemoryStream(zipBuffer))
            using (var zs = new GZipStream(ms, CompressionMode.Decompress))
            {
                zs.CopyTo(zipStream);
                zs.Close();
                ms.Close();
            }
            return zipStream;
        }
        public static MemoryStream DecompressToMemory(this MemoryStream zipStream)
        {
            if (zipStream == null)
                return null;
            zipStream.Position = 0;
            var ms = new MemoryStream();
            using (var zs = new GZipStream(zipStream, CompressionMode.Decompress))
            {
                zs.CopyTo(ms);
                zs.Close();
            }
            return ms;
        }
        public static byte[] SerializeAndCompress<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            byte[] buffer = null;
            using (var ms = new MemoryStream())
            {
                using (var zs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    new BinaryFormatter().Serialize(zs, model);
                    zs.Close();
                }
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }
        public static MemoryStream SerializeAndCompressToMemory<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var ms = new MemoryStream();
            using (var zs = new GZipStream(ms, CompressionMode.Compress, true))
            {
                (new BinaryFormatter()).Serialize(zs, model);
                zs.Close();
            }
            return ms;
        }
        public static T DecompressAndDeserialize<T>(this byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            T model = default(T);
            if (buffer.Length > 0)
                using (var ms = new MemoryStream(buffer))
                using (var zs = new GZipStream(ms, CompressionMode.Decompress, true))
                {
                    model = (T)new BinaryFormatter().Deserialize(zs);
                    zs.Close();
                    ms.Close();
                }
            return model;
        }
        public static T DecompressAndDeserialize<T>(this MemoryStream graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            T model = default(T);
            if (graph.Length > 0)
                using (var zs = new GZipStream(graph, CompressionMode.Decompress, true))
                {
                    model = (T)(new BinaryFormatter()).Deserialize(zs);
                    zs.Close();
                }
            return model;
        }
        public static byte[] GetBytes(this string s, Encoding encoding)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            return encoding != null ? encoding.GetBytes(s) : Encoding.UTF8.GetBytes(s);
        }
        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            return encoding != null ? encoding.GetString(bytes) : Encoding.UTF8.GetString(bytes);
        }
        public static byte[] GetBytes(this Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            return TypeDescriptor.GetConverter(image).ConvertTo(image, typeof(byte[])) as byte[];
        }
        public static T ToImage<T>(this byte[] buffer)
            where T : Image
        {
            if (buffer == null || buffer.Length == 0)
                return null;
            T img = null;
            using (var ms = new MemoryStream(buffer))
            {
                ms.Position = 0;
                img = Image.FromStream(ms) as T;
                ms.Flush();
                ms.Close();
            }
            return img;
        }
        public static Image ToImage(this byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return null;
            Image img = null;
            using (var ms = new MemoryStream(buffer))
            {
                ms.Position = 0;
                img = Image.FromStream(ms);
                ms.Flush();
                ms.Close();
            }
            return img;
        }
        public static string ToBase64String(this Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            return Convert.ToBase64String(image.GetBytes());
        }
        public static T ImageFromBase64String<T>(this string base64)
            where T : Image
        {
            if (base64 == null)
                throw new ArgumentNullException(base64);
            return Convert.FromBase64String(base64).ToImage<T>();
        }
        public static Image ImageFromBase64String(this string base64)
        {
            if (base64 == null)
                throw new ArgumentNullException(base64);
            return Convert.FromBase64String(base64).ToImage();
        }
        public static void AddValue<T>(this SerializationInfo info, string name, T value)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            info.AddValue(name, value, typeof(T));
        }
        public static void AddCollectionValues<TElement>(this SerializationInfo info, string name, IEnumerable<TElement> collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (collection == null)
            {
                info.AddValue($"{name}", -1);
                return;
            }
            int i = 0;
            foreach (var item in collection)
                info.AddValue<TElement>($"__{name}[{i++}]", item);
            info.AddValue($"{name}", i);
        }
        public static void AddCollectionValues<TKey, TValue>(this SerializationInfo info, string name, IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (collection == null)
            {
                info.AddValue($"{name}", -1);
                return;
            }
            int i = 0;
            string s = null;
            foreach (var item in collection)
            {
                info.AddValue<TKey>($"{(s = $"__{name}[{i++}].")}Key", item.Key);
                info.AddValue<TValue>($"{s}Value", item.Value);
            }
            info.AddValue($"{name}", i);
        }
        public static void AddCollectionValues<K, V1, V2>(this SerializationInfo info, string name, IEnumerable<KeyValuePair<K, Tuple<V1, V2>>> collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (collection == null)
            {
                info.AddValue($"{name}", -1);
                return;
            }
            int i = 0;
            string s = null;
            foreach (var item in collection)
            {
                info.AddValue<K>($"{(s = $"__{name}[{i++}].")}Key", item.Key);
                bool ok = item.Value != null;
                info.AddValue($"{s}Value", ok);
                if (ok)
                {
                    info.AddValue<V1>($"{s}Value.Item1", item.Value.Item1);
                    info.AddValue<V2>($"{s}Value.Item2", item.Value.Item2);
                }
            }
            info.AddValue($"{name}", i);
        }
        public static void AddCollectionValues<K, V1, V2, V3>(this SerializationInfo info, string name, IEnumerable<KeyValuePair<K, Tuple<V1, V2, V3>>> collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (collection == null)
            {
                info.AddValue($"{name}", -1);
                return;
            }
            int i = 0;
            string s = null;
            foreach (var item in collection)
            {
                info.AddValue<K>($"{(s = $"__{name}[{i++}].")}Key", item.Key);
                bool ok = item.Value != null;
                info.AddValue($"{s}Value", ok);
                if (ok)
                {
                    info.AddValue<V1>($"{s}Value.Item1", item.Value.Item1);
                    info.AddValue<V2>($"{s}Value.Item2", item.Value.Item2);
                    info.AddValue<V3>($"{s}Value.Item3", item.Value.Item3);
                }
            }
            info.AddValue($"{name}", i);
        }
        public static void AddCollectionValues_2DimJ<TElement>(this SerializationInfo info, string name, IEnumerable<IEnumerable<TElement>> collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (collection == null)
            {
                info.AddValue($"{name}", -1);
                return;
            }
            int i = 0, j = 0;
            string s = null;
            foreach (var x in collection)
            {
                j = -1;
                if (x != null)
                {
                    j = 0;
                    s = $"__{name}.[{i},{{0}}]";
                    foreach (var y in x)
                        info.AddValue<TElement>(string.Format(s, j++), y);
                }
                info.AddValue($"__{name}.[{i++}]", j);
            }
            info.AddValue($"{name}", i);
        }
        public static void AddCollectionValues_2Dim<TElement>(this SerializationInfo info, string name, TElement[,] collection)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length0 = collection?.GetLength(0) ?? -1;
            info.AddValue($"{name}", length0);
            if (length0 == -1)
                return;
            int length1 = collection.GetLength(1);
            info.AddValue($"{name}_", length1);
            string s = $"__{name}.[{{0}},{{1}}]";
            for (int i = 0; i < length0; i++)
                for (int j = 0; j < length1; j++)
                    info.AddValue<TElement>(string.Format(s, i, j), collection[i, j]);
        }
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            return (T)info.GetValue(name, typeof(T));
        }
        public static TElement[] GetCollectionValues<TElement>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length = info.GetInt32($"{name}");
            if (length < 0)
                return null;
            var array = new TElement[length];
            for (int i = 0; i < length; i++)
                array[i] = info.GetValue<TElement>($"__{name}[{i}]");
            return array;
        }
        public static KeyValuePair<TElementKey, TElementValue>[] GetCollectionValues<TElementKey, TElementValue>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length = info.GetInt32($"{name}");
            if (length < 0)
                return null;
            var array = new KeyValuePair<TElementKey, TElementValue>[length];
            for (int i = 0; i < length; i++)
                array[i] = new KeyValuePair<TElementKey, TElementValue>(
                    info.GetValue<TElementKey>($"__{name}[{i}].Key"),
                    info.GetValue<TElementValue>($"__{name}[{i}].Value"));
            return array;
        }
        public static KeyValuePair<K, Tuple<V1, V2>>[] GetCollectionValues<K, V1, V2>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length = info.GetInt32($"{name}");
            if (length < 0)
                return null;
            var array = new KeyValuePair<K, Tuple<V1, V2>>[length];
            for (int i = 0; i < length; i++)
                array[i] = new KeyValuePair<K, Tuple<V1, V2>>(
                    info.GetValue<K>($"__{name}[{i}].Key"),
                    !info.GetBoolean($"__{name}[{i}].Value") ? null : Tuple.Create(
                        info.GetValue<V1>($"__{name}[{i}].Value.Item1"),
                        info.GetValue<V2>($"__{name}[{i}].Value.Item2")));
            return array;
        }
        public static KeyValuePair<K, Tuple<V1, V2, V3>>[] GetCollectionValues<K, V1, V2, V3>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length = info.GetInt32($"{name}");
            if (length < 0)
                return null;
            var array = new KeyValuePair<K, Tuple<V1, V2, V3>>[length];
            for (int i = 0; i < length; i++)
                array[i] = new KeyValuePair<K, Tuple<V1, V2, V3>>(
                    info.GetValue<K>($"__{name}[{i}].Key"),
                    !info.GetBoolean($"__{name}[{i}].Value") ? null : Tuple.Create(
                        info.GetValue<V1>($"__{name}[{i}].Value.Item1"),
                        info.GetValue<V2>($"__{name}[{i}].Value.Item2"),
                        info.GetValue<V3>($"__{name}[{i}].Value.Item3")));
            return array;
        }
        public static TElement[][] GetCollectionValues_2DimJ<TElement>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length0 = info.GetInt32($"{name}");
            if (length0 < 0)
                return null;
            var array = new TElement[length0][];
            for (int i = 0; i < length0; i++)
            {
                int length1 = info.GetInt32($"__{name}.[{i}]");
                if (length1 == -1)
                    continue;
                array[i] = new TElement[length1];
                for (int j = 0; j < length1; j++)
                    array[i][j] = info.GetValue<TElement>($"__{name}.[{i},{j}]");
            }
            return array;
        }
        public static TElement[,] GetCollectionValues_2Dim<TElement>(this SerializationInfo info, string name)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int length0 = info.GetInt32($"{name}");
            if (length0 < 0)
                return null;
            int length1 = info.GetInt32($"{name}_");
            var array = new TElement[length0, length1];
            string s = $"__{name}.[{{0}},{{1}}]";
            for (int i = 0; i < length0; i++)
                for (int j = 0; j < length1; j++)
                    array[i, j] = info.GetValue<TElement>(string.Format(s, i, j));
            return array;
        }
    }
}