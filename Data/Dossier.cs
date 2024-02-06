using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

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
                var o = ((Dossier)Parent)?.GetDossier(value);
                if (o != null && o != this) throw new DuplicateNameException($"Un dossier portant le nom \"{value}\" existe déjà");
                DateLastModification = DateTime.Now;
                base.Nom = value;
            }
        }
        public Dossier(string nom) : base(nom) {
            Nom = nom;
        }
        // Fichiers
        public IEnumerable<Fichier> GetFichiers()
        {
            return fichiers.AsEnumerable();
        }
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
        public void SupprimerFichier(Fichier fichier)
        {
            if (fichier.Parent != this) throw new ArgumentException("Le fichier n'appartient pas a ce dossier");
            fichier.Parent = null;
            fichiers.Remove(fichier);
            DateLastModification = DateTime.Now;
        }
        // Dossiers
        public IEnumerable<Dossier> GetDossiers()
        {
            return fichiers.OfType<Dossier>();
        }
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
        public bool SupprimerDossier(string nom)
        {
            var o = (Dossier)fichiers.Find(d => d is Dossier && d.Nom == nom);
            if (o == null) return false;
            o.Parent = null;
            fichiers.Remove(o);
            DateLastModification = DateTime.Now;
            return true;
        }
        // Contacts
        public Contact GetContact(string nom)
        {
            return (Contact)fichiers.Find(c => c is Contact && c.Nom == nom);
        }
        public IEnumerable<Contact> GetContacts()
        {
            return fichiers.OfType<Contact>();
        }
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
        public void Clear()
        {
            foreach (Fichier fichier in fichiers) fichier.Parent = null;
            fichiers.Clear();
            DateLastModification = DateTime.Now;
        }
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
        public Dossier GetRoot()
        {
            Dossier dossier = this;
            while (dossier.Parent != null) dossier = (Dossier)dossier.Parent;
            return dossier;
        }
        public override void ToString(string prefix = "")
        {
            Console.Write($"{prefix}[D] {Nom}");
            Console.WriteLine($"\t(creation {DateCreation.ToString("G", CultureInfo.CurrentCulture)}) (Derniere Mise à jour {DateLastModification.ToString("G", CultureInfo.CurrentCulture)})");
            prefix += "\t";
            foreach (var c in GetContacts()) c.ToString(prefix+"\t");
            foreach (var f in fichiers.Where(fichier => fichier is not Dossier && fichier is not Contact)) f.ToString(prefix);
            foreach (var d in GetDossiers())
            {
                Console.WriteLine();
                d.ToString(prefix);
            }
        }
        public override Storage.Fichier ToStorage()
        {
            var list = new List<Storage.Fichier>();
            var storage = new Storage.Dossier(Nom, DateCreation, DateLastModification, list);
            foreach (var f in fichiers) list.Add(f.ToStorage());
            return storage;
        }

    }
}
