# Architecture AdvancedDevSample

## 🏗️ Vue d'ensemble

Architecture en **4 couches** selon les principes de **Clean Architecture** et **DDD**.

```
┌──────────────────────────────────┐
│    API (Presentation Layer)      │  ← Controllers, Middlewares
├──────────────────────────────────┤
│   Application Layer              │  ← Services, DTOs
├──────────────────────────────────┤
│   Domain Layer (CORE)            │  ← Entities, Value Objects
├──────────────────────────────────┤
│   Infrastructure Layer           │  ← Repositories, EF Core
└──────────────────────────────────┘
```

## 🔄 Flux de données

**Création d'un produit :**

1. **Client** → POST /api/products {price: 99.99}
2. **Controller** → Valide JWT + ModelState
3. **Service** → CreateProduct(request)
4. **Entity** → new Product(99.99) + validation
5. **Repository** → Add(product)
6. **Database** → Persiste
7. **Response** → 201 Created + ProductResponse

## 📦 Responsabilités par couche

### API Layer
- Recevoir requêtes HTTP
- Authentification JWT
- Retourner réponses HTTP

### Application Layer
- Orchestrer use cases
- Transformer Entity ↔ DTO
- Coordonner domaine + infra

### Domain Layer
- Règles métier
- Validation domaine
- Logique pure (0 dépendance)

### Infrastructure Layer
- Accès base de données
- Implémentation repositories
- Services externes

## 🎯 Patterns utilisés

- **Repository Pattern** - Abstraction données
- **Dependency Injection** - IoC
- **Value Object** - Encapsulation
- **Middleware** - Cross-cutting concerns

Pour plus de détails, voir **DOCUMENTATION_TECHNIQUE.md**

