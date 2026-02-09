# 🚀 AdvancedDevSample

API REST .NET 10 construite avec **Domain-Driven Design (DDD)** et **Clean Architecture**.

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

## 📋 Description

AdvancedDevSample est un projet d'exemple démontrant les meilleures pratiques de développement d'une API REST en .NET, incluant :

- ✅ Architecture en couches (Clean Architecture)
- ✅ Domain-Driven Design (DDD)
- ✅ Authentification JWT
- ✅ Tests unitaires et d'intégration
- ✅ Documentation Swagger/OpenAPI
- ✅ Gestion centralisée des erreurs

## 🏗️ Architecture

```
┌─────────────────────────────────────────┐
│         API Layer (Presentation)        │
│    Controllers + Middlewares + Swagger  │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│       Application Layer (Services)      │
│    Services + DTOs + Business Logic     │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│        Domain Layer (Core Logic)        │
│  Entities + Value Objects + Interfaces  │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Infrastructure Layer (Data Access)   │
│    Repositories + EF Core + Database    │
└─────────────────────────────────────────┘
```

## 🛠️ Technologies

- **.NET 10** - Framework
- **ASP.NET Core Web API** - API REST
- **JWT** - Authentification
- **Entity Framework Core (In-Memory)** - ORM
- **Swagger/OpenAPI** - Documentation
- **xUnit** - Tests

## 🚀 Démarrage rapide

### Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Un IDE (Rider, Visual Studio, ou VS Code)

### Installation

```bash
# Cloner le repository
git clone <repository-url>
cd AdvancedDevSample

# Restaurer les dépendances
dotnet restore

# Compiler le projet
dotnet build

# Lancer l'application
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur : **http://localhost:5069**

### Accéder à Swagger

Ouvrez votre navigateur : **http://localhost:5069/swagger**

## 🔐 Authentification

### 1. Obtenir un token JWT

```bash
curl -X POST http://localhost:5069/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password"
  }'
```

**Réponse :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-09T12:53:28Z"
}
```

### 2. Utiliser le token

Ajoutez l'en-tête `Authorization` à vos requêtes :

```bash
curl -X GET http://localhost:5069/api/products \
  -H "Authorization: Bearer {votre_token}"
```

### Identifiants de test

| Username | Password |
|----------|----------|
| admin    | password |
| user     | password |

## 📚 API Endpoints

### Authentication

| Méthode | Endpoint          | Description       | Auth |
|---------|-------------------|-------------------|------|
| POST    | /api/auth/login   | Obtenir un token  | ❌   |

### Products (🔒 Authentification requise)

| Méthode | Endpoint                  | Description              |
|---------|---------------------------|--------------------------|
| POST    | /api/products             | Créer un produit         |
| GET     | /api/products             | Lister tous les produits |
| GET     | /api/products/{id}        | Obtenir un produit       |
| PUT     | /api/products/{id}        | Mettre à jour un produit |
| PUT     | /api/products/{id}/price  | Changer le prix          |
| DELETE  | /api/products/{id}        | Supprimer un produit     |

## 🧪 Tests

### Exécuter les tests

```bash
# Tous les tests
dotnet test

# Avec détails
dotnet test --verbosity detailed

# Tests spécifiques
dotnet test --filter "FullyQualifiedName~ProductTests"
```

### Script de test d'authentification

```bash
# Rendre le script exécutable
chmod +x test-auth.sh

# Exécuter les tests
./test-auth.sh
```

### Couverture des tests

- ✅ Tests unitaires du domaine (Entities + Value Objects)
- ✅ Tests de services (Application Layer)
- ✅ Tests d'intégration (API End-to-End)

## 📖 Documentation

- **[Documentation technique complète](DOCUMENTATION_TECHNIQUE.md)** - Architecture, diagrammes, patterns
- **[Guide d'authentification JWT](AUTHENTICATION.md)** - Configuration JWT, troubleshooting
- **[Swagger UI](http://localhost:5069/swagger)** - Documentation API interactive (après démarrage)

## 📁 Structure du projet

```
AdvancedDevSample/
├── AdvancedDevSample.Domain/           # Entités, Value Objects, Interfaces
│   ├── Entities/
│   │   └── Product.cs
│   ├── ValueObjects/
│   │   └── Price.cs
│   ├── Interfaces/
│   │   └── IProductRepository.cs
│   └── Exceptions/
│       └── DomainException.cs
│
├── AdvancedDevSample.Application/      # Services métier, DTOs
│   ├── Services/
│   │   ├── ProductService.cs
│   │   └── TokenService.cs
│   ├── DTOs/
│   │   ├── CreateProductRequest.cs
│   │   ├── ProductResponse.cs
│   │   ├── LoginRequest.cs
│   │   └── LoginResponse.cs
│   └── Exceptions/
│       └── ApplicationServiceException.cs
│
├── AdvancedDevSample.Infrastructure/   # Repositories, EF Core
│   └── Repositories/
│       ├── EfProductRepository.cs
│       └── ProductEntity.cs
│
├── AdvancedDevSample.Api/              # Controllers, Middlewares, Config
│   ├── Controllers/
│   │   ├── ProductsController.cs
│   │   └── AuthController.cs
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── requests.http
│
└── AdvancedDevSample.Test/             # Tests unitaires + intégration
    ├── Domain/
    │   ├── Entities/ProductTests.cs
    │   └── ValueObjects/PriceTests.cs
    ├── Application/
    │   └── Services/ProductServiceTests.cs
    └── API/
        └── Integration/ProductsControllerIntegrationTests.cs
```

## 🎯 Exemples d'utilisation

### Créer un produit

```http
POST http://localhost:5069/api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "price": 99.99
}
```

### Obtenir tous les produits

```http
GET http://localhost:5069/api/products
Authorization: Bearer {token}
```

### Mettre à jour un produit

```http
PUT http://localhost:5069/api/products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "price": 149.99,
  "isActive": true
}
```

### Changer le prix uniquement

```http
PUT http://localhost:5069/api/products/{id}/price
Authorization: Bearer {token}
Content-Type: application/json

{
  "newPrice": 199.99
}
```

## 🔍 Patterns & Principes

### Design Patterns

- **Repository Pattern** - Abstraction de la couche de données
- **Dependency Injection** - Inversion de contrôle
- **Value Object Pattern** - Encapsulation de la logique métier
- **Middleware Pattern** - Gestion centralisée des erreurs
- **DTO Pattern** - Séparation des modèles

### Principes SOLID

- ✅ **S**ingle Responsibility Principle
- ✅ **O**pen/Closed Principle
- ✅ **L**iskov Substitution Principle
- ✅ **I**nterface Segregation Principle
- ✅ **D**ependency Inversion Principle

### Clean Architecture

Le projet respecte les principes de la Clean Architecture :
- Indépendance des frameworks
- Testabilité maximale
- Indépendance de l'UI
- Indépendance de la base de données
- Indépendance de tout agent externe

## ⚙️ Configuration

### JWT Settings (appsettings.json)

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

⚠️ **Attention** : En production, utilisez des secrets sécurisés (Azure Key Vault, AWS Secrets Manager, etc.)

## 🐛 Troubleshooting

### Erreur 401 Unauthorized

✅ Vérifiez que vous avez inclus le header `Authorization: Bearer {token}`  
✅ Vérifiez que le token n'est pas expiré (durée : 60 minutes)  
✅ Vérifiez que vous utilisez le bon port (5069, pas 5000)

### Erreur de compilation

```bash
# Nettoyer et reconstruire
dotnet clean
dotnet restore
dotnet build
```

