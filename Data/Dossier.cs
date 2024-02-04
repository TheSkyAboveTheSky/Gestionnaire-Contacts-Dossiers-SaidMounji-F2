using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                if (value.Contains('/') || value.Contains('\\')) throw new FormatException("Le nom du dossier ne peut pas contenir '/' ou '\\'");
                DateLastModification = DateTime.Now;
                base.Nom = value;
            }
        }
        public Dossier(string nom) : base(nom) {
            Nom = nom;
        }
        public override void ToString(string prefix = "")
        {
            Console.WriteLine(prefix + Nom);
            foreach (Fichier f in fichiers)
            {
                f.ToString(prefix + "  ");
            }
        }

    }
}
