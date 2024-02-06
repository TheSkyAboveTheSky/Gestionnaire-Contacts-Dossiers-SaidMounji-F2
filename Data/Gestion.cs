using Serialize;
using System;
using System.Linq;
using System.IO;
using System.Security.Principal;
using System.Text;

namespace Data
{
    // Classe Gestion qui gère les dossiers et les contacts
    public class Gestion
    {
        // Attribut courant de type Dossier qui représente le dossier courant
        private Dossier courant;
        // Propriété Root avec un getter et un setter de type Dossier qui représente la racine
        public Dossier Root { get; protected set; }
        // Propriété Courant avec un getter et un setter de type Dossier qui représente le dossier courant
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
        // Propriété TryCount avec un getter et un setter de type int qui représente le nombre d'essais
        public int TryCount { get; private set; } = 3;
        // Attribut serializer de type Serializer<Storage.Fichier> qui représente le sérialiseur de type Storage.Fichier
        private readonly Serializer<Storage.Fichier> serializer;
        // Attribut file de type string qui représente le fichier
        private readonly string file;
        // Constructeur de la classe Gestion avec un paramètre serializerType de type SerializerType qui représente le type de sérialiseur
        public Gestion(SerializerType serializerType = SerializerType.XML)
        {
            serializer = SerializerFactory.GetSerializer<Storage.Fichier>(serializerType, WindowsIdentity.GetCurrent().User.Value);
            Root = new Dossier("root");
            Courant = Root;
            file = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\GestionnaireContactsDossiers.db";
        }
        // contacts
        // Méthode AjouterContact avec des paramètres nom, prenom, adresse, telephone, email, entreprise, sexe et relation de type string, Sexe et Relation qui ajoute un contact
        public void AjouterContact(string nom,string prenom,string adresse , string telephone , string email , string entreprise , Sexe sexe ,Relation relation)
        {
            courant.AjouterFichier(new Contact(nom, prenom, adresse, telephone, email, entreprise, sexe, relation));
        }
        // SupprimerContact avec un paramètre nom de type string qui supprime un contact
        public bool SupprimerContact(string nom)
        {
            return courant.SupprimerContact(nom) > 0;
        }
        // Dossiers
        // Méthode AjouterDossier avec un paramètre nom de type string qui ajoute un dossier
        public void AjouterDossier(string nom)
        {
            var nouveauDossier = new Dossier(nom);
            courant.AjouterFichier(nouveauDossier);
            courant = nouveauDossier;
        }
        // Méthode SupprimerDossier avec un paramètre nom de type string qui supprime un dossier
        public bool SupprimerDossier(string nom)
        {
            return courant.SupprimerDossier(nom);
        }
        // Courant
        // Méthode ChangerCourant avec un paramètre path de type string qui change le dossier courant
        public bool ChangerCourant(string path)
        {
            Dossier dossier = (path.Count() > 0 && "/\\".Contains(path[0])) ? Root.GetDossier(path.Substring(1)) : courant.GetDossier(path);
            if (dossier != null) courant = dossier;
            return dossier != null;
        }
        // Méthode SetCourantAsRoot qui met le dossier courant comme racine
        public void SetCourantAsRoot() => courant = Root;
        // Méthode SupprimerCourant qui supprime le dossier courant
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
        // Charger avec un paramètre key de type string qui charge le fichier
        public bool Charger(string key = null)
        {
            if (string.IsNullOrWhiteSpace(key)) key = WindowsIdentity.GetCurrent().User.Value;
            serializer.Key = Encoding.UTF8.GetBytes(key);
            var r = serializer.Deserialize(file);
            if (r != null)
            {
                TryCount = 3;
                Root = courant = (Dossier)r.ToObject();
            }else if (--TryCount <= 0) { Decharger(); }
            return r!= null;
        }
        // Enregistrer avec un paramètre key de type string qui enregistre le fichier
        public bool Enregistrer(string key = null)
        {
            if (string.IsNullOrWhiteSpace(key)) key = WindowsIdentity.GetCurrent().User.Value;

            serializer.Key = Encoding.UTF8.GetBytes(key);
            var r = serializer.Serialize(Root.ToStorage(), file);

            if (r) TryCount = 3;

            return r;
        }
        // Decharger qui décharge le fichier
        public bool Decharger()
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
