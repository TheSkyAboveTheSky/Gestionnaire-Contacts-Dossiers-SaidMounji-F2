using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCSharpGestionContactsDossiersSaidMounjiF2
{
    internal class Commande
    {
        public readonly string Nom;
        public readonly string Description;
        public readonly string Syntax;
        private Func<string[], bool> Action { get; }

        public Commande(string nom, string description, string syntax, Func<string[], bool> action)
        {
            Nom = nom;
            Description = description;
            Syntax = syntax;
            Action = action;
        }
        public void Executer(string[] args)
        {
            if (Action(args)) Console.WriteLine($"Arguments invalides, tapez \"aide {Nom}\" pour voir comment utiliser {Nom}");

        }
        static public void Executer(string[] args, Commande[] commandes)
        {
            Commande commande = commandes.FirstOrDefault(c => c.Nom == args[0]);

            if (commande == null) Console.WriteLine("Commande invalide, tapez \"aide\" pour voir la liste des commandes");
            else commande.Executer(args);
        }
    }
}
