using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TSN.Hashing.FNV
{
    /// <summary>
    /// Provides a static class that contains some extension methods that serve as functions to calculate hash codes by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer.
    /// </summary>
    public static class HashingFNV128
    {
        static HashingFNV128()
        {
            _hashingFactor1 = BigInteger.Parse("309485009821345068724781371");
            _hashingFactor2 = BigInteger.Parse("144066263297769815596495629667062367629");
            _formatter = new BinaryFormatter();
        }


        private static readonly BigInteger _hashingFactor1;
        private static readonly BigInteger _hashingFactor2;
        private static readonly BinaryFormatter _formatter;



        private static BigInteger CreateHashCode(byte[] data, Type objectType = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            BigInteger hash = _hashingFactor1;
            if (objectType != null)
                hash = (hash * _hashingFactor2) ^ CreateHashCode(objectType.FullName.ToCharArray());
            foreach (byte b in data)
            {
                hash *= _hashingFactor2;
                hash ^= b;
            }
            return hash;
        }
        private static BigInteger CreateHashCode(char[] data, Type objectType = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            BigInteger hash = _hashingFactor1;
            if (objectType != null)
                hash = (hash * _hashingFactor2) ^ CreateHashCode(objectType.FullName.ToCharArray());
            foreach (char c in data)
            {
                hash *= _hashingFactor2;
                hash ^= c;
            }
            return hash;
        }
        private static BigInteger CreateHashCodeFromHashes(IEnumerable<BigInteger> hashCodes, Type objectType = null)
        {
            if (hashCodes == null)
                throw new ArgumentNullException(nameof(hashCodes));
            byte[] bytes = null;
            BigInteger hash = _hashingFactor1;
            if (objectType != null)
                hash = (hash * _hashingFactor2) ^ CreateHashCode(objectType.FullName.ToCharArray());
            foreach (int i in hashCodes)
            {
                bytes = BitConverter.GetBytes(i);
                hash *= _hashingFactor2;
                hash ^= (uint)CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes);
            }
            return hash;
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object if it is of any of expected types; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>). The expected object types: all general value types including <see cref="Enum"/>, besides <see cref="DateTime"/>, <see cref="TimeSpan"/>, <see cref="Type"/>, any type implemented from <see cref="IEnumerable"/> and any type implemented from <see cref="ISerializable"/>
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>Either 0 if the specified parameter is null; or the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>), if the type of specified parameter is expected; or the result of the <see cref="object.GetHashCode()"/> for the object.</returns>
        public static BigInteger GetHashCodeFNV128(this object o)
        {
            if (o == null)
                return 0;
            var objectType = o.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return o.GetHashCode();
            if (o is Enum e)
            {
                var _objectType = e.GetType();
                var _e = Convert.ChangeType(o, Enum.GetUnderlyingType(_objectType));
                if (_e is sbyte _sb)
                    return CreateHashCode(new[] { (byte)_sb }, _objectType);
                if (_e is byte _b)
                    return CreateHashCode(new[] { _b }, _objectType);
                if (_e is short _s)
                {
                    var bytes = BitConverter.GetBytes(_s);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
                if (_e is ushort _us)
                {
                    var bytes = BitConverter.GetBytes(_us);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
                if (_e is int _i)
                {
                    var bytes = BitConverter.GetBytes(_i);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
                if (_e is uint _ui)
                {
                    var bytes = BitConverter.GetBytes(_ui);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
                if (_e is long _l)
                {
                    var bytes = BitConverter.GetBytes(_l);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
                if (_e is ulong _ul)
                {
                    var bytes = BitConverter.GetBytes(_ul);
                    return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, _objectType);
                }
            }
            if (o is sbyte sb)
                return CreateHashCode(new[] { (byte)sb }, objectType);
            if (o is byte b)
                return CreateHashCode(new[] { b }, objectType);
            if (o is char c)
                return CreateHashCode(new[] { c }, objectType);
            if (o is short s)
            {
                var bytes = BitConverter.GetBytes(s);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is ushort us)
            {
                var bytes = BitConverter.GetBytes(us);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is int i)
            {
                var bytes = BitConverter.GetBytes(i);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is uint ui)
            {
                var bytes = BitConverter.GetBytes(ui);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is long l)
            {
                var bytes = BitConverter.GetBytes(l);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is ulong ul)
            {
                var bytes = BitConverter.GetBytes(ul);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is float f)
            {
                var bytes = BitConverter.GetBytes(f);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is double d)
            {
                var bytes = BitConverter.GetBytes(d);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is decimal dec)
                return CreateHashCode(decimal.GetBits(dec).SelectMany(x =>
                {
                    var __bytes = BitConverter.GetBytes(x);
                    return BitConverter.IsLittleEndian ? __bytes.Reverse().ToArray() : __bytes;
                }).ToArray(), objectType);
            if (o is string str)
                return CreateHashCode(str.ToCharArray(), objectType);
            if (o is DateTime dt)
            {
                var bytes = BitConverter.GetBytes(dt.ToBinary());
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is TimeSpan ts)
            {
                var bytes = BitConverter.GetBytes(ts.Ticks);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (o is Type t)
                return CreateHashCode(t.FullName.ToCharArray(), objectType);
            if (o is IEnumerable enumerable)
                try
                {
                    return GetHashCodeFNV128(enumerable);
                }
                catch { }
            if (objectType.IsSerializable || o is ISerializable)
                using (var ms = new MemoryStream())
                    try
                    {
                        _formatter.Serialize(ms, o);
                        return CreateHashCode(ms.ToArray(), objectType);
                    }
                    catch { }
            int hash = o.GetHashCode();
            var _bytes = BitConverter.GetBytes(hash);
            return CreateHashCode(BitConverter.IsLittleEndian ? _bytes.Reverse().ToArray() : _bytes, objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this sbyte o)
        {
            return CreateHashCode(new[] { (byte)o }, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this byte o)
        {
            return CreateHashCode(new[] { o }, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this char o)
        {
            return CreateHashCode(new[] { o }, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this short o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this ushort o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this int o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this uint o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this long o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this ulong o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this float o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this double o)
        {
            var bytes = BitConverter.GetBytes(o);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this decimal o)
        {
            var bytes = decimal.GetBits(o).SelectMany(x =>
            {
                var b = BitConverter.GetBytes(x);
                return BitConverter.IsLittleEndian ? b.Reverse().ToArray() : b;
            }).ToArray();
            return CreateHashCode(bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this string o)
        {
            return o == null ? 0 : CreateHashCode(o.ToCharArray(), o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this Enum o)
        {
            var objectType = o.GetType();
            var e = Convert.ChangeType(o, Enum.GetUnderlyingType(objectType));
            if (e is sbyte sb)
                return CreateHashCode(new[] { (byte)sb }, objectType);
            if (e is byte b)
                return CreateHashCode(new[] { b }, objectType);
            if (e is short s)
            {
                var bytes = BitConverter.GetBytes(s);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (e is ushort us)
            {
                var bytes = BitConverter.GetBytes(us);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (e is int i)
            {
                var bytes = BitConverter.GetBytes(i);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (e is uint ui)
            {
                var bytes = BitConverter.GetBytes(ui);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (e is long l)
            {
                var bytes = BitConverter.GetBytes(l);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            if (e is ulong ul)
            {
                var bytes = BitConverter.GetBytes(ul);
                return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, objectType);
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this DateTime o)
        {
            var bytes = BitConverter.GetBytes(o.ToBinary());
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>The calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this TimeSpan o)
        {
            var bytes = BitConverter.GetBytes(o.Ticks);
            return CreateHashCode(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes, o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        public static BigInteger GetHashCodeFNV128(this Type o)
        {
            return o == null ? 0 : CreateHashCode(o.FullName.ToCharArray(), o.GetType());
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="SerializationException">An error has occurred during serialization, such as if an object in the graph parameter is not marked as serializable.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public static BigInteger GetHashCodeFNV128(this ISerializable o)
        {
            if (o == null)
                return 0;
            var objectType = o.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return o.GetHashCode();
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, o);
                return CreateHashCode(ms.ToArray(), objectType);
            }
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified object; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="o">The object of which to be calculated hash code.</param>
        /// <param name="formatter">Binary formatter to serialize the specified object that is of <see cref="ISerializable"/>, to calculate its hash code.</param>
        /// <returns>0 if the specified parameter that is of <see cref="ISerializable"/> is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="formatter"/> is null.</exception>
        /// <exception cref="SerializationException">An error has occurred during serialization, such as if an object in the graph parameter is not marked as serializable.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public static BigInteger GetHashCodeFNV128(this ISerializable o, BinaryFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));
            if (o == null)
                return 0;
            var objectType = o.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return o.GetHashCode();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, o);
                return CreateHashCode(ms.ToArray(), objectType);
            }
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable sequence)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            var enumerator = sequence.GetEnumerator();
            var hashes = new List<BigInteger>(0);
            while (enumerator.MoveNext())
                hashes.Add(GetHashCodeFNV128(enumerator.Current));
            hashes.TrimExcess();
            return CreateHashCodeFromHashes(hashes, objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <typeparam name="TElement">Type of the elements of the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128<TElement>(this IEnumerable<TElement> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <typeparam name="TElement">Type of the elements of the <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="hasher">A delegate to calculate hash codes of elements of the <paramref name="sequence"/></param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="hasher"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128<TElement>(this IEnumerable<TElement> sequence, Func<TElement, BigInteger> hasher, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            if (hasher == null)
                throw new ArgumentNullException(nameof(hasher));
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => hasher(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<sbyte> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<byte> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<char> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<short> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<ushort> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<int> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<uint> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<long> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<ulong> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<float> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<double> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<decimal> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<string> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<Enum> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<DateTime> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<TimeSpan> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<Type> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        /// <exception cref="SerializationException">An error has occurred during serialization, such as if an object in the graph parameter is not marked as serializable.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<ISerializable> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="formatter">Binary formatter to serialize the specified <paramref name="sequence"/>'s inner elements that is of <see cref="ISerializable"/>, to calculate their hash codes.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="formatter"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        /// <exception cref="SerializationException">An error has occurred during serialization, such as if an object in the graph parameter is not marked as serializable.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<ISerializable> sequence, BinaryFormatter formatter, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
        /// <summary>
        /// Serves as a hash function that is to be calculated by using the Fowler/Noll/Vo algorithm which results as 128-bit unsigned integer, for the specified sequence; and returns the result as 128-bit signed integer (<see cref="BigInteger"/>).
        /// </summary>
        /// <param name="sequence">The object of which to be calculated hash code, of its inner elements.</param>
        /// <param name="typeOfSequence">Underlying type of the <paramref name="sequence"/>. For example, if an object's some inner members' hash code to be calculated at once, this parameter should be passed as that encapsulating object's type.</param>
        /// <returns>0 if the specified parameter is null, the calculated hash code as signed 128-bit integer (<see cref="BigInteger"/>).</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="sequence"/> was modified during the calculation.</exception>
        public static BigInteger GetHashCodeFNV128(this IEnumerable<IEnumerable> sequence, Type typeOfSequence = null)
        {
            if (sequence == null)
                return 0;
            var objectType = sequence.GetType();
            if (Attribute.IsDefined(objectType, typeof(NativeHashableAttribute)))
                return sequence.GetHashCode();
            return sequence == null ? 0 : CreateHashCodeFromHashes(
                sequence.Select(o => GetHashCodeFNV128(o)),
                typeOfSequence ?? objectType);
        }
    }
}