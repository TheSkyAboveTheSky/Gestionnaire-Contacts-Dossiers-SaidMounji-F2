using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Serialize
{
    public abstract class Serializer<T> where T : class

    {
        protected byte[] iv;
        private byte[] key;

        public byte[] Key
        {
            protected get => key; 
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                key = SHA256.Create().ComputeHash(value);
            }
        }
        public byte[] IV
        {
            get => iv;
            protected set => iv = value;
        }
        public Serializer(string key, string iv = null) : this(Encoding.UTF8.GetBytes("SaidMounjiSalt" + key), iv == null ? null : Encoding.UTF8.GetBytes(iv)) { }

        public Serializer(byte[] key, byte[] iv = null)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length == 0)
                throw new ArgumentException("La Cle ne peut pas etre vide.", nameof(key));

            Key = key;
            IV = iv ?? MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("iv"));
        }


        public abstract bool Serialize(T obj, string filePath);

        public abstract T Deserialize(string filePath);
    }
}
