using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Serialize
{
#pragma warning disable SYSLIB0011
    internal class BinarySerializer<T> : Serializer<T> where T : class
    {
        public BinarySerializer(string key, string iv = null) : base(key, iv) { }

        public BinarySerializer(byte[] key, byte[] iv = null) : base(key, iv) { }

        public override bool Serialize(T obj, string filePath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (Stream fs = new FileStream(filePath, FileMode.Create),
                            cs = new CryptoStream(fs, Aes.Create().CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    new BinaryFormatter().Serialize(cs, obj!);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override T Deserialize(string filePath)
        {
            try
            {
                using (Stream fs = new FileStream(filePath, FileMode.Open),
                            cs = new CryptoStream(fs, Aes.Create().CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                {
                    return (T)new BinaryFormatter().Deserialize(cs);
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

#pragma warning restore SYSLIB0011
}
