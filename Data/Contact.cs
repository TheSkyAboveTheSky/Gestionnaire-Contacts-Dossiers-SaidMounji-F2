using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
    public class Contact : Fichier
    {
        private string prenom;
        private string adresse;
        private string telephone;
        private string email;
        private string entreprise;
        private Sexe sexe;
        private Relation relation;

        public string Prenom
        {
            get { return prenom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le prenom ne peut pas etre vide.");
                DateLastModification = DateTime.Now;
                prenom = value;
            }
        }
        public string Adresse
        {
            get { return adresse; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("L'Adresse ne peut pas etre vide.");
                DateLastModification = DateTime.Now;
                adresse = value;
            }
        }
        public string Telephone
        {
            get { return telephone; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le Numero de Telephone ne peut pas etre vide.");
                DateLastModification = DateTime.Now;
                telephone = value;
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (!new EmailAddressAttribute().IsValid(value)) throw new FormatException($"\"{value}\" n'est pas une adresse e-mail valide."); ;
                DateLastModification = DateTime.Now;
                email = value;
            }
        }
        public string Entreprise
        {
            get { return entreprise; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Le Nom de l'Entreprise ne peut pas etre vide.");
                DateLastModification = DateTime.Now;
                entreprise = value;
            }
        }
        public Sexe Sexe
        {
            get { return sexe; }
            set
            {
                DateLastModification = DateTime.Now;
                sexe = value;
            }
        }
        public Relation Relation
        {
            get { return relation; }
            set
            {
                DateLastModification = DateTime.Now;
                relation = value;
            }
        }
        public Contact(string nom, string prenom, string adresse, string telephone, string email, string entreprise, Sexe sexe, Relation relation) : base(nom)
        {
            this.prenom = prenom;
            this.adresse = adresse;
            this.telephone = telephone;
            this.email = email;
            this.entreprise = entreprise;
            this.sexe = sexe;
            this.relation = relation;
        }
        public override void ToString(string prefix = "")
        {
            Console.WriteLine($"{prefix} [C] Contact {Nom} {Prenom} :");
            prefix += "\t";
            Console.WriteLine($"{prefix}-Adresse : {Adresse}");
            Console.WriteLine($"{prefix}-Telephone : {Telephone}");
            Console.WriteLine($"{prefix}-Email : {Email}");
            Console.WriteLine($"{prefix}-Entreprise : {Entreprise}");
            Console.WriteLine($"{prefix}-Sexe : {Sexe.ToString()}");
            Console.WriteLine($"{prefix}-Relation : {Relation.ToString()}");
            Console.WriteLine($"{prefix}-Creation {DateCreation.ToString("G", CultureInfo.CurrentCulture)}");
            Console.WriteLine($"{prefix}-Derniere Mise à jour {DateLastModification.ToString("G", CultureInfo.CurrentCulture)}");

        }
        public override Storage.Fichier ToStorage()
        {
            return new Storage.Contact(Nom, DateCreation, DateLastModification, Prenom, Adresse, Telephone, Email, Entreprise, Sexe,Relation);
        }
    }

}
