using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Fichier
    {
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateLastModification { get; set; }
        public Fichier(string nom)
        {
            Nom = nom;
            DateCreation = DateTime.Now;
            DateLastModification = DateTime.Now;
        }
    }
}
