# Diagrammes du projet AdvancedDevSample

Ce document contient les diagrammes visuels du projet au format Mermaid.
Vous pouvez les visualiser directement sur GitHub ou avec n'importe quel outil compatible Mermaid.

## Table des matières

1. [Diagramme de classes - Domaine](#diagramme-de-classes---domaine)
2. [Diagramme de séquence - Création de produit](#diagramme-de-séquence---création-de-produit)
3. [Diagramme de séquence - Authentification JWT](#diagramme-de-séquence---authentification-jwt)
4. [Diagramme de flux - Gestion des erreurs](#diagramme-de-flux---gestion-des-erreurs)
5. [Diagramme de composants](#diagramme-de-composants)
6. [Diagramme d'architecture](#diagramme-darchitecture)

---

## Diagramme de classes - Domaine

```mermaid
classDiagram
    class Product {
        -Guid Id
        -Price Price
        -bool IsActive
        -DateTime CreatedAt
        -DateTime UpdatedAt
        +Product(decimal price)
        +UpdatePrice(Price newPrice) void
        +Deactivate() void
        +Update(Price price, bool isActive) void
    }
    
    class Price {
        <<Value Object>>
        +decimal Value
        +Price(decimal value)
        +Equals(Price other) bool
        +GetHashCode() int
        +ToString() string
    }
    
    class IProductRepository {
        <<interface>>
        +Add(Product product) void
        +Update(Product product) void
        +Delete(Guid id) void
        +GetById(Guid id) Product
        +GetAll() List~Product~
    }
    
    class DomainException {
        <<exception>>
        +DomainException(string message)
    }
    
    Product "1" *-- "1" Price : contains
    Product ..> DomainException : throws
    Price ..> DomainException : throws
    IProductRepository ..> Product : manages
```

---

## Diagramme de séquence - Création de produit

```mermaid
sequenceDiagram
    participant C as Client
    participant Ctrl as ProductsController
    participant Svc as ProductService
    participant Prod as Product
    participant Repo as IProductRepository
    participant DB as Database

    C->>Ctrl: POST /api/products<br/>{price: 99.99}
    activate Ctrl
    
    Ctrl->>Ctrl: Validate JWT Token
    Ctrl->>Svc: CreateProduct(request)
    activate Svc
    
    Svc->>Prod: new Product(99.99)
    activate Prod
    Prod->>Prod: Validate price > 0
    Prod-->>Svc: Product instance
    deactivate Prod
    
    Svc->>Repo: Add(product)
    activate Repo
    Repo->>DB: INSERT INTO Products
    DB-->>Repo: Success
    deactivate Repo
    
    Svc->>Svc: Map to ProductResponse
    Svc-->>Ctrl: ProductResponse
    deactivate Svc
    
    Ctrl-->>C: 201 Created<br/>ProductResponse
    deactivate Ctrl
```

---

## Diagramme de séquence - Authentification JWT

```mermaid
sequenceDiagram
    participant C as Client
    participant Auth as AuthController
    participant Token as TokenService
    participant Config as Configuration

    C->>Auth: POST /api/auth/login<br/>{username, password}
    activate Auth
    
    Auth->>Auth: Validate credentials<br/>(admin/password)
    
    alt Credentials valid
        Auth->>Token: GenerateToken(userId, username)
        activate Token
        
        Token->>Config: Get JWT settings<br/>(SecretKey, Issuer, etc.)
        Config-->>Token: JWT settings
        
        Token->>Token: Create claims<br/>(sub, unique_name, jti)
        Token->>Token: Create JWT<br/>(HMAC-SHA256 signature)
        Token->>Token: Set expiration<br/>(60 minutes)
        
        Token-->>Auth: JWT token string
        deactivate Token
        
        Auth-->>C: 200 OK<br/>{token, expiresAt}
    else Credentials invalid
        Auth-->>C: 401 Unauthorized
    end
    deactivate Auth

    Note over C: Store token for future requests
    
    C->>C: Add Authorization header<br/>Bearer {token}
```

---

## Diagramme de flux - Gestion des erreurs

```mermaid
flowchart TD
    A[Request HTTP] --> B{JWT Token valide?}
    B -->|Non| C[401 Unauthorized]
    B -->|Oui| D[Execute Controller]
    
    D --> E{Exception levée?}
    E -->|Non| F[200 OK / 201 Created]
    
    E -->|Oui| G{Type d'exception?}
    
    G -->|DomainException| H[ExceptionHandlingMiddleware]
    H --> I[400 Bad Request<br/>Erreur métier]
    
    G -->|ApplicationServiceException| J[ExceptionHandlingMiddleware]
    J --> K{Status Code?}
    K -->|NotFound| L[404 Not Found<br/>Ressource introuvable]
    K -->|BadRequest| M[400 Bad Request<br/>Validation échouée]
    
    G -->|InfrastructureException| N[ExceptionHandlingMiddleware]
    N --> O[500 Internal Server Error<br/>Erreur technique]
    
    G -->|Exception générique| P[ExceptionHandlingMiddleware]
    P --> Q[500 Internal Server Error<br/>Erreur inattendue]
    
    H --> R[Log error]
    J --> R
    N --> R
    P --> R
    
    R --> S[Return JSON error response]
```

---

## Diagramme de composants

```mermaid
graph TB
    subgraph API["API Layer<br/>(AdvancedDevSample.Api)"]
        PC[ProductsController<br/>Authorize]
        AC[AuthController<br/>AllowAnonymous]
        MW[ExceptionHandlingMiddleware]
        JWT[JWT Authentication<br/>Middleware]
    end
    
    subgraph APP["Application Layer<br/>(AdvancedDevSample.Application)"]
        PS[ProductService]
        TS[TokenService]
        DTO[DTOs]
        APPEX[Application<br/>Exceptions]
    end
    
    subgraph DOM["Domain Layer<br/>(AdvancedDevSample.Domain)"]
        PROD[Product Entity]
        PRICE[Price Value Object]
        IFACE[IProductRepository<br/>Interface]
        DOMEX[Domain<br/>Exceptions]
    end
    
    subgraph INFRA["Infrastructure Layer<br/>(AdvancedDevSample.Infrastructure)"]
        REPO[EfProductRepository]
        EF[Entity Framework<br/>Core]
        DB[(In-Memory<br/>Database)]
    end
    
    PC --> PS
    AC --> TS
    MW -.-> PC
    JWT -.-> PC
    
    PS --> PROD
    PS --> IFACE
    PS --> DTO
    
    PROD --> PRICE
    PROD --> DOMEX
    PRICE --> DOMEX
    
    REPO -.implements.-> IFACE
    REPO --> EF
    EF --> DB
    
    PS --> APPEX
    
    style API fill:#e1f5ff
    style APP fill:#fff3e0
    style DOM fill:#f3e5f5
    style INFRA fill:#e8f5e9
```

---

## Diagramme d'architecture

```mermaid
graph TB
    subgraph External["Clients externes"]
        SWAGGER[Swagger UI]
        HTTP[HTTP Client<br/>Postman/curl]
        TEST[Tests d'intégration]
    end
    
    subgraph Gateway["API Gateway"]
        HTTPS[HTTPS Endpoint<br/>localhost:5069]
    end
    
    subgraph Presentation["Presentation Layer"]
        direction TB
        CTRL[Controllers]
        MWARE[Middlewares]
        
        subgraph Auth["Authentication"]
            JWTM[JWT Middleware]
        end
    end
    
    subgraph Application["Application Layer"]
        direction TB
        SERVICES[Business Services]
        DTOS[Data Transfer Objects]
        
        subgraph UseCases["Use Cases"]
            UC1[Create Product]
            UC2[Update Product]
            UC3[Get Products]
            UC4[Delete Product]
            UC5[Generate Token]
        end
    end
    
    subgraph Domain["Domain Layer"]
        direction TB
        ENTITIES[Entities]
        VALUEOBJ[Value Objects]
        RULES[Business Rules]
        INTERFACES[Repository Interfaces]
    end
    
    subgraph Infrastructure["Infrastructure Layer"]
        direction TB
        REPOS[Repositories]
        
        subgraph Persistence["Data Persistence"]
            EFCORE[EF Core]
            INMEM[(In-Memory DB)]
        end
    end
    
    subgraph CrossCutting["Cross-Cutting Concerns"]
        LOG[Logging]
        CONFIG[Configuration]
        EXCEPT[Exception Handling]
    end
    
    SWAGGER --> HTTPS
    HTTP --> HTTPS
    TEST --> HTTPS
    
    HTTPS --> CTRL
    CTRL --> MWARE
    MWARE --> JWTM
    
    CTRL --> SERVICES
    SERVICES --> DTOS
    SERVICES --> UseCases
    
    UseCases --> ENTITIES
    UseCases --> VALUEOBJ
    ENTITIES --> RULES
    ENTITIES --> VALUEOBJ
    
    SERVICES --> INTERFACES
    REPOS -.implements.-> INTERFACES
    
    REPOS --> EFCORE
    EFCORE --> INMEM
    
    MWARE -.uses.-> LOG
    MWARE -.uses.-> EXCEPT
    SERVICES -.uses.-> CONFIG
    
    style External fill:#e3f2fd
    style Gateway fill:#fff3e0
    style Presentation fill:#f3e5f5
    style Application fill:#e8f5e9
    style Domain fill:#fff9c4
    style Infrastructure fill:#ffebee
    style CrossCutting fill:#f5f5f5
```

---

## Diagramme de déploiement (Architecture de déploiement)

```mermaid
graph TB
    subgraph Client["Client Layer"]
        BROWSER[Web Browser<br/>Swagger UI]
        CLI[CLI Tools<br/>curl/Postman]
    end
    
    subgraph Server["Application Server"]
        subgraph Runtime["ASP.NET Core Runtime"]
            API[AdvancedDevSample.Api<br/>Kestrel Web Server<br/>Port: 5069]
            
            subgraph AppServices["Application Services"]
                PRODSERV[ProductService]
                TOKENSERV[TokenService]
            end
            
            subgraph Domain["Domain Logic"]
                ENTITIES[Product Entities]
            end
            
            subgraph Data["Data Access"]
                REPOS[Repositories]
                EFCORE[EF Core]
            end
        end
    end
    
    subgraph Storage["Storage Layer"]
        MEMORY[(In-Memory<br/>Database)]
    end
    
    subgraph Config["Configuration"]
        SETTINGS[appsettings.json<br/>JWT Settings]
    end
    
    BROWSER -->|HTTPS| API
    CLI -->|HTTPS| API
    
    API --> AppServices
    AppServices --> Domain
    AppServices --> Data
    Data --> EFCORE
    EFCORE --> MEMORY
    
    API -.reads.-> SETTINGS
    
    style Client fill:#e3f2fd
    style Server fill:#f3e5f5
    style Storage fill:#e8f5e9
    style Config fill:#fff9c4
```

---

## Diagramme d'état - Cycle de vie d'un produit

```mermaid
stateDiagram-v2
    [*] --> Créé: CreateProduct()
    
    Créé --> Actif: Auto (IsActive=true)
    
    Actif --> Actif: UpdatePrice()<br/>(si IsActive=true)
    Actif --> Actif: Update()<br/>(si IsActive=true)
    
    Actif --> Inactif: Deactivate()<br/>Update(IsActive=false)
    
    Inactif --> Actif: Update(IsActive=true)
    
    Inactif --> Inactif: ❌ UpdatePrice()<br/>(Exception)
    
    Actif --> [*]: Delete()
    Inactif --> [*]: Delete()
    
    note right of Actif
        Peut modifier le prix
        Peut être désactivé
        Peut être supprimé
    end note
    
    note right of Inactif
        ❌ Ne peut pas modifier le prix
        Peut être réactivé
        Peut être supprimé
    end note
```

---

## Diagramme de package - Dépendances

```mermaid
graph LR
    subgraph External["Packages externes"]
        EFCORE[Microsoft.EntityFrameworkCore]
        JWT[Microsoft.AspNetCore<br/>.Authentication.JwtBearer]
        SWAGGER[Swashbuckle.AspNetCore]
        XUNIT[xUnit]
    end
    
    subgraph Project["Projet AdvancedDevSample"]
        API[AdvancedDevSample.Api]
        APP[AdvancedDevSample.Application]
        DOM[AdvancedDevSample.Domain]
        INFRA[AdvancedDevSample.Infrastructure]
        TEST[AdvancedDevSample.Test]
    end
    
    API --> APP
    API --> INFRA
    API --> JWT
    API --> SWAGGER
    
    APP --> DOM
    
    INFRA --> DOM
    INFRA --> EFCORE
    
    TEST --> API
    TEST --> APP
    TEST --> DOM
    TEST --> INFRA
    TEST --> XUNIT
    
    style External fill:#e3f2fd
    style Project fill:#f3e5f5
```

---

## Comment visualiser ces diagrammes

### Sur GitHub
Les diagrammes Mermaid sont automatiquement rendus sur GitHub dans les fichiers Markdown.

### En local avec VS Code
1. Installez l'extension "Markdown Preview Mermaid Support"
2. Ouvrez ce fichier
3. Utilisez Ctrl+Shift+V (Cmd+Shift+V sur Mac) pour prévisualiser

### En ligne
1. Copiez le code d'un diagramme
2. Allez sur [Mermaid Live Editor](https://mermaid.live/)
3. Collez le code pour le visualiser et l'exporter

### Avec JetBrains Rider
Rider supporte nativement Mermaid dans les fichiers Markdown.

---

**Date de création** : 9 février 2026  
**Version** : 1.0  
**Auteur** : AdvancedDevSample Team

