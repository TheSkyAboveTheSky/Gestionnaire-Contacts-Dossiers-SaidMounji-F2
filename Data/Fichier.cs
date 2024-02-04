using System;

namespace Data
{
    public abstract class Fichier
    {
        protected string nom;
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
        public DateTime DateCreation { get;internal protected set; }
        public DateTime DateLastModification { get;internal protected set; }
        public Fichier(string nom)
        {
            this.nom = nom;
            DateCreation = DateTime.Now;
            DateLastModification = DateTime.Now;
        }
    }
}
