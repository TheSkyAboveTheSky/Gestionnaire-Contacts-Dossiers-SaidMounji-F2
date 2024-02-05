using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Serialize
{
    internal class XmlSerializer<T> : Serializer<T> where T : class
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T));

        public XmlSerializer(string key, string iv = null) : base(key, iv) { }

        public XmlSerializer(byte[] key, byte[] iv = null) : base(key, iv) { }

        public override bool Serialize(T obj, string filePath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (Stream fs = new FileStream(filePath, FileMode.Create),
                            cs = new CryptoStream(fs, Aes.Create().CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    serializer.Serialize(cs, obj!);
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
                    return (T)serializer.Deserialize(cs);
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

}
