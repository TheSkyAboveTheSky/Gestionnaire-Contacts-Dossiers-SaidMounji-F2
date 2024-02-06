using System;

namespace Serialize
{
    // Enumération SerializerType qui permet de définir les types de sérialiseurs
    public enum SerializerType
    {
        XML,
        Binary,
    }
    // Classe SerializerFactory qui permet de créer des instances de sérialiseurs
    public static class SerializerFactory
    {
        // Méthode GetSerializer qui permet de créer une instance de sérialiseur
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
