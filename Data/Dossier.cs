using System;
using System.Collections.Generic;

namespace Data
{
    public class Dossier : Fichier
    {
        private readonly List<Fichier> fichiers = new List<Fichier>();
        public new string Nom
        {
            get { return base.Nom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le Nom ne peut pas etre vide.");
                base.Nom = value;
            }
        }
        public Dossier(string nom) : base(nom) {
            Nom = nom;
        }

    }
}
