using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Serialize
{
    // Classe XmlSerializer qui permet de sérialiser et désérialiser des objets en utilisant XmlSerializer
    internal class XmlSerializer<T> : Serializer<T> where T : class
    {
        // Attribut de la classe XmlSerializer
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T));
        // Constructeur de la classe XmlSerializer qui prend en paramètre la clé et le vecteur d'initialisation
        public XmlSerializer(string key, string iv = null) : base(key, iv) { }
        // Constructeur de la classe XmlSerializer qui prend en paramètre la clé et le vecteur d'initialisation
        public XmlSerializer(byte[] key, byte[] iv = null) : base(key, iv) { }
        // Méthode qui permet de sérialiser un objet
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
        // Méthode qui permet de désérialiser un objet
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
