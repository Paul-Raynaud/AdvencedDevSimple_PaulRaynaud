# 📚 Index de la Documentation - AdvancedDevSample

Bienvenue dans la documentation du projet **AdvancedDevSample** !

## 🚀 Démarrage rapide

**Première fois ici ?** Commencez par ces étapes :

1. **[README.md](README.md)** - Vue d'ensemble et démarrage rapide
2. **[AUTHENTICATION.md](AUTHENTICATION.md)** - Configuration JWT et premiers tests
3. Lancer l'app : `cd AdvancedDevSample.Api && dotnet run`
4. Ouvrir Swagger : http://localhost:5069/swagger

---

## 📖 Documentation disponible

### 🎯 Pour les développeurs

| Document | Description | Quand l'utiliser |
|----------|-------------|------------------|
| **[README.md](README.md)** | Présentation générale du projet | Premier contact avec le projet |
| **[ARCHITECTURE.md](ARCHITECTURE.md)** | Vue synthétique de l'architecture | Comprendre rapidement la structure |
| **[DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)** | Documentation technique complète | Développement et maintenance |
| **[DIAGRAMMES.md](DIAGRAMMES.md)** | Tous les diagrammes (Mermaid) | Visualiser l'architecture |
| **[AUTHENTICATION.md](AUTHENTICATION.md)** | Guide d'authentification JWT | Problèmes avec les tokens JWT |

### 🧪 Pour les tests

| Fichier | Description |
|---------|-------------|
| `test-auth.sh` | Script de test d'authentification automatique |
| `requests.http` | Collection de requêtes HTTP de test |
| `AdvancedDevSample.Test/` | Tests unitaires et d'intégration |

---

## 🗺️ Navigation par besoin

### "Je veux comprendre l'architecture"

1. **[ARCHITECTURE.md](ARCHITECTURE.md)** - Résumé rapide
2. **[DIAGRAMMES.md](DIAGRAMMES.md)** - Visualisation
3. **[DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)** Section "Architecture du projet"

### "Je veux ajouter une fonctionnalité"

1. **[DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)** Section "Structure des couches"
2. **[ARCHITECTURE.md](ARCHITECTURE.md)** Section "Checklist d'implémentation"
3. Suivre le pattern existant des produits

### "J'ai une erreur 401 Unauthorized"

1. **[AUTHENTICATION.md](AUTHENTICATION.md)** Section "Erreurs courantes"
2. Vérifier le format du header : `Authorization: Bearer {token}`
3. Vérifier l'expiration du token (60 min)
4. Exécuter `./test-auth.sh` pour diagnostiquer

### "Je veux tester l'API"

1. Lancer l'app : `dotnet run` (depuis AdvancedDevSample.Api/)
2. Option 1 : **Swagger** → http://localhost:5069/swagger
3. Option 2 : **requests.http** → Ouvrir dans Rider/VS Code
4. Option 3 : **Script shell** → `./test-auth.sh`

### "Je veux écrire des tests"

1. **[DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)** Section "Tests"
2. Voir les exemples dans `AdvancedDevSample.Test/`
3. Exécuter : `dotnet test`

---

## 📊 Diagrammes disponibles

Tous les diagrammes sont dans **[DIAGRAMMES.md](DIAGRAMMES.md)** :

| Diagramme | Utilité |
|-----------|---------|
| **Classes - Domaine** | Comprendre les entités métier |
| **Séquence - Création produit** | Flux de création d'un produit |
| **Séquence - Authentification JWT** | Flux d'obtention du token |
| **Flux - Gestion des erreurs** | Comment les erreurs sont gérées |
| **Composants** | Relations entre les composants |
| **Architecture globale** | Vue d'ensemble complète |
| **État - Cycle de vie produit** | États d'un produit |
| **Package - Dépendances** | Dépendances entre projets |

---

## 🔍 Recherche rapide

### Concepts clés

- **Domain-Driven Design (DDD)** → [DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)
- **Clean Architecture** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **JWT Authentication** → [AUTHENTICATION.md](AUTHENTICATION.md)
- **Repository Pattern** → [ARCHITECTURE.md](ARCHITECTURE.md) + [DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md)
- **Value Objects** → [DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md) Section "Domain Layer"
- **Dependency Injection** → [DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md) Section "Configuration"

### Fichiers importants

- **Program.cs** → Configuration de l'application
- **ProductsController.cs** → Endpoints API produits
- **AuthController.cs** → Endpoint d'authentification
- **ProductService.cs** → Logique applicative
- **Product.cs** (Domain) → Entité métier
- **Price.cs** → Value Object

---

## 🛠️ Commandes utiles

```bash
# Restaurer les dépendances
dotnet restore

# Compiler
dotnet build

# Lancer l'application
cd AdvancedDevSample.Api
dotnet run

# Exécuter les tests
dotnet test

# Exécuter les tests avec détails
dotnet test --verbosity detailed

# Tester l'authentification
chmod +x test-auth.sh
./test-auth.sh

# Nettoyer et reconstruire
dotnet clean
dotnet build
```

---

## 📈 Parcours d'apprentissage recommandé

### Niveau 1 : Débutant
1. Lire [README.md](README.md)
2. Lancer l'application
3. Tester avec Swagger
4. Lire [AUTHENTICATION.md](AUTHENTICATION.md)

### Niveau 2 : Intermédiaire
1. Lire [ARCHITECTURE.md](ARCHITECTURE.md)
2. Explorer le code des controllers
3. Comprendre ProductService
4. Voir les diagrammes dans [DIAGRAMMES.md](DIAGRAMMES.md)

### Niveau 3 : Avancé
1. Lire [DOCUMENTATION_TECHNIQUE.md](DOCUMENTATION_TECHNIQUE.md) complète
2. Étudier le Domain Layer
3. Comprendre les patterns utilisés
4. Écrire de nouveaux tests

---

## 🎯 Objectifs de la documentation

Cette documentation vise à :
- ✅ Faciliter l'onboarding des nouveaux développeurs
- ✅ Documenter les décisions architecturales
- ✅ Servir de référence technique
- ✅ Illustrer les bonnes pratiques .NET

---

## 📞 Besoin d'aide ?

1. **Chercher** dans la documentation ci-dessus
2. **Consulter** les exemples de code existants
3. **Exécuter** les tests pour voir des cas d'usage
4. **Déboguer** avec les logs de l'application

---

## 📝 Contribuer à la documentation

Pour améliorer cette documentation :
1. Identifier les sections manquantes ou peu claires
2. Ajouter des exemples concrets
3. Mettre à jour les diagrammes si l'architecture change
4. Ajouter des cas d'usage réels

---

**Dernière mise à jour** : 9 février 2026  
**Version de la documentation** : 1.0  
**Projet** : AdvancedDevSample .NET 10

