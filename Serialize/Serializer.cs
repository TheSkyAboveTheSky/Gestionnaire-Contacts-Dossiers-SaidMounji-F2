using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Serialize
{
    // Classe Serializer qui permet de sérialiser et désérialiser des objets
    public abstract class Serializer<T> where T : class
    {
        // Attributs de la classe Serializer
        protected byte[] iv;
        private byte[] key;
        // Propriété Key qui permet de définir la clé de chiffrement
        public byte[] Key
        {
            protected get => key;
            set { 
                key = SHA256.Create().ComputeHash(value);
            }
        }
        // Propriété IV qui permet de définir le vecteur d'initialisation
        public byte[] IV
        {
            get => iv;
            protected set => iv = value;
        }
        // Constructeur de la classe Serializer qui prend en paramètre la clé et le vecteur d'initialisation
        public Serializer(string key, string iv = null) : this(Encoding.UTF8.GetBytes("SaidMounjiSalt" + key), iv == null ? null : Encoding.UTF8.GetBytes(iv)) { }
        // Constructeur de la classe Serializer qui prend en paramètre la clé et le vecteur d'initialisation
        public Serializer(byte[] key, byte[] iv = null)
        {
            Key = key;
            IV = iv ?? MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("iv"));
        }
        // Méthode abstraite qui permet de sérialiser un objet
        public abstract bool Serialize(T obj, string filePath);
        // Méthode abstraite qui permet de désérialiser un objet
        public abstract T Deserialize(string filePath);
    }
}
