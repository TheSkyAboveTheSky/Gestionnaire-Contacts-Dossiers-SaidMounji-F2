using Serialize;
using System;
using System.Linq;
using System.IO;
using System.Security.Principal;
using System.Text;

namespace Data
{
    public class Gestion
    {
        private Dossier courant;
        public Dossier Root { get; protected set; }
        public Dossier Courant
        {
            get { return courant; }
            set
            {
                if (value == null) throw new ArgumentNullException("Le dossier ne peut pas etre null");
                if (value.GetRoot() != Root) throw new ArgumentException("Le dossier n'appartient pas a la meme racine");
                courant = value;
            }
        }
        public int TryCount { get; private set; } = 3;
        private readonly Serializer<Storage.Fichier> serializer;
        private readonly string file;

        public Gestion(SerializerType serializerType = SerializerType.XML)
        {
            Root = new Dossier("root");
            Courant = Root;
            file = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{WindowsIdentity.GetCurrent().Name.Split('\\').Last()}.bin";
        }
        // contacts
        public void AjouterContact(string nom,string prenom,string adresse , string telephone , string email , string entreprise , Sexe sexe ,Relation relation)
        {
            courant.AjouterFichier(new Contact(nom, prenom, adresse, telephone, email, entreprise, sexe, relation));
        }
        public bool SupprimerContact(string nom)
        {
            return courant.SupprimerContact(nom) > 0;
        }
        // Dossiers
        public void AjouterDossier(string nom)
        {
            var nouveauDossier = new Dossier(nom);
            courant.AjouterFichier(nouveauDossier);
            courant = nouveauDossier;
        }
        public bool SupprimerDossier(string nom)
        {
            return courant.SupprimerDossier(nom);
        }
        // Courant
        public bool ChangerCourant(string path)
        {
            Dossier dossier = (path.Count() > 0 && "/\\".Contains(path[0])) ? Root.GetDossier(path.Substring(1)) : courant.GetDossier(path);
            if (dossier != null) courant = dossier;
            return dossier != null;
        }
        public void SetCourantAsRoot() => courant = Root;
        public void SupprimerCourant()
        {
            if (courant == Root) courant.Clear();
            else
            {
                var parent = (Dossier)courant.Parent;
                parent.SupprimerFichier(courant);
                courant = (Dossier)courant.Parent;
            }
        }
        public bool Charger(string key = null)
        {
            if (string.IsNullOrWhiteSpace(key)) key = WindowsIdentity.GetCurrent().User.Value;
            serializer.Key = Encoding.UTF8.GetBytes(key);
            var r = serializer.Deserialize(file);
            if (r != null)
            {
                TryCount = 3;
                Root = courant = (Dossier)r.ToObject();
            }
            else if (--TryCount <= 0) DeleteSave();

            return r != null;
        }
        public bool Save(string key = null)
        {
            if (string.IsNullOrWhiteSpace(key)) key = WindowsIdentity.GetCurrent().User.Value;

            serializer.Key = Encoding.UTF8.GetBytes(key);
            var r = serializer.Serialize(Root.ToStorage(), file);

            if (r) TryCount = 3;

            return r;
        }
        public bool DeleteSave()
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
