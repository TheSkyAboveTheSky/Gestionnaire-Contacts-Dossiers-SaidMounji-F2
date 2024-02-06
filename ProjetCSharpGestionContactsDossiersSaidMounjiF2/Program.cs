using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

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
                new Commande("changerdossier", "changerdossier [<chemin relatif>]", "Change le répertoire de travail actuel vers celui spécifié, ou vers la racine s'il n'y en a pas", ChangerDossier),
                new Commande("afficherchemin", "afficherchemin", "Affiche le chemin absolu du répertoire de travail actuel", AfficherChemin),
                new Commande("sortir", "sortir", "Quitte le programme sans enregistrer.", Sortir),
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
                    Console.WriteLine("Aide: Commandes disponibles :");
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
                    Console.WriteLine($"Aide: la commande \"{args[1]}\" n'existe pas, tapez \"aide\" pour voir les commandes disponibles");
                    break;
                default:
                    return true;
            }
            return false;

        }

        private bool AjouterDossier(string[] args)
        {
            if (args.Length != 2) return true;
            try
            {
                var nomDossier = args[1];
                gestionnaire.AjouterDossier(nomDossier);
                Console.WriteLine($"AjouterDossier: Dossier \"{nomDossier}\" ajouté avec succès.");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"AjouterDossier: {e.Message}");
            }
            catch (DuplicateNameException e)
            {
                Console.WriteLine($"AjouterDossier: {e.Message}");
            }
            return false;
        }

        private bool AjouterContact(string[] args)
        {
            if (args.Length != 9)
            {
                return true;
            }

            try
            {
                var nom = args[1];
                var prenom = args[2];
                var adresse = args[3];
                var telephone = args[4];
                var email = args[5];
                var entreprise = args[6];
                var sexe = args[7];
                var relation = args[8];

                if (!Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(relation), out Relation relationEnum))
                {
                    Console.WriteLine("AjouterContact: relation invalide, tapez \"aide ajoutercontact\" pour voir les relations disponibles");
                    return false;
                }

                if (!Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sexe), out Sexe sexeEnum))
                {
                    Console.WriteLine("AjouterContact: sexe invalide, tapez \"aide ajoutercontact\" pour voir les sexes disponibles");
                    return false;
                }

                gestionnaire.AjouterContact(nom, prenom, adresse, telephone, email, entreprise, sexeEnum, relationEnum);
                Console.WriteLine($"AjouterContact: Contact \"{nom}\" ajouté avec succès.");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"AjouterContact: {e.Message}");
            }
            catch (DuplicateNameException e)
            {
                Console.WriteLine($"AjouterContact: {e.Message}");
            }
            return false;
        }

        private bool SupprimerDossier(string[] args)
        {
            if (args.Length != 2) return true;
            try
            {
                var nomDossier = args[1];
                if (nomDossier == ".")
                {
                    gestionnaire.SupprimerCourant();
                    Console.WriteLine("SupprimerDossier: Dossier courant supprimé avec succès.");
                }
                else
                {
                    if (gestionnaire.SupprimerDossier(nomDossier))
                    {
                        Console.WriteLine($"SupprimerDossier: Dossier \"{nomDossier}\" supprimé avec succès.");
                    }
                    else
                    {
                        Console.WriteLine($"SupprimerDossier: Le dossier \"{nomDossier}\" n'existe pas.");
                    }
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine($"SupprimerDossier: {e.Message}");
            }
            catch (DuplicateNameException e)
            {
                Console.WriteLine($"SupprimerDossier: {e.Message}");
            }
            return false;
        }

        private bool SupprimerContact(string[] args)
        {
            if (args.Length != 2) return true;
            try
            {
                var nomContact = args[1];
                if (gestionnaire.SupprimerContact(nomContact))
                {
                    Console.WriteLine($"SupprimerContact: Contact \"{nomContact}\" supprimé avec succès.");
                }
                else
                {
                    Console.WriteLine($"SupprimerContact: Le contact \"{nomContact}\" n'existe pas.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"SupprimerContact: {e.Message}");
            }
            return false;
        }

        private bool Afficher(string[] args)
        {
            if (args.Length > 2) return true;
            if (args.Length == 2)
            {
                if (args[1] != ".") return true;
                gestionnaire.Courant.ToString();
            }
            else gestionnaire.Root.ToString();

            return false;
        }

        private bool ChangerDossier(string[] args)
        {
            if (args.Length > 2) return true;
            var chemin = args.Length == 1 ? "" : args[1];
            if (args.Length == 1)
            {
                gestionnaire.SetCourantAsRoot();
            }
            else if (!gestionnaire.ChangerCourant(chemin))
            {
                Console.WriteLine($"ChangerDossier: Le dossier \"{chemin}\" n'existe pas.");
            }
            return false;
        }

        private bool AfficherChemin(string[] args)
        {
            if (args.Length != 1) return true;
            Console.WriteLine($"AfficherChemin: {gestionnaire.Courant.GetPath()}");
            return false;
        }

        private bool Sortir(string[] args)
        {
            if (args.Length != 1) return true;
            throw new Exception("Sortir");
        }

        private bool ModifierDossier(string[] args)
        {
            if (args.Length != 3) return true;
            try
            {
                var nomDossier = args[1];
                var nomChanger = args[2].Split('=')[0];
                var nouveauNom = args[2].Split('=')[1];
                if(!nomChanger.Equals("nom", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("ModifierDossier: syntaxe invalide, tapez \"aide modifierdossier\" pour voir la syntaxe correcte");
                    return true;
                }
                var dossier = gestionnaire.Courant.GetDossier(nomDossier);
                if (dossier == null)
                {
                    Console.WriteLine($"ModifierDossier: Le dossier \"{nomDossier}\" n'existe pas.");
                }
                else
                {
                    dossier.Nom = nouveauNom;
                    Console.WriteLine($"ModifierDossier: Le nom du dossier a été modifié avec succès de \"{nomDossier}\" à \"{dossier.Nom}\".");
                }

            }
            catch (FormatException e)
            {
                Console.WriteLine($"ModifierDossier: {e.Message}");
            }
            catch (DuplicateNameException e)
            {
                Console.WriteLine($"ModifierDossier: {e.Message}");
            }
            return false;
        }

        private bool ModifierContact(string[] args)
        {
            if (args.Length < 3) return true;
            else
            {
                var nomContact = args[1];
                var contact = gestionnaire.Courant.GetContact(nomContact);
                if (contact == null)
                {
                    Console.WriteLine($"ModifierContact: Le contact \"{nomContact}\" n'existe pas.");
                }
                else
                {
                    for(int i=2;i<args.Length; i++)
                    {
                        if (!args[i].Contains("="))
                        {
                            Console.WriteLine("ModifierContact: syntaxe invalide, tapez \"aide modifiercontact\" pour voir la syntaxe correcte");
                            return false;
                        }
                        string[] parameters = args[i].Split(new char[] { '=' }, 2);
                        string parameter = parameters[0];
                        string value = parameters[1];
                        switch(parameter.ToLower())
                        {
                            case "nom":
                                try
                                {
                                    contact.Nom = value;
                                    Console.WriteLine($"ModifierContact: Le nom du contact  \"{nomContact}\"a été modifié avec succès.");
                                }catch(ArgumentException e)
                                {
                                    Console.WriteLine($"ModifierContact: {e.Message}");
                                }
                                break;
                            case "prenom":
                                try
                                {
                                    contact.Prenom = value;
                                    Console.WriteLine($"ModifierContact: Le prénom du contact \"{nomContact}\" a été modifié avec succès.");
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine($"ModifierContact: {e.Message}");
                                }
                                break;
                            case "adresse":
                                try
                                {
                                    contact.Adresse = value;
                                    Console.WriteLine($"ModifierContact: L'adresse du contact \"{nomContact}\" a été modifiée avec succès.");
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine($"ModifierContact: {e.Message}");
                                }
                                break;
                            case "telephone":
                                try
                                {
                                    contact.Telephone = value;
                                    Console.WriteLine($"ModifierContact: Le numéro de téléphone du contact \"{nomContact}\" a été modifié avec succès.");
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine($"ModifierContact: {e.Message}");
                                }
                                break;
                            case "email":
                                try
                                {
                                    contact.Email = value;
                                    Console.WriteLine($"ModifierContact: L'adresse e-mail du contact \"{nomContact}\" a été modifiée avec succès.");
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine($"ModifierContact: L'adresse e-mail \"{value}\" est invalide.");
                                }
                                break;
                            case "entreprise":
                                try
                                {
                                    contact.Entreprise = value;
                                    Console.WriteLine($"ModifierContact: L'entreprise du contact \"{nomContact}\" a été modifiée avec succès.");
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine($"ModifierContact: {e.Message}");
                                }
                                break;
                            case "sexe":
                                if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value), out Sexe sexe))
                                {
                                    contact.Sexe = sexe;
                                    Console.WriteLine($"ModifierContact: Le sexe du contact \"{nomContact}\" a été modifié avec succès.");
                                }
                                else Console.WriteLine($"ModifierContact: sexe invalide, tapez \"aide modifiercontact\" pour voir la syntaxe.");
                                break;
                            case "relation":
                                if (Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value), out Relation relation))
                                {
                                    contact.Relation = relation;
                                    Console.WriteLine($"ModifierContact: La relation du contact \"{nomContact}\" a été modifiée avec succès.");
                                }
                                else Console.WriteLine($"ModifierContact: relation invalide, tapez \"aide modifiercontact\" pour voir la syntaxe.");
                                break;
                            default:
                                Console.WriteLine($"ModifierContact: paramètre \"{parameter}\" invalide, tapez \"aide modifiercontact\" pour voir la syntaxe.");
                                break;
                        }
                    }
                }
            }
            return false;
        }

        private bool Charger(string[] args)
        {
            if (args.Length > 2) return true;
            string key = args.Length == 2 ? args[1] : null;
            try
            {
                if(gestionnaire.Charger(key))
                {
                    Console.WriteLine($"Charger: L'arborescence a été chargée avec succès.");
                }
                else if (gestionnaire.TryCount <=0) Console.WriteLine("$Charger: trop de mots de passe invalides : suppression de la base de données...");
                else Console.WriteLine($"Charger: échec du chargement : mot de passe invalide, \"{gestionnaire.TryCount}\"/3 tentatives restantes ");
            }catch(FileNotFoundException e)
            {
                Console.WriteLine($"Charger: Aucune sauvegarde n'a été trouvée.");
            }catch(Exception e)
            {
                Console.WriteLine($"Charger: {e.Message}");
            }
            return false;
        }

        private bool Enregistrer(string[] args)
        {
            if (args.Length > 2) return true;
            Console.WriteLine($"Enregistrer: Enregistrement du fichier GestionnaireContactsDossiers.db...");
            string key = args.Length == 2 ? args[1] : null;
            try
            {
                gestionnaire.Enregistrer(key);
                Console.WriteLine($"Enregistrer: L'arboresce a été enregistrée avec succès dans C:\\Users\\Geek Store\\Documents\\GestionnaireContactsDossiers.db.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Enregistrer: {e.Message}");
            }
            return false;
        }
    }
}
