using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public enum Relation
    {
        Ami = 0,
        Famille,
        Collegue,
        Connaissance,
        Autre
    }
    public enum Sexe
    {
        Homme = 0,
        Femme
    }
    public class Contact
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Entreprise { get; set; }
        public Relation Relation { get; set; }
        public Sexe Sexe { get; set; }

        public Contact(string nom, string prenom, string telephone, string email, string entreprise, Relation relation, Sexe sexe)
        {
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            Email = email;
            Entreprise = entreprise;
            Relation = relation;
            Sexe = sexe;
        }
    }

}
