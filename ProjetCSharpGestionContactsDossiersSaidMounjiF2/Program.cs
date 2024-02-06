using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ProjetCSharpGestionContactsDossiersSaidMounjiF2
{
    // Classe Program
    internal class Program
    {
        // Propriété privée (gestionnaire) de type Gestion
        private readonly Gestion gestionnaire = new Gestion();
        // Propriété privée (commandes) de type Commande[]
        private readonly Commande[] commandes;

        public Program()
        {
            // Initialisation de la propriété commandes avec les commandes disponibles
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
        // Méthode privée Run pour exécuter le programme
        private void Run()
        {
            // Boucle infinie pour lire les commandes de l'utilisateur
            while (true)
            {
                // Affichage du chemin actuel
                Console.Write(gestionnaire.Courant.GetPath() + "> ");
                // Lecture de la commande de l'utilisateur
                List<string> inputList = new List<string>();
                // Variables locales (escaped, attach) de type booléen
                bool escaped = false, attach = false;
                // Boucle foreach pour lire les commandes de l'utilisateur
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
        // Méthode privée Aide pour afficher l'aide
        private bool Aide(string[] args)
        {
            // Switch pour afficher l'aide selon le nombre d'arguments
            switch (args.Length)
            {
                // Cas 1 : afficher les commandes disponibles
                case 1:
                    Console.WriteLine("Aide: Commandes disponibles :");
                    foreach (Commande commande in commandes) Console.WriteLine("  " +commande.Nom + ":\t" + commande.Description);
                    break;
                // Cas 2 : afficher l'aide pour une commande spécifique
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
        // Méthode privée AjouterDossier pour ajouter un dossier à l'arborescence actuelle avec paramètre args de type string[]
        private bool AjouterDossier(string[] args)
        {
            // Si le nombre d'arguments est différent de 2, retourner vrai
            if (args.Length != 2) return true;
            try
            {
                // Variable locale nomDossier de type string
                var nomDossier = args[1];
                // Appel de la méthode AjouterDossier de la propriété gestionnaire avec paramètre nomDossier
                gestionnaire.AjouterDossier(nomDossier);
                // Affichage du message de succès
                Console.WriteLine($"AjouterDossier: Dossier \"{nomDossier}\" ajouté avec succès.");
            }
            // Gestion des exceptions
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
        // Méthode privée AjouterContact pour ajouter un contact à l'arborescence actuelle avec paramètre args de type string[]
        private bool AjouterContact(string[] args)
        {
            // Si le nombre d'arguments est différent de 9, retourner vrai
            if (args.Length != 9)
            {
                return true;
            }
            try
            {
                // Variables locales nom, prenom, adresse, telephone, email, entreprise, sexe, relation
                var nom = args[1];
                var prenom = args[2];
                var adresse = args[3];
                var telephone = args[4];
                var email = args[5];
                var entreprise = args[6];
                var sexe = args[7];
                var relation = args[8];
                // Si le sexe ou la relation n'est pas valide, afficher un message d'erreur
                if (!Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(relation), out Relation relationEnum))
                {
                    Console.WriteLine("AjouterContact: relation invalide, tapez \"aide ajoutercontact\" pour voir les relations disponibles");
                    return false;
                }
                // Si le sexe ou la relation n'est pas valide, afficher un message d'erreur
                if (!Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sexe), out Sexe sexeEnum))
                {
                    Console.WriteLine("AjouterContact: sexe invalide, tapez \"aide ajoutercontact\" pour voir les sexes disponibles");
                    return false;
                }
                // Appel de la méthode AjouterContact de la propriété gestionnaire avec paramètres nom, prenom, adresse, telephone, email, entreprise, sexeEnum, relationEnum
                gestionnaire.AjouterContact(nom, prenom, adresse, telephone, email, entreprise, sexeEnum, relationEnum);
                // Affichage du message de succès
                Console.WriteLine($"AjouterContact: Contact \"{nom}\" ajouté avec succès.");
            }
            // Gestion des exceptions
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
        // Méthode privée SupprimerDossier pour supprimer un dossier de l'arborescence actuelle avec paramètre args de type string[]
        private bool SupprimerDossier(string[] args)
        {
            // Si le nombre d'arguments est différent de 2, retourner vrai
            if (args.Length != 2) return true;
            try
            {
                // Variable locale nomDossier de type string
                var nomDossier = args[1];
                // Si le nom du dossier est égal à ".", supprimer le dossier courant
                if (nomDossier == ".")
                {
                    // Appel de la méthode SupprimerCourant de la propriété gestionnaire
                    gestionnaire.SupprimerCourant();
                    // Affichage du message de succès
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
            // Gestion des exceptions
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
        // Méthode privée SupprimerContact pour supprimer un contact de l'arborescence actuelle avec paramètre args de type string[]
        private bool SupprimerContact(string[] args)
        {
            // Si le nombre d'arguments est différent de 2, retourner vrai
            if (args.Length != 2) return true;
            try
            {
                // Variable locale nomContact de type string
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
            // Gestion des exceptions
            catch (Exception e)
            {
                Console.WriteLine($"SupprimerContact: {e.Message}");
            }
            return false;
        }
        // Méthode privée Afficher pour afficher l'arborescence actuelle avec paramètre args de type string[]
        private bool Afficher(string[] args)
        {
            // Si le nombre d'arguments est supérieur à 2, retourner vrai
            if (args.Length > 2) return true;
            if (args.Length == 2)
            {
                // Si le deuxième argument est différent de ".", retourner vrai
                if (args[1] != ".") return true;
                // Appel de la méthode ToString de la propriété gestionnaire pour courant
                gestionnaire.Courant.ToString();
            }
            // Sinon, appeler la méthode ToString de la propriété gestionnaire pour la racine
            else gestionnaire.Root.ToString();

            return false;
        }
        // Méthode privée ChangerDossier pour changer le dossier actuel avec paramètre args de type string[]
        private bool ChangerDossier(string[] args)
        {
            // Si le nombre d'arguments est supérieur à 2, retourner vrai
            if (args.Length > 2) return true;
            // Variable locale chemin de type string
            var chemin = args.Length == 1 ? "" : args[1];
            // si n'est pas d'argument, appeler la méthode SetCourantAsRoot de la propriété gestionnaire
            if (args.Length == 1)
            {
                gestionnaire.SetCourantAsRoot();
            }
            // Sinon, si le chemin n'est pas valide, afficher un message d'erreur
            else if (!gestionnaire.ChangerCourant(chemin))
            {
                Console.WriteLine($"ChangerDossier: Le dossier \"{chemin}\" n'existe pas.");
            }
            return false;
        }
        // Méthode privée AfficherChemin pour afficher le chemin actuel avec paramètre args de type string[]
        private bool AfficherChemin(string[] args)
        {
            // Si le nombre d'arguments est différent de 1, retourner vrai
            if (args.Length != 1) return true;
            // Affichage du chemin actuel
            Console.WriteLine($"AfficherChemin: {gestionnaire.Courant.GetPath()}");
            return false;
        }
        // Méthode privée Sortir pour quitter le programme avec paramètre args de type string[]
        private bool Sortir(string[] args)
        {
            // Si le nombre d'arguments est différent de 1, retourner vrai
            if (args.Length != 1) return true;
            // Affichage du message de sortie
            throw new Exception("Sortir");
        }
        // Méthode privée ModifierDossier pour modifier un dossier de l'arborescence actuelle avec paramètre args de type string[]
        private bool ModifierDossier(string[] args)
        {
            // Si le nombre d'arguments est différent de 3, retourner vrai
            if (args.Length != 3) return true;
            try
            {
                // Variables locales nomDossier, nomChanger, nouveauNom
                var nomDossier = args[1];
                var nomChanger = args[2].Split('=')[0];
                var nouveauNom = args[2].Split('=')[1];
                // Si le nomChanger n'est pas égal à "nom", afficher un message d'erreur
                if(!nomChanger.Equals("nom", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("ModifierDossier: syntaxe invalide, tapez \"aide modifierdossier\" pour voir la syntaxe correcte");
                    return true;
                }
                // Appel de la méthode GetDossier de la propriété gestionnaire avec paramètre nomDossier
                var dossier = gestionnaire.Courant.GetDossier(nomDossier);
                // Si le dossier est null, afficher un message d'erreur
                if (dossier == null)
                {
                    Console.WriteLine($"ModifierDossier: Le dossier \"{nomDossier}\" n'existe pas.");
                }
                else
                {
                    // Modifier le nom du dossier
                    dossier.Nom = nouveauNom;
                    Console.WriteLine($"ModifierDossier: Le nom du dossier a été modifié avec succès de \"{nomDossier}\" à \"{dossier.Nom}\".");
                }

            }
            // Gestion des exceptions
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
        // Méthode privée ModifierContact pour modifier un contact de l'arborescence actuelle avec paramètre args de type string[]
        private bool ModifierContact(string[] args)
        {
            // Si le nombre d'arguments est inférieur à 3, retourner vrai
            if (args.Length < 3) return true;
            else
            {
                // Variable locale nomContact de type string
                var nomContact = args[1];
                // Appel de la méthode GetContact de la propriété gestionnaire avec paramètre nomContact
                var contact = gestionnaire.Courant.GetContact(nomContact);
                // Si le contact est null, afficher un message d'erreur
                if (contact == null)
                {
                    Console.WriteLine($"ModifierContact: Le contact \"{nomContact}\" n'existe pas.");
                }
                else
                {
                    // Boucle for pour modifier les paramètres du contact
                    for(int i=2;i<args.Length; i++)
                    {
                        // Si l'argument ne contient pas "=", afficher un message d'erreur
                        if (!args[i].Contains("="))
                        {
                            Console.WriteLine("ModifierContact: syntaxe invalide, tapez \"aide modifiercontact\" pour voir la syntaxe correcte");
                            return false;
                        }
                        // Variables locales parameters, parameter, value
                        string[] parameters = args[i].Split(new char[] { '=' }, 2);
                        string parameter = parameters[0];
                        string value = parameters[1];
                        // Switch pour modifier les paramètres du contact
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
        // Méthode privée Charger pour charger l'arborescence avec paramètre args de type string[]
        private bool Charger(string[] args)
        {
            // Si le nombre d'arguments est supérieur à 2, retourner vrai
            if (args.Length > 2) return true;
            // Variable locale key de type string
            string key = args.Length == 2 ? args[1] : null;
            try
            {
                // Appel de la méthode Charger de la propriété gestionnaire avec paramètre key
                if(gestionnaire.Charger(key))
                {
                    Console.WriteLine($"Charger: L'arborescence a été chargée avec succès.");
                }
                // Si le mot de passe est invalide, afficher un message d'erreur et supprimer la base de données
                else if (gestionnaire.TryCount <=0) Console.WriteLine("$Charger: trop de mots de passe invalides : suppression de la base de données...");
                // Sinon, afficher un message d'erreur et le nombre de tentatives restantes
                else Console.WriteLine($"Charger: échec du chargement : mot de passe invalide, \"{gestionnaire.TryCount}\"/3 tentatives restantes ");
            }
            // Gestion des exceptions
            catch(FileNotFoundException e)
            {
                Console.WriteLine($"Charger: Aucune sauvegarde n'a été trouvée.");
            }catch(Exception e)
            {
                Console.WriteLine($"Charger: {e.Message}");
            }
            return false;
        }
        // Méthode privée Enregistrer pour enregistrer l'arborescence avec paramètre args de type string[]
        private bool Enregistrer(string[] args)
        {
            // Si le nombre d'arguments est supérieur à 2, retourner vrai
            if (args.Length > 2) return true;
            Console.WriteLine($"Enregistrer: Enregistrement du fichier GestionnaireContactsDossiers.db...");
            // Variable locale key de type string
            string key = args.Length == 2 ? args[1] : null;
            try
            {
                // Appel de la méthode Enregistrer de la propriété gestionnaire avec paramètre key
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
