using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Data.Storage
{
    [XmlInclude(typeof(Contact))]
    [XmlInclude(typeof(Dossier))]
    [Serializable]
    // Classe abstraite pour les fichiers
    public abstract class Fichier
    {
        // Nom du fichier
        public string Nom { get; set; }
        // Date de création
        public DateTime DateCreation { get; set; }
        // Date de dernière modification
        public DateTime DateLastModification { get; set; }
        // Constructeur de la classe Fichier avec des paramètres nom, creationDate et lastModificationDate de type string et DateTime qui représente le nom, la date de création et la date de dernière modification
        public Fichier(string nom, DateTime creationDate, DateTime lastModificationDate)
        {
            Nom = nom;
            DateCreation = creationDate;
            DateLastModification = lastModificationDate;
        }
        // Constructeur vide de la classe Fichier
        public Fichier() { }
        // Méthode ToObject qui retourne un objet de type Data.Fichier
        public abstract Data.Fichier ToObject();
    }
    [Serializable]
    // Classe Contact qui hérite de la classe Fichier
    public class Contact : Fichier
    {
        // Prénom du contact de type string avec un getter et un setter
        public string Prenom { get; set; }
        // Adresse du contact de type string avec un getter et un setter
        public string Adresse { get; set; }
        // Téléphone du contact de type string avec un getter et un setter
        public string Telephone { get; set; }
        // Email du contact de type string avec un getter et un setter
        public string Email { get; set; }
        // Entreprise du contact de type string avec un getter et un setter
        public string Entreprise { get; set; }
        // Sexe du contact de type Sexe avec un getter et un setter
        public Sexe Sexe { get; set; }
        // Relation du contact de type Relation avec un getter et un setter
        public Relation Relation { get; set; }
        // Constructeur de la classe Contact avec des paramètres nom, dateCreation, dateLastModification, prenom, adresse, telephone, email, entreprise, sexe et relation de type string, DateTime, Sexe et Relation qui représente le nom, la date de création, la date de dernière modification, le prénom, l'adresse, le téléphone, l'email, l'entreprise, le sexe et la relation
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
        // Constructeur vide de la classe Contact
        public Contact() { }
        // Méthode ToObject qui retourne un objet de type Data.Fichier
        public override Data.Fichier ToObject()
        {
            var contact = new Data.Contact(Nom, Prenom, Adresse, Telephone, Email, Entreprise, Sexe,Relation);
            contact.DateCreation = DateCreation;
            contact.DateLastModification = DateLastModification;
            return contact;
        }
    }
    [Serializable]
    // Classe Dossier qui hérite de la classe Fichier
    public class Dossier : Fichier
    {
        // Liste de fichiers de type Fichier avec un getter et un setter
        public List<Fichier> Fichiers { get; set; }
        // Constructeur de la classe Dossier avec des paramètres nom, dateCreation, dateLastModification et fichiers de type string, DateTime et List<Fichier> qui représente le nom, la date de création, la date de dernière modification et la liste de fichiers
        public Dossier(string nom, DateTime dateCreation, DateTime dateLastModification, List<Fichier> fichiers) : base(nom, dateCreation, dateLastModification)
        {
            Fichiers = fichiers;
        }
        // Constructeur vide de la classe Dossier
        public Dossier() { }
        // Méthode ToObject qui retourne un objet de type Data.Fichier
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
