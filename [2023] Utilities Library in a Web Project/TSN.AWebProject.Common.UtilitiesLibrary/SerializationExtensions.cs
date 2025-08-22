using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TSN.AWebProject.Common.UtilitiesLibrary
{
    public static class SerializationExtensions
    {
        private const string _serializationNameSeperator = "###";
        private const string _serializationNameKey = "Key";
        private const string _serializationNameValue = "Value";



        public static string ConvertToJson(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
            return JsonConvert.SerializeObject(new
            {
                ErrorDateUTC = DateTimeOffset.UtcNow.ToString("G"),
                Type = ex.GetType(),
                Data = ex.Data.Keys.Cast<object>().Select(x => new { Key = x, Value = ex.Data[x] }).ToArray(),
                ex.HelpLink,
                ex.HResult,
                InnerException = ex.InnerException?.ConvertToJson(),
                ex.Message,
                ex.Source,
                ex.StackTrace,
                ex.TargetSite
            });
        }

        public static byte[] Serialize(this ISerializable data)
        {
            if (data == null)
                return null;
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, data);
                return ms.ToArray();
            }
        }
        public static T Deserialize<T>(this byte[] bytes)
            where T : ISerializable
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
                return (T)formatter.Deserialize(ms);
        }
        public static bool TryToSerializeBinary(this object data, out byte[] bytes)
        {
            if (data == null)
            {
                bytes = null;
                return true;
            }
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, data);
                }
                catch
                {
                    bytes = null;
                    return false;
                }
                bytes = ms.ToArray();
                return true;
            }
        }
        public static bool TryToDeserializeBinary<T>(this byte[] bytes, out T data)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                try
                {
                    data = (T)formatter.Deserialize(ms);
                }
                catch
                {
                    data = default;
                    return false;
                }
                return true;
            }
        }

        public static void AddValue<T>(this SerializationInfo info, string name, T value) => info.AddValue(name, value, typeof(T));
        public static T GetValue<T>(this SerializationInfo info, string name) => (T)info.GetValue(name, typeof(T));
        public static void AddCollection<TElement>(this SerializationInfo info, string name, ICollection<TElement> collection)
        {
            info.AddValue(name, collection?.Count ?? -1);
            if (collection != null)
            {
                int i = 0;
                foreach (var item in collection)
                    info.AddValue<TElement>($"{name}{_serializationNameSeperator}{i++}", item);
            }
        }
        public static IEnumerable<TElement> GetCollection<TElement>(this SerializationInfo info, string name)
        {
            var count = info.GetInt32(name);
            return count == -1 ? null : Enumerable.Range(0, count).Select(i => info.GetValue<TElement>($"{name}{_serializationNameSeperator}{i}"));
        }
        public static void AddDictionary<TKey, TValue>(this SerializationInfo info, string name, ICollection<KeyValuePair<TKey, TValue>> dictionary)
        {
            info.AddValue(name, dictionary?.Count ?? -1);
            if (dictionary != null)
            {
                int i = 0;
                foreach (var item in dictionary)
                {
                    info.AddValue<TKey>($"{name}{_serializationNameSeperator}{_serializationNameKey}{i}", item.Key);
                    info.AddValue<TValue>($"{name}{_serializationNameSeperator}{_serializationNameValue}{i++}", item.Value);
                }
            }
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetDictionary<TKey, TValue>(this SerializationInfo info, string name)
        {
            var count = info.GetInt32(name);
            return count == -1 ? null : Enumerable.Range(0, count).Select(i => new KeyValuePair<TKey, TValue>(
                info.GetValue<TKey>($"{name}{_serializationNameSeperator}{_serializationNameKey}{i}"),
                info.GetValue<TValue>($"{name}{_serializationNameSeperator}{_serializationNameValue}{i}")));
        }
    }
}