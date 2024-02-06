using System;
using System.Linq;

namespace ProjetCSharpGestionContactsDossiersSaidMounjiF2
{
    // Classe Commande qui permet de créer des commandes
    internal class Commande
    {
        // Attributs de la classe Commande
        // Nom de la commande
        public readonly string Nom;
        // Description de la commande
        public readonly string Description;
        // Syntaxe de la commande
        public readonly string Syntax;
        // Action de la commande
        private Func<string[], bool> Action { get; }
        // Constructeur de la classe Commande qui prend en paramètre le nom, la description, la syntaxe et l'action de la commande
        public Commande(string nom, string description, string syntax, Func<string[], bool> action)
        {
            Nom = nom;
            Description = description;
            Syntax = syntax;
            Action = action;
        }
        // Méthode qui permet d'exécuter une commande
        public void Executer(string[] args)
        {
            if (Action(args)) Console.WriteLine($"Arguments invalides, tapez \"aide {Nom}\" pour voir comment utiliser {Nom}");

        }
        // Méthode qui permet d'afficher la syntaxe d'une commande
        static public void Executer(string[] args, Commande[] commandes)
        {
            Commande commande = commandes.FirstOrDefault(c => c.Nom == args[0]);

            if (commande == null) Console.WriteLine("Commande invalide, tapez \"aide\" pour voir la liste des commandes");
            else commande.Executer(args);
        }
    }
}
