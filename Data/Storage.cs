using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Data.Storage
{
    [XmlInclude(typeof(Contact))]
    [XmlInclude(typeof(Dossier))]
    [Serializable]
    public abstract class Fichier
    {
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateLastModification { get; set; }

        public Fichier(string nom, DateTime creationDate, DateTime lastModificationDate)
        {
            Nom = nom;
            DateCreation = creationDate;
            DateLastModification = lastModificationDate;
        }
        public Fichier() { }
        public abstract Data.Fichier ToObject();
    }
    [Serializable]
    public class Contact : Fichier
    {
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Entreprise { get; set; }
        public Sexe Sexe { get; set; }
        public Relation Relation { get; set; }

        public Contact(string nom, DateTime dateCreation,DateTime dateLastModification, string prenom, string adresse, string telephone, string email, string entreprise,Sexe sexe ,Relation relation) : base(nom, dateCreation, dateLastModification)
        {
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Entreprise = entreprise;        
            Sexe = sexe;
            Relation = relation;
        }
        public Contact() { }
        public override Data.Fichier ToObject()
        {
            var contact = new Data.Contact(Nom, Prenom, Adresse, Telephone, Email, Entreprise, Sexe,Relation);
            contact.DateCreation = DateCreation;
            contact.DateLastModification = DateLastModification;
            return contact;
        }
    }
    [Serializable]
    public class Dossier : Fichier
    {
        public List<Fichier> Fichiers { get; set; }

        public Dossier(string nom, DateTime dateCreation, DateTime dateLastModification, List<Fichier> fichiers) : base(nom, dateCreation, dateLastModification)
        {
            Fichiers = fichiers;
        }
        public Dossier() { }
        public override Data.Fichier ToObject()
        {
            var dossier = new Data.Dossier(Nom);
            foreach (var fichier in Fichiers)
            {
                dossier.AjouterFichier(fichier.ToObject());
            }
            dossier.DateCreation = DateCreation;
            dossier.DateLastModification = DateLastModification;
            return dossier;
        }
    }
}
