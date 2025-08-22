using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TSN.EncryptDecrypt.WindowsForms
{
    public static class AesHmac
    {
        static AesHmac()
        {
            _random = RandomNumberGenerator.Create();
        }


        public const int BlockBitSize = 128;
        public const int KeyBitSize = 256;
        public const int SaltBitSize = 64;
        public const int Iterations = 10000;
        public const int MinPasswordLength = 12;
        private const int _length1 = KeyBitSize / 8;
        private const int _length2 = SaltBitSize / 8;
        private const int _length2Double = _length2 * 2;

        private static readonly RandomNumberGenerator _random;



        private static string ThrowIfNullOrEmptyAndReturn(string s, string name) => (s ?? throw new ArgumentNullException(name)).Equals(string.Empty) ? throw new ArgumentOutOfRangeException(name) : s;

        public static byte[] NewKey()
        {
            var key = new byte[KeyBitSize / 8];
            _random.GetBytes(key);
            return key;
        }
        public static string SimpleEncrypt(string secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null) => Convert.ToBase64String(SimpleEncrypt(Encoding.UTF8.GetBytes(ThrowIfNullOrEmptyAndReturn(secretMessage, nameof(secretMessage))), cryptKey, authKey, nonSecretPayload));
        public static string SimpleDecrypt(string encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            var plainText = SimpleDecrypt(Convert.FromBase64String(ThrowIfNullOrEmptyAndReturn(encryptedMessage?.Trim(), nameof(encryptedMessage))), cryptKey, authKey, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }
        public static string SimpleEncryptWithPassword(string secretMessage, string password, byte[] nonSecretPayload = null) => Convert.ToBase64String(SimpleEncryptWithPassword(Encoding.UTF8.GetBytes(ThrowIfNullOrEmptyAndReturn(secretMessage, nameof(secretMessage))), password, nonSecretPayload));
        public static string SimpleDecryptWithPassword(string encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            var plainText = SimpleDecryptWithPassword(Convert.FromBase64String(ThrowIfNullOrEmptyAndReturn(encryptedMessage?.Trim(), nameof(encryptedMessage))), password, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }
        public static byte[] SimpleEncrypt(byte[] secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            if ((cryptKey ?? throw new ArgumentNullException(nameof(cryptKey))).Length != _length1)
                throw new ArgumentOutOfRangeException(nameof(cryptKey));
            if ((authKey ?? throw new ArgumentNullException(nameof(authKey))).Length != _length1)
                throw new ArgumentOutOfRangeException(nameof(authKey));
            if ((secretMessage ?? throw new ArgumentNullException(nameof(authKey))).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(secretMessage));
            nonSecretPayload = nonSecretPayload ?? new byte[] { };
            byte[] cipherText;
            byte[] iv;
            using (var aes = new AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {
                aes.GenerateIV();
                iv = aes.IV;
                using (var encrypter = aes.CreateEncryptor(cryptKey, iv))
                using (var cipherStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                        binaryWriter.Write(secretMessage);
                    cipherText = cipherStream.ToArray();
                }
            }
            using (var hmac = new HMACSHA256(authKey))
            using (var encryptedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(encryptedStream))
                {
                    binaryWriter.Write(nonSecretPayload);
                    binaryWriter.Write(iv);
                    binaryWriter.Write(cipherText);
                    binaryWriter.Flush();
                    var tag = hmac.ComputeHash(encryptedStream.ToArray());
                    binaryWriter.Write(tag);
                }
                return encryptedStream.ToArray();
            }
        }
        public static byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            if ((cryptKey ?? throw new ArgumentNullException(nameof(cryptKey))).Length != _length1)
                throw new ArgumentOutOfRangeException(nameof(cryptKey));
            if ((authKey ?? throw new ArgumentNullException(nameof(authKey))).Length != _length1)
                throw new ArgumentOutOfRangeException(nameof(authKey));
            if ((encryptedMessage ?? throw new ArgumentNullException(nameof(authKey))).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(encryptedMessage));
            using (var hmac = new HMACSHA256(authKey))
            {
                var sentTag = new byte[hmac.HashSize / 8];
                var calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length);
                var ivLength = BlockBitSize / 8;
                if (encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength)
                    return null;
                Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length);
                var compare = 0;
                for (var i = 0; i < sentTag.Length; i++)
                    compare |= sentTag[i] ^ calcTag[i];
                if (compare != 0)
                    return null;
                using (var aes = new AesManaged
                {
                    KeySize = KeyBitSize,
                    BlockSize = BlockBitSize,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                })
                {
                    var iv = new byte[ivLength];
                    Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length);
                    using (var decrypter = aes.CreateDecryptor(cryptKey, iv))
                    using (var plainTextStream = new MemoryStream())
                    {
                        using (var decrypterStream = new CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write))
                        using (var binaryWriter = new BinaryWriter(decrypterStream))
                            binaryWriter.Write(
                                encryptedMessage,
                                nonSecretPayloadLength + iv.Length,
                                encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length
                            );
                        return plainTextStream.ToArray();
                    }
                }
            }
        }
        public static byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
        {
            if ((password ?? throw new ArgumentNullException(nameof(password))).Trim().Equals(string.Empty) || password.Length < MinPasswordLength)
                throw new ArgumentOutOfRangeException(nameof(password));
            if ((secretMessage ?? throw new ArgumentNullException(nameof(secretMessage))).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(secretMessage));
            nonSecretPayload = nonSecretPayload ?? new byte[] { };
            var payload = new byte[_length2Double + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            int payloadIndex = nonSecretPayload.Length;
            byte[] cryptKey;
            byte[] authKey;
            using (var generator = new Rfc2898DeriveBytes(password, _length2, Iterations))
            {
                var salt = generator.Salt;
                cryptKey = generator.GetBytes(_length1);
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                payloadIndex += salt.Length;
            }
            using (var generator = new Rfc2898DeriveBytes(password, _length2, Iterations))
            {
                var salt = generator.Salt;
                authKey = generator.GetBytes(_length1);
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
            }
            return SimpleEncrypt(secretMessage, cryptKey, authKey, payload);
        }
        public static byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            if ((password ?? throw new ArgumentNullException(nameof(password))).Trim().Equals(string.Empty) || password.Length < MinPasswordLength)
                throw new ArgumentOutOfRangeException(nameof(password));
            if ((encryptedMessage ?? throw new ArgumentNullException(nameof(encryptedMessage))).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(encryptedMessage));
            var cryptSalt = new byte[_length2];
            var authSalt = new byte[_length2];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length);
            Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length);
            byte[] cryptKey;
            byte[] authKey;
            using (var generator = new Rfc2898DeriveBytes(password, cryptSalt, Iterations))
                cryptKey = generator.GetBytes(_length1);
            using (var generator = new Rfc2898DeriveBytes(password, authSalt, Iterations))
                authKey = generator.GetBytes(_length1);
            return SimpleDecrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength);
        }
    }
}