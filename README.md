# Gestionnaire des Contacts & Dossiers

## Contexte

Ce projet a été réalisé dans le cadre du cours de développement C#.NET de ZZ2 à l'ISIMA, au sein de la filière F2. Il s'agissait d'un projet individuel réaliseé par moi Said Mounji ,visant à développer une application de gestion de contacts et de dossiers structurée sous forme d'arborescence. Cette approche permettait une organisation intuitive et efficace des informations, offrant ainsi une expérience utilisateur optimale.

L'implémentation de ce projet se décomposent en 3 sous-projets suivant:

### Serialize

Ce projet propose une implémentation robuste et sécurisée d'une interface de sérialisation de données, qui est basée sur le design pattern Factory. Cette interface offre une flexibilité accrue en permettant aux utilisateurs de choisir entre deux méthodes de sérialisation : binaire (utilisant BinaryFormatter) ou XML (utilisant XMLSerializer). En adoptant ce design pattern, le projet garantit une modularité et une extensibilité optimales, tout en simplifiant la gestion et la maintenance du code.

### Data

Ce projet se concentre sur l'implémentation et l'exposition des structures de données métier essentielles requises pour son fonctionnement. En plus de ces structures, il intègre un gestionnaire d'arborescence ,qui incluant la sérialisation des données chiffrées. La sérialisation des données est réalisée sur des objets spécifiques, ce qui permet de sauvegarder uniquement les données nécessaires à la régénération de l'arborescence et de ses informations associées. Cette approche optimise l'utilisation des ressources et réduit la taille des fichiers de sauvegarde, tout en préservant l'intégrité des données.

### Application Console

#### liste des commandes : `aide`
