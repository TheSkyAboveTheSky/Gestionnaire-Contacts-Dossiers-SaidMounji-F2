using System;
using Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ProjetCSharpGestionContactsDossiersSaidMounjiF2
{
    internal class Program
    {
        private readonly Gestion gestionnaire = new Gestion();
        private readonly Commande[] commandes;

        public Program()
        {
            commandes = new Commande[]
            {
                new Commande("aide", "aide [<nom de la commande>]", "Fournit de l'aide pour la commande spécifiée, ou liste les commandes si aucune n'est spécifiée.", Aide),
                new Commande("ajouterdossier", "ajouterdossier <nom>", "Ajoute un nouveau dossier au dossier actuel", AjouterDossier),
                new Commande("ajoutercontact", "ajoutercontact <nom> <prenom> <adresse> <numero de telephone> <e-mail> <entreprise> <sexe: homme|femme> <relation: ami|famille|collegue|connaissance|autre>", "Ajoute un nouveau contact au dossier actuel", AjouterContact),
                new Commande("supprimerdossier", "supprimerdossier <nom>", "Supprime un dossier par nom du dossier actuel", SupprimerDossier),
                new Commande("supprimercontact", "supprimercontact <nom>", "Supprime un contact par nom du dossier actuel", SupprimerContact),
                new Commande("modifierdossier", "modifierdossier <nom> nom=<nouveau-nom>", "Met à jour un dossier par nom du dossier actuel.", ModifierDossier),
                new Commande("modifiercontact", "modifiercontact <nom> nom=<nouveau-nom> [prenom=<nouveau-prénom>] [adresse=<nouvelle-adresse>] [telephone=<nouveau-numéro-de-téléphone>] [email=<nouvelle-adresse-e-mail>] [entreprise=<nouvelle-entreprise>] [sexe=<nouveau-sexe: homme|femme>] [relation=<nouvelle-relation: ami|famille|collegue|connaissance|autre>]", "Met à jour un contact par nom du dossier actuel.", ModifierContact),
                new Commande("afficher", "afficher [.]", "Affiche l'arborescence complète.", Afficher),
                new Commande("charger", "charger [<mot de passe>]", "Charge l'arborescence à partir d'une sauvegarde si elle existe, en utilisant le mot de passe spécifié (facultatif)", Charger),
                new Commande("enregistrer", "enregistrer [<mot de passe>]", "Enregistre l'arborescence, chiffrée en utilisant le mot de passe spécifié (facultatif)", Enregistrer),
                new Commande("cd", "cd [<chemin relatif>]", "Change le répertoire de travail actuel vers celui spécifié, ou vers la racine s'il n'y en a pas", Cd),
                new Commande("pwd", "pwd", "Affiche le chemin absolu du répertoire de travail actuel", Pwd),
                new Commande("sortir", "sortir", "Quitte le programme sans enregistrer.", Sortir),
            };
        }

        static void Main(string[] args)
        {

        }
    }
}
