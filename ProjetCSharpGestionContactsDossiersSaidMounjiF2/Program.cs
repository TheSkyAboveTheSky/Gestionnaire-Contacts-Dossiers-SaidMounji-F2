using System;
using Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Collections.Specialized.BitVector32;

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
                /*
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
                */
            };
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            while (true)
            {
                Console.Write(gestionnaire.Courant.GetPath() + "> ");
                List<string> inputList = new List<string>();
                bool escaped = false, attach = false;

                foreach (string s in Console.ReadLine().Split('"'))
                {
                    if (s != "")
                    {
                        if (escaped)
                        {
                            if (attach) inputList[inputList.Count - 1] += s;
                            else inputList.Add(s);
                        }
                        else
                        {
                            inputList.AddRange(s.Split(' ').Where(s => s != ""));
                            attach = s.LastOrDefault() != ' ';
                        }
                    }

                    escaped = !escaped;
                }

                if (!escaped)
                {
                    Console.WriteLine("Error: \" manquant");
                    continue;
                }

                if (inputList.Count == 0) continue;

                try
                {
                    Commande.Executer(inputList.ToArray(), commandes);
                }
                catch (Exception e)
                {
                    if (e.Message == "Sortir") break;
                    throw e;
                }
            }
        }

        private bool Aide(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    Console.WriteLine("Commandes disponibles :");
                    foreach (Commande commande in commandes) Console.WriteLine("  " +commande.Nom + ":\t" + commande.Description);
                    break;
                case 2:
                    foreach (Commande commande in commandes)
                    {
                        if (commande.Nom == args[1])
                        {
                            Console.WriteLine(commande.Nom + ":\t" + commande.Description);
                            Console.WriteLine(commande.Syntax);
                            return false;
                        }
                    }
                    Console.WriteLine($"Aide : la commande \"{args[1]}\" n'existe pas, tapez \"aide\" pour voir les commandes disponibles");
                    break;
                default:
                    return true;
            }
            return false;

        }
    }
}
