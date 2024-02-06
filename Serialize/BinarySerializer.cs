using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Serialize
{
#pragma warning disable SYSLIB0011
    // Classe BinarySerializer qui permet de sérialiser et désérialiser des objets en utilisant BinaryFormatter
    internal class BinarySerializer<T> : Serializer<T> where T : class
    {
        // Constructeur de la classe BinarySerializer qui prend en paramètre la clé et le vecteur d'initialisation
        public BinarySerializer(string key, string iv = null) : base(key, iv) { }
        // Constructeur de la classe BinarySerializer qui prend en paramètre la clé et le vecteur d'initialisation
        public BinarySerializer(byte[] key, byte[] iv = null) : base(key, iv) { }
        // Méthode qui permet de sérialiser un objet
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
        // Méthode qui permet de désérialiser un objet
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
