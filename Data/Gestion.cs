using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Gestion()
        {
            Root = new Dossier("root");
            Courant = Root;
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
        public void SupprimerDossier(string nom)
        {
            courant.SupprimerDossier(nom);
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
    }
}
