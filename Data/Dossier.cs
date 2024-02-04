using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Dossier
    {
        private readonly List<Fichier> fichiers = new List<Fichier>();
        public string Nom { get; set; }
        public Dossier(string nom)
        {
            Nom = nom;
        }
    }
}
