using System;

namespace Serialize
{
    public enum SerializerType
    {
        XML,
        Binary,
    }

    public static class SerializerFactory
    {
        public static Serializer<T> GetSerializer<T>(SerializerType type, string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("La cle ne peut pas etre vide ou nulle", nameof(key));
            }

            switch (type)
            {
                case SerializerType.XML:
                    return new XmlSerializer<T>(key);
                case SerializerType.Binary:
                    return new BinarySerializer<T>(key);
                default:
                    throw new ArgumentException("Type de serialiseur invalide", nameof(type));
            }
        }
    }

}
