# Documentation Technique - AdvancedDevSample

## 📋 Table des matières

1. [Vue d'ensemble](#vue-densemble)
2. [Architecture du projet](#architecture-du-projet)
3. [Diagrammes](#diagrammes)
4. [Structure des couches](#structure-des-couches)
5. [Modèle de données](#modèle-de-données)
6. [API Endpoints](#api-endpoints)
7. [Authentification et sécurité](#authentification-et-sécurité)
8. [Gestion des erreurs](#gestion-des-erreurs)
9. [Tests](#tests)
10. [Configuration](#configuration)

---

## 🎯 Vue d'ensemble

**AdvancedDevSample** est une API REST .NET 10 construite selon les principes du **Domain-Driven Design (DDD)** et de l'**architecture en couches (Clean Architecture)**.

### Technologies utilisées

- **.NET 10** - Framework principal
- **ASP.NET Core Web API** - API REST
- **JWT (JSON Web Token)** - Authentification
- **Entity Framework Core** (In-Memory) - Persistence de données
- **Swashbuckle/Swagger** - Documentation API
- **xUnit** - Tests unitaires et d'intégration

### Principes architecturaux

- ✅ **Domain-Driven Design (DDD)**
- ✅ **Clean Architecture** / Architecture en couches
- ✅ **SOLID Principles**
- ✅ **Dependency Injection**
- ✅ **Repository Pattern**
- ✅ **Value Objects**
- ✅ **Exception-based error handling**

---

## 🏗️ Architecture du projet

### Structure des projets

```
AdvancedDevSample/
├── AdvancedDevSample.Domain/          # Couche Domaine (Entités, Value Objects, Exceptions)
├── AdvancedDevSample.Application/      # Couche Application (Services, DTOs)
├── AdvancedDevSample.Infrastructure/   # Couche Infrastructure (Repositories, EF Core)
├── AdvancedDevSample.Api/             # Couche Présentation (Controllers, Middlewares)
└── AdvancedDevSample.Test/            # Tests (Unitaires + Intégration)
```

### Architecture en couches

```
┌─────────────────────────────────────────────────────────┐
│                    PRÉSENTATION                          │
│              (AdvancedDevSample.Api)                     │
│  ┌─────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ Controllers │  │ Middlewares  │  │  Program.cs   │  │
│  └─────────────┘  └──────────────┘  └───────────────┘  │
└────────────────────────┬────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────┐
│                    APPLICATION                           │
│          (AdvancedDevSample.Application)                 │
│  ┌──────────────┐  ┌─────────┐  ┌──────────────────┐   │
│  │   Services   │  │  DTOs   │  │   Exceptions     │   │
│  └──────────────┘  └─────────┘  └──────────────────┘   │
└────────────────────────┬────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────┐
│                      DOMAINE                             │
│           (AdvancedDevSample.Domain)                     │
│  ┌─────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │  Entities   │  │ Value Objects│  │  Interfaces  │   │
│  │  (Product)  │  │   (Price)    │  │ (IRepository)│   │
│  └─────────────┘  └──────────────┘  └──────────────┘   │
└────────────────────────┬────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────┐
│                  INFRASTRUCTURE                          │
│        (AdvancedDevSample.Infrastructure)                │
│  ┌──────────────────┐  ┌─────────────────────────────┐ │
│  │  Repositories    │  │    EF Core / DB Context     │ │
│  └──────────────────┘  └─────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
```

---

## 📊 Diagrammes

### 1. Diagramme de classes - Domaine

```
┌─────────────────────────────────────┐
│           <<Entity>>                │
│            Product                  │
├─────────────────────────────────────┤
│ + Id: Guid                          │
│ + Price: Price                      │
│ + IsActive: bool                    │
│ + CreatedAt: DateTime               │
│ + UpdatedAt: DateTime               │
├─────────────────────────────────────┤
│ + Product(price: decimal)           │
│ + UpdatePrice(newPrice: Price): void│
│ + Deactivate(): void                │
│ + Update(price: Price, isActive:    │
│          bool): void                │
└──────────────┬──────────────────────┘
               │ contains
               │ 1
               ▼
┌─────────────────────────────────────┐
│        <<Value Object>>             │
│            Price                    │
├─────────────────────────────────────┤
│ + Value: decimal { get; }           │
├─────────────────────────────────────┤
│ + Price(value: decimal)             │
│ + Equals(other: Price): bool        │
│ + GetHashCode(): int                │
│ + ToString(): string                │
└─────────────────────────────────────┘
```

### 2. Diagramme de séquence - Création d'un produit

```
Client          Controller       ProductService     Product       Repository
  │                 │                  │               │              │
  │ POST /products  │                  │               │              │
  ├────────────────>│                  │               │              │
  │                 │                  │               │              │
  │                 │ CreateProduct()  │               │              │
  │                 ├─────────────────>│               │              │
  │                 │                  │               │              │
  │                 │                  │ new Product() │              │
  │                 │                  ├──────────────>│              │
  │                 │                  │               │              │
  │                 │                  │<──────────────┤              │
  │                 │                  │  Product      │              │
  │                 │                  │               │              │
  │                 │                  │ Add(product)  │              │
  │                 │                  ├──────────────────────────────>│
  │                 │                  │               │              │
  │                 │                  │<──────────────────────────────┤
  │                 │                  │               │              │
  │                 │<─────────────────┤               │              │
  │                 │  ProductResponse │               │              │
  │                 │                  │               │              │
  │<────────────────┤                  │               │              │
  │  201 Created    │                  │               │              │
  │                 │                  │               │              │
```

### 3. Diagramme de flux - Authentification JWT

```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │
       │ 1. POST /api/auth/login
       │    { username, password }
       ▼
┌──────────────────┐
│ AuthController   │
└──────┬───────────┘
       │
       │ 2. Validation identifiants
       ▼
┌──────────────────┐          ┌──────────────────┐
│  TokenService    │◄─────────┤  Configuration   │
└──────┬───────────┘          │  (appsettings)   │
       │                      └──────────────────┘
       │ 3. GenerateToken()
       │    - Claims (sub, unique_name, jti)
       │    - Signature (HMAC-SHA256)
       │    - Expiration (60 min)
       ▼
┌──────────────────┐
│   JWT Token      │
│  eyJhbGciOi...   │
└──────┬───────────┘
       │
       │ 4. Return token + expiresAt
       ▼
┌──────────────────┐
│    Client        │
│  Store token     │
└──────┬───────────┘
       │
       │ 5. Subsequent requests
       │    Authorization: Bearer {token}
       ▼
┌──────────────────┐
│ JWT Middleware   │
└──────┬───────────┘
       │
       │ 6. Validate token
       │    ✓ Signature
       │    ✓ Issuer
       │    ✓ Audience
       │    ✓ Expiration
       ▼
┌──────────────────┐
│  Authorized      │
│  Access granted  │
└──────────────────┘
```

### 4. Diagramme de composants

```
┌──────────────────────────────────────────────────────────────┐
│                        API Layer                              │
│  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐ │
│  │ ProductsCtrl   │  │  AuthCtrl      │  │  Middlewares   │ │
│  │ [Authorize]    │  │  [AllowAnon]   │  │  - Exception   │ │
│  └───────┬────────┘  └───────┬────────┘  │  - JWT Auth    │ │
│          │                   │            └────────────────┘ │
└──────────┼───────────────────┼───────────────────────────────┘
           │                   │
           ▼                   ▼
┌──────────────────────────────────────────────────────────────┐
│                    Application Layer                          │
│  ┌────────────────────────┐  ┌─────────────────────────────┐ │
│  │   ProductService       │  │     TokenService            │ │
│  │  - CreateProduct()     │  │  - GenerateToken()          │ │
│  │  - UpdateProduct()     │  │                             │ │
│  │  - DeleteProduct()     │  │                             │ │
│  │  - GetProduct()        │  │                             │ │
│  │  - GetAllProducts()    │  │                             │ │
│  └───────────┬────────────┘  └─────────────────────────────┘ │
└──────────────┼───────────────────────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────────────────────┐
│                      Domain Layer                             │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────────┐  │
│  │   Product   │  │    Price     │  │ IProductRepository │  │
│  │   Entity    │  │ Value Object │  │    Interface       │  │
│  └─────────────┘  └──────────────┘  └──────────┬─────────┘  │
└─────────────────────────────────────────────────┼────────────┘
                                                   │
                                                   ▼
┌──────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                         │
│  ┌──────────────────────────────────────────────────────┐    │
│  │            EfProductRepository                        │    │
│  │  - Add(), Update(), Delete(), GetById(), GetAll()    │    │
│  └────────────────────┬─────────────────────────────────┘    │
│                       │                                       │
│                       ▼                                       │
│  ┌──────────────────────────────────────────────────────┐    │
│  │         In-Memory Database (EF Core)                 │    │
│  └──────────────────────────────────────────────────────┘    │
└──────────────────────────────────────────────────────────────┘
```

---

## 🔧 Structure des couches

### 1. Domain Layer (AdvancedDevSample.Domain)

**Responsabilité** : Contient la logique métier centrale et les règles du domaine.

#### Entités

**Product.cs**
```csharp
public class Product
{
    public Guid Id { get; private set; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Business rules dans les méthodes
    public void UpdatePrice(Price newPrice)
    {
        if (!IsActive)
            throw new DomainException("Cannot update price of inactive product");
        
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

#### Value Objects

**Price.cs**
- Encapsule la logique de validation du prix
- Immuable (immutable)
- Égalité par valeur (value equality)

#### Interfaces

**IProductRepository.cs**
- Contrat pour la persistance des produits
- Indépendant de l'implémentation

### 2. Application Layer (AdvancedDevSample.Application)

**Responsabilité** : Orchestration des use cases et transformation des données.

#### Services

**ProductService.cs**
- Orchestration des opérations CRUD
- Transformation Entity ↔ DTO
- Coordination avec le Repository

**TokenService.cs**
- Génération de tokens JWT
- Configuration des claims
- Gestion de l'expiration

#### DTOs (Data Transfer Objects)

```
CreateProductRequest   → Création
UpdateProductRequest   → Mise à jour complète
ChangePriceRequest     → Mise à jour partielle
ProductResponse        → Réponse API
LoginRequest          → Authentification
LoginResponse         → Token JWT
```

### 3. Infrastructure Layer (AdvancedDevSample.Infrastructure)

**Responsabilité** : Implémentation concrète de la persistance.

#### Repositories

**EfProductRepository.cs**
- Implémentation de `IProductRepository`
- Utilise Entity Framework Core
- Base de données In-Memory (développement)

**ProductEntity.cs**
- Entité EF Core (mapping DB)
- Séparée de l'entité du domaine

### 4. API Layer (AdvancedDevSample.Api)

**Responsabilité** : Point d'entrée HTTP et gestion des requêtes.

#### Controllers

**ProductsController.cs**
- CRUD complet (Create, Read, Update, Delete)
- Sécurisé par JWT (`[Authorize]`)
- Documentation Swagger

**AuthController.cs**
- Endpoint `/api/auth/login`
- Génération de tokens JWT
- Accès public (`[AllowAnonymous]`)

#### Middlewares

**ExceptionHandlingMiddleware.cs**
- Capture globale des exceptions
- Transformation en réponses HTTP appropriées
- Logging centralisé

---

## 💾 Modèle de données

### Entité Product

| Propriété   | Type     | Description                    | Contraintes              |
|-------------|----------|--------------------------------|--------------------------|
| Id          | Guid     | Identifiant unique             | Primary Key, Auto        |
| Price       | Price    | Prix du produit (Value Object) | > 0                      |
| IsActive    | bool     | Statut actif/inactif           | Default: true            |
| CreatedAt   | DateTime | Date de création               | UTC, Auto                |
| UpdatedAt   | DateTime | Dernière modification          | UTC, Auto on update      |

### Value Object Price

| Propriété | Type    | Description      | Contraintes |
|-----------|---------|------------------|-------------|
| Value     | decimal | Valeur du prix   | > 0         |

### Règles métier

1. ✅ Le prix doit être strictement positif (> 0)
2. ✅ Le prix d'un produit inactif ne peut pas être modifié
3. ✅ `UpdatedAt` est automatiquement mis à jour lors de modifications
4. ✅ `CreatedAt` est défini une seule fois à la création

---

## 🌐 API Endpoints

### Authentication

| Méthode | Endpoint          | Description              | Auth Required |
|---------|-------------------|--------------------------|---------------|
| POST    | /api/auth/login   | Obtenir un token JWT     | ❌ Non        |

**Request Body:**
```json
{
  "username": "admin",
  "password": "password"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-09T12:53:28Z"
}
```

### Products

Tous les endpoints produits nécessitent un token JWT dans l'en-tête `Authorization: Bearer {token}`.

| Méthode | Endpoint                  | Description              | Auth Required |
|---------|---------------------------|--------------------------|---------------|
| POST    | /api/products             | Créer un produit         | ✅ Oui        |
| GET     | /api/products             | Liste tous les produits  | ✅ Oui        |
| GET     | /api/products/{id}        | Obtenir un produit       | ✅ Oui        |
| PUT     | /api/products/{id}        | Mettre à jour un produit | ✅ Oui        |
| PUT     | /api/products/{id}/price  | Changer le prix          | ✅ Oui        |
| DELETE  | /api/products/{id}        | Supprimer un produit     | ✅ Oui        |

#### Exemples de requêtes

**Créer un produit**
```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "price": 99.99
}
```

**Mettre à jour un produit**
```http
PUT /api/products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "price": 149.99,
  "isActive": true
}
```

**Changer uniquement le prix**
```http
PUT /api/products/{id}/price
Authorization: Bearer {token}
Content-Type: application/json

{
  "newPrice": 199.99
}
```

---

## 🔐 Authentification et sécurité

### Configuration JWT

**Fichier:** `appsettings.json`

```json
{
  "JwtSettings": {
    "SecretKey": "VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleClient",
    "ExpirationInMinutes": 60
  }
}
```

### Claims inclus dans le token

- **sub** (Subject) : ID de l'utilisateur
- **unique_name** : Nom d'utilisateur
- **jti** (JWT ID) : Identifiant unique du token

### Validation du token

Le middleware JWT valide automatiquement :
1. ✅ **Signature** : Token non modifié
2. ✅ **Issuer** : Émetteur valide
3. ✅ **Audience** : Destinataire valide
4. ✅ **Expiration** : Token non expiré
5. ✅ **Algorithme** : HMAC-SHA256

### Utilisateurs de test

| Username | Password | Role |
|----------|----------|------|
| admin    | password | Admin|
| user     | password | User |

---

## ⚠️ Gestion des erreurs

### Hiérarchie des exceptions

```
Exception
│
├── DomainException               (400 Bad Request)
│   └── Prix invalide, produit inactif, etc.
│
├── ApplicationServiceException   (404 Not Found / 400)
│   └── Produit introuvable, validation échouée
│
└── InfrastructureException      (500 Internal Server Error)
    └── Erreurs DB, réseau, etc.
```

### Middleware de gestion des erreurs

Le `ExceptionHandlingMiddleware` capture toutes les exceptions et les transforme en réponses HTTP appropriées :

```csharp
DomainException              → 400 Bad Request
ApplicationServiceException  → 404 Not Found (ou 400)
InfrastructureException      → 500 Internal Server Error
Exception (autres)           → 500 Internal Server Error
```

### Format des erreurs

```json
{
  "title": "Erreur métier",
  "detail": "Le prix doit être supérieur à zéro"
}
```

---

## 🧪 Tests

### Structure des tests

```
AdvancedDevSample.Test/
├── Domain/
│   ├── Entities/
│   │   └── ProductTests.cs         (Tests de l'entité Product)
│   └── ValueObjects/
│       └── PriceTests.cs           (Tests du Value Object Price)
├── Application/
│   └── Services/
│       └── ProductServiceTests.cs  (Tests du service métier)
└── API/
    └── Integration/
        └── ProductsControllerIntegrationTests.cs
```

### Types de tests

#### 1. Tests unitaires de domaine

**ProductTests.cs**
- Création de produit
- Mise à jour du prix
- Désactivation
- Règles métier (produit inactif)

**PriceTests.cs**
- Validation du prix (> 0)
- Égalité des Value Objects

#### 2. Tests de service

**ProductServiceTests.cs**
- CRUD complet
- Gestion des erreurs
- Mocking du repository

#### 3. Tests d'intégration

**ProductsControllerIntegrationTests.cs**
- Tests end-to-end de l'API
- Validation des codes HTTP
- Sérialisation JSON

### Exécution des tests

```bash
# Tous les tests
dotnet test

# Tests avec détails
dotnet test --verbosity detailed

# Tests d'une catégorie
dotnet test --filter "FullyQualifiedName~Domain"
```

### Couverture de code

Les tests couvrent :
- ✅ Entités et Value Objects (Domain)
- ✅ Services métier (Application)
- ✅ Controllers (API)
- ✅ Scénarios d'erreur

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JwtSettings": {
    "SecretKey": "VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleClient",
    "ExpirationInMinutes": 60
  }
}
```

### launchSettings.json

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5069",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### Program.cs - Configuration DI

```csharp
// Repositories
builder.Services.AddScoped<IProductRepository, EfProductRepository>();

// Services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TokenService>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* ... */ });

// Swagger
builder.Services.AddSwaggerGen(options => { /* ... */ });
```

---

## 🚀 Démarrage rapide

### 1. Prérequis

- .NET 10 SDK
- IDE : JetBrains Rider / Visual Studio / VS Code

### 2. Installation

```bash
# Clone le projet
git clone <repository-url>
cd AdvancedDevSample

# Restaurer les dépendances
dotnet restore

# Compiler
dotnet build
```

### 3. Lancer l'application

```bash
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur : **http://localhost:5069**

### 4. Accéder à Swagger

Ouvrez votre navigateur : **http://localhost:5069/swagger**

### 5. Tester l'authentification

```bash
# Obtenir un token
curl -X POST http://localhost:5069/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'

# Utiliser le token
curl -X GET http://localhost:5069/api/products \
  -H "Authorization: Bearer {votre_token}"
```

---

## 📦 Dépendances NuGet

### AdvancedDevSample.Api
- `Microsoft.AspNetCore.Authentication.JwtBearer` (10.x)
- `Swashbuckle.AspNetCore` (7.x)
- `Microsoft.OpenApi` (2.x)

### AdvancedDevSample.Application
- `Microsoft.Extensions.Configuration.Abstractions`
- `Microsoft.IdentityModel.Tokens`
- `System.IdentityModel.Tokens.Jwt`

### AdvancedDevSample.Infrastructure
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.InMemory`

### AdvancedDevSample.Test
- `xunit`
- `Microsoft.AspNetCore.Mvc.Testing`
- `Microsoft.EntityFrameworkCore.InMemory`

---

## 🔍 Patterns et bonnes pratiques

### Design Patterns utilisés

1. **Repository Pattern** : Abstraction de la couche de données
2. **Dependency Injection** : Inversion de contrôle
3. **Value Object Pattern** : Encapsulation de la logique du prix
4. **Middleware Pattern** : Gestion centralisée des exceptions
5. **DTO Pattern** : Séparation des modèles de domaine et API

### Principes SOLID

- **S**ingle Responsibility : Chaque classe a une seule raison de changer
- **O**pen/Closed : Ouvert à l'extension, fermé à la modification
- **L**iskov Substitution : Les interfaces sont respectées
- **I**nterface Segregation : Interfaces spécifiques et ciblées
- **D**ependency Inversion : Dépendance sur des abstractions

### Bonnes pratiques appliquées

✅ Séparation des responsabilités en couches  
✅ Validation au niveau du domaine  
✅ Gestion centralisée des erreurs  
✅ Documentation API avec Swagger  
✅ Tests unitaires et d'intégration  
✅ Logging structuré  
✅ Configuration externalisée  
✅ Sécurité avec JWT  

---

## 📈 Évolutions futures possibles

### Court terme
- [ ] Ajouter plus de propriétés au produit (nom, description, catégorie)
- [ ] Implémenter la pagination pour `GET /api/products`
- [ ] Ajouter des filtres et recherche
- [ ] Base de données réelle (SQL Server / PostgreSQL)

### Moyen terme
- [ ] Gestion des utilisateurs avec rôles
- [ ] Upload d'images de produits
- [ ] Système de catégories
- [ ] Historique des modifications (audit trail)
- [ ] Cache avec Redis

### Long terme
- [ ] Microservices architecture
- [ ] Event Sourcing
- [ ] CQRS pattern
- [ ] Message queue (RabbitMQ / Kafka)
- [ ] GraphQL API

---

## 📞 Support

Pour toute question ou problème :
1. Consultez la documentation d'authentification : `AUTHENTICATION.md`
2. Vérifiez les logs de l'application
3. Exécutez les tests : `dotnet test`
4. Utilisez le script de test : `./test-auth.sh`

---

## 📝 Notes de version

### v1.0.0 (Février 2026)
- ✅ Architecture DDD mise en place
- ✅ CRUD complet des produits
- ✅ Authentification JWT
- ✅ Tests unitaires et d'intégration
- ✅ Documentation Swagger
- ✅ Gestion des erreurs centralisée

---

**Date de dernière mise à jour** : 9 février 2026  
**Version du framework** : .NET 10  
**Auteur** : AdvancedDevSample Team

