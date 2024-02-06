using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Data
{
    // Classe Dossier héritant de Fichier
    public class Dossier : Fichier
    {
        // Liste de fichiers
        private readonly List<Fichier> fichiers = new List<Fichier>();
        // Propriété publique avec la validation (Nom)
        public new string Nom
        {
            get { return base.Nom; }
            set
            {
                if (value.Contains('/') || value.Contains('\\')) throw new FormatException("Le nom du dossier ne peut pas contenir '/' ou '\\'");
                var o = ((Dossier)Parent)?.GetDossier(value);
                if (o != null && o != this) throw new DuplicateNameException($"Un dossier portant le nom \"{value}\" existe déjà");
                DateLastModification = DateTime.Now;
                base.Nom = value;
            }
        }
        // Constructeur avec le nom
        public Dossier(string nom) : base(nom) {
            Nom = nom;
        }
        // Fonctions pour les fichiers
        // GetFichiers : retourne la liste des fichiers
        public IEnumerable<Fichier> GetFichiers()
        {
            return fichiers.AsEnumerable();
        }
        // AjouterFichier : ajoute un fichier , arguments (Fichier fichier)
        public void AjouterFichier(Fichier fichier)
        {
            if (fichier is Dossier dossier && fichiers.Any(d => d is Dossier && d.Nom == dossier.Nom))
                throw new DuplicateNameException($"Un dossier avec le nom \"{dossier.Nom}\" existe deja");

            if (fichier.Parent != null)
                throw new ArgumentException("Le fichier appartient deja a un autre dossier.");
            fichier.Parent = this;
            fichiers.Add(fichier);
            DateLastModification = DateTime.Now;
        }
        // SupprimerFichier : supprime un fichier, arguments (Fichier fichier)
        public void SupprimerFichier(Fichier fichier)
        {
            if (fichier.Parent != this) throw new ArgumentException("Le fichier n'appartient pas a ce dossier");
            fichier.Parent = null;
            fichiers.Remove(fichier);
            DateLastModification = DateTime.Now;
        }
        // Fonctions pour les Dossiers
        // GetDossiers : retourne la liste des dossiers
        public IEnumerable<Dossier> GetDossiers()
        {
            return fichiers.OfType<Dossier>();
        }
        // GetDossier : retourne un dossier, arguments (string path)
        public Dossier GetDossier(string path)
        {
            string[] noms = path.Split(new char[] { '/', '\\' }, 2);
            path = noms[0];
            string subpath = noms.Length == 1 ? null : noms[1];

            Dossier dossier = null;

            switch(path)
            {
                case ".":
                    dossier = this;
                    break;
                case "":
                    dossier = this;
                    break;
                case "..":
                    dossier = (Dossier)Parent;
                    break;
                default:
                    dossier = (Dossier)fichiers.Find(d => d is Dossier && d.Nom == path);
                    break;
            }
            return subpath == null ? dossier : dossier?.GetDossier(subpath);

        }
        // SupprimerDossier : supprime un dossier, arguments (string nom)
        public bool SupprimerDossier(string nom)
        {
            var o = (Dossier)fichiers.Find(d => d is Dossier && d.Nom == nom);
            if (o == null) return false;
            o.Parent = null;
            fichiers.Remove(o);
            DateLastModification = DateTime.Now;
            return true;
        }
        // Fonctions pour les Contacts
        // GetContact : retourne un contact, arguments (string nom)
        public Contact GetContact(string nom)
        {
            return (Contact)fichiers.Find(c => c is Contact && c.Nom == nom);
        }
        // GetContacts : retourne la liste des contacts
        public IEnumerable<Contact> GetContacts()
        {
            return fichiers.OfType<Contact>();
        }
        // SupprimerContact : supprime un contact, arguments (string nom)
        public int SupprimerContact(string nom)
        {
            int nbSuppressions = 0;
            foreach (Contact contact in fichiers.Where(fichier => fichier is Contact && fichier.Nom == nom).ToArray())
            {
                contact.Parent = null;
                fichiers.Remove(contact);
                nbSuppressions++;
            }
            if (nbSuppressions > 0) DateLastModification = DateTime.Now;
            return nbSuppressions;
        }
        // Autre methodes
        // Clear : supprime tous les fichiers
        public void Clear()
        {
            foreach (Fichier fichier in fichiers) fichier.Parent = null;
            fichiers.Clear();
            DateLastModification = DateTime.Now;
        }
        // GetPath : retourne le chemin du dossier
        public string GetPath()
        {
            StringBuilder path = new StringBuilder("/");
            List<string> noms = new List<string>();
            Dossier dossier = this;
            while (dossier.Parent != null)
            {
                noms.Add(dossier.Nom);
                dossier = (Dossier)dossier.Parent;
            }
            noms.Reverse();
            path.Append(string.Join("/", noms));
            return path.ToString();
        }
        // GetRoot : retourne le dossier racine
        public Dossier GetRoot()
        {
            Dossier dossier = this;
            while (dossier.Parent != null) dossier = (Dossier)dossier.Parent;
            return dossier;
        }
        // ToString : affiche les informations du dossier
        public override void ToString(string prefix = "")
        {
            Console.Write($"{prefix}[D] {Nom}");
            Console.WriteLine($"{prefix}-Création : {DateCreation.ToString("G", CultureInfo.CurrentCulture)} - Dernière Mise à jour : {DateLastModification.ToString("G", CultureInfo.CurrentCulture)}");
            prefix += "\t";
            foreach (var c in GetContacts()) c.ToString(prefix+"\t");
            foreach (var f in fichiers.Where(fichier => fichier is not Dossier && fichier is not Contact)) f.ToString(prefix);
            foreach (var d in GetDossiers())
            {
                Console.WriteLine();
                d.ToString(prefix);
            }
        }
        // ToStorage : convertit les informations du dossier en fichier de stockage
        public override Storage.Fichier ToStorage()
        {
            var list = new List<Storage.Fichier>();
            var storage = new Storage.Dossier(Nom, DateCreation, DateLastModification, list);
            foreach (var f in fichiers) list.Add(f.ToStorage());
            return storage;
        }

    }
}
