# Guide Complet - AdvancedDevSample API avec Tests

## 🚀 Lancer l'application et voir Swagger

### Option 1 : Via terminal
```bash
cd /Volumes/Paul_SSD/AdvancedDevSample/AdvancedDevSample.Api
dotnet run
```

### Option 2 : Avec un port spécifique
```bash
cd /Volumes/Paul_SSD/AdvancedDevSample/AdvancedDevSample.Api
dotnet run --urls "http://localhost:5000"
```

### Accéder à Swagger
Une fois l'application démarrée, ouvrez votre navigateur à :
- **http://localhost:5000/swagger** 
- ou **https://localhost:5001/swagger** (avec HTTPS)

Le port exact sera affiché dans les logs au démarrage de l'application.

---

## ✅ Ce qui a été ajouté au projet

### 1. **CRUD Complet pour les Produits**

#### Contrôleur (`ProductsController.cs`)
- ✅ **POST** `/api/products` - Créer un produit
- ✅ **GET** `/api/products/{id}` - Obtenir un produit par ID
- ✅ **GET** `/api/products` - Obtenir tous les produits
- ✅ **PUT** `/api/products/{id}` - Mettre à jour un produit
- ✅ **DELETE** `/api/products/{id}` - Supprimer un produit
- ✅ **PUT** `/api/products/{id}/price` - Changer uniquement le prix

#### DTOs créés
- `CreateProductRequest.cs` - Pour créer un produit
- `ProductResponse.cs` - Pour retourner un produit
- `UpdateProductRequest.cs` - Pour mettre à jour un produit
- `ChangePriceRequest.cs` - Pour changer le prix (existant)

#### Service (`ProductService.cs`)
- `CreateProduct()` - Création
- `GetProduct()` - Lecture unique
- `GetAllProducts()` - Lecture multiple
- `UpdateProduct()` - Mise à jour
- `DeleteProduct()` - Suppression
- `ChangePrice()` - Changement de prix spécifique

#### Repository (`EfProductRepository.cs`)
Implémentation en mémoire avec toutes les méthodes CRUD :
- `Add()`
- `GetById()`
- `GetAll()`
- `Save()`
- `Delete()`
- `Exists()`

---

### 2. **Tests Unitaires**

#### Tests du Domaine

**`ProductTests.cs`** - Tests de l'entité Product :
- ✅ Création d'un produit avec un prix valide
- ✅ Création avec un ID spécifique
- ✅ Changement de prix (produit actif)
- ✅ Changement de prix bloqué (produit inactif)
- ✅ Désactivation/Activation d'un produit

**`PriceTests.cs`** - Tests du Value Object Price :
- ✅ Création avec valeur positive
- ✅ Rejet de valeur négative ou zéro
- ✅ Formatage ToString()
- ✅ Égalité entre deux prix

#### Tests de l'Application

**`ProductServiceTests.cs`** - Tests du service avec Moq :
- ✅ CreateProduct - succès et échec
- ✅ GetProduct - existant et non existant
- ✅ GetAllProducts
- ✅ UpdateProduct - plusieurs scénarios
- ✅ DeleteProduct
- ✅ ChangePrice

---

### 3. **Tests d'Intégration**

**`ProductsControllerIntegrationTests.cs`** - Tests de bout en bout :
- ✅ POST /api/products - création valide et invalide
- ✅ GET /api/products/{id} - existant et non existant
- ✅ GET /api/products - liste complète
- ✅ PUT /api/products/{id} - mise à jour
- ✅ DELETE /api/products/{id} - suppression
- ✅ PUT /api/products/{id}/price - changement de prix
- ✅ Scénarios d'erreur métier (produit inactif, etc.)

---

## 🧪 Exécuter les tests

### Tous les tests
```bash
cd /Volumes/Paul_SSD/AdvancedDevSample
dotnet test
```

### Tests avec détails
```bash
dotnet test --verbosity normal
```

### Tests avec couverture
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Tests d'un projet spécifique
```bash
cd /Volumes/Paul_SSD/AdvancedDevSample/AdvancedDevSample.Test
dotnet test
```

### Exécuter uniquement les tests unitaires du domaine
```bash
dotnet test --filter "FullyQualifiedName~Domain"
```

### Exécuter uniquement les tests d'intégration
```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

---

## 📦 Packages ajoutés

Dans `AdvancedDevSample.Test.csproj` :
- **xunit** - Framework de tests
- **Moq** - Mock pour les tests unitaires
- **Microsoft.AspNetCore.Mvc.Testing** - Tests d'intégration API
- **coverlet.collector** - Couverture de code

---

## 🏗️ Architecture du Projet

```
AdvancedDevSample/
├── AdvancedDevSample.Api/          # Couche API/Présentation
│   ├── Controllers/
│   │   └── ProductsController.cs   # CRUD complet
│   ├── Middlewares/
│   └── Program.cs                   # Configuration
│
├── AdvancedDevSample.Application/   # Couche Application
│   ├── DTOs/                        # Data Transfer Objects
│   │   ├── CreateProductRequest.cs
│   │   ├── ProductResponse.cs
│   │   ├── UpdateProductRequest.cs
│   │   └── ChangePriceRequest.cs
│   └── Services/
│       └── ProductService.cs        # Logique applicative
│
├── AdvancedDevSampleDomain/         # Couche Domaine
│   ├── Entities/
│   │   └── Product.cs               # Entité métier
│   ├── ValueObjects/
│   │   └── Price.cs                 # Value Object
│   ├── Interfaces/
│   │   └── IProductRepository.cs
│   └── Exceptions/
│
├── AdvancedDevSample.Infrastructure/ # Couche Infrastructure
│   └── Repositories/
│       └── EfProductRepository.cs    # Implémentation en mémoire
│
└── AdvancedDevSample.Test/          # Tests
    ├── Domain/
    │   ├── Entities/
    │   │   └── ProductTests.cs       # Tests unitaires Product
    │   └── ValueObjects/
    │       └── PriceTests.cs         # Tests unitaires Price
    ├── Application/
    │   └── Services/
    │       └── ProductServiceTests.cs # Tests avec mocks
    └── API/
        └── Integration/
            └── ProductsControllerIntegrationTests.cs # Tests E2E
```

---

## 🎯 Exemples de requêtes API

### Créer un produit
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"price": 99.99}'
```

### Obtenir tous les produits
```bash
curl http://localhost:5000/api/products
```

### Obtenir un produit par ID
```bash
curl http://localhost:5000/api/products/{guid}
```

### Mettre à jour un produit
```bash
curl -X PUT http://localhost:5000/api/products/{guid} \
  -H "Content-Type: application/json" \
  -d '{"price": 149.99, "isActive": true}'
```

### Changer le prix
```bash
curl -X PUT http://localhost:5000/api/products/{guid}/price \
  -H "Content-Type: application/json" \
  -d '{"newPrice": 199.99}'
```

### Supprimer un produit
```bash
curl -X DELETE http://localhost:5000/api/products/{guid}
```

---

## 🔍 Règles métier implémentées

1. **Prix strictement positif** : Un prix doit être > 0
2. **Produit actif pour changement de prix** : On ne peut pas changer le prix d'un produit inactif
3. **Activation/Désactivation** : Un produit peut être activé ou désactivé
4. **Value Object Price** : Garantit l'invariant du prix positif

---

## 📊 Résultats attendus des tests

Les tests couvrent :
- ✅ Tests unitaires du domaine (Product, Price)
- ✅ Tests unitaires de l'application (ProductService avec mocks)
- ✅ Tests d'intégration (API complète)
- ✅ Tests des règles métier
- ✅ Tests des cas d'erreur
- ✅ Tests de validation

---

## 🚀 Prochaines étapes possibles

1. **Base de données réelle** : Remplacer le repository en mémoire par Entity Framework avec SQL
2. **Authentification** : Ajouter JWT pour sécuriser l'API
3. **Logging** : Ajouter Serilog pour les logs structurés
4. **Validation avancée** : FluentValidation
5. **Documentation** : Améliorer les commentaires XML pour Swagger
6. **CI/CD** : GitHub Actions ou Azure DevOps
7. **Docker** : Conteneurisation de l'application

---

## 📝 Notes importantes

- Le repository utilise actuellement un `Dictionary` statique en mémoire
- Les tests d'intégration utilisent `WebApplicationFactory` pour créer un serveur de test
- Le constructeur `Program` est rendu `partial` et `public` pour les tests
- Les tests utilisent Moq pour simuler les dépendances
- Swagger est configuré pour afficher la documentation XML

---

**Créé le 9 février 2026**
