using System;

namespace Data
{
    // Classe abstraite Fichier
    public abstract class Fichier
    {
        // Attribut nom
        protected string nom;
        // Définition de la propriété Nom avec un getter et un setter
        public string Nom
        {
            get { return nom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le Nom ne peut pas etre vide.");
                DateLastModification = DateTime.Now;
                nom = value;
            }
        }
        // Définition de la propriété Parent avec un getter et un setter
        public Fichier Parent { get; internal protected set; }
        // Définition de la propriété DateCreation avec un getter et un setter
        public DateTime DateCreation { get;internal protected set; }
        // Définition de la propriété DateLastModification avec un getter et un setter
        public DateTime DateLastModification { get;internal protected set; }
        // Constructeur de la classe Fichier
        public Fichier(string nom)
        {
            this.nom = nom;
            DateCreation = DateTime.Now;
            DateLastModification = DateTime.Now;
        }
        // Méthode abstraite ToString
        public abstract void ToString(string prefix="");
        // Méthode abstraite ToStorage
        public abstract Storage.Fichier ToStorage();
    }
}
