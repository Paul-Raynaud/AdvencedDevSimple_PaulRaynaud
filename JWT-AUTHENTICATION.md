# Authentification JWT dans AdvancedDevSample

## 📝 Vue d'ensemble

L'authentification JWT (JSON Web Token) a été mise en place pour sécuriser les API du projet. Toutes les routes du contrôleur `ProductsController` nécessitent maintenant un token JWT valide.

## 🔑 Fonctionnement

### 1. Architecture

- **TokenService** : Service qui génère les tokens JWT
- **AuthController** : Contrôleur pour l'authentification et la génération de tokens
- **ProductsController** : Protégé par l'attribut `[Authorize]`
- **Configuration JWT** : Définie dans `appsettings.json`

### 2. Configuration

Les paramètres JWT sont configurés dans `appsettings.json` :

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

**⚠️ Important** : En production, la `SecretKey` doit être stockée de manière sécurisée (variables d'environnement, Azure Key Vault, etc.)

## 🚀 Utilisation

### Étape 1 : Démarrer l'API

```bash
# Depuis la racine du projet
./start-api.sh

# Ou directement
cd AdvancedDevSample.Api
dotnet run
```

L'API démarre sur `https://localhost:7086` (ou le port configuré)

### Étape 2 : Obtenir un token JWT

**Endpoint** : `POST /api/auth/login`

**Identifiants de test** :
- Username: `admin` / Password: `password`
- Username: `user` / Password: `password`

**Exemple de requête avec cURL** :
```bash
curl -X POST "https://localhost:7086/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

**Réponse** :
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "expiresAt": "2026-02-09T10:28:00Z"
}
```

### Étape 3 : Utiliser le token pour accéder aux APIs protégées

**Avec cURL** :
```bash
# Récupérer tous les produits
curl -X GET "https://localhost:7086/api/products" \
  -H "Authorization: Bearer VOTRE_TOKEN_ICI"

# Créer un produit
curl -X POST "https://localhost:7086/api/products" \
  -H "Authorization: Bearer VOTRE_TOKEN_ICI" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Nouveau Produit",
    "description": "Description du produit",
    "price": 99.99,
    "stock": 50
  }'
```

**Avec Swagger UI** :
1. Ouvrez `https://localhost:7086/swagger`
2. Cliquez sur le bouton **"Authorize"** 🔒 en haut à droite
3. Dans le champ "Value", entrez : `Bearer VOTRE_TOKEN_ICI`
4. Cliquez sur **"Authorize"** puis **"Close"**
5. Toutes vos requêtes incluront maintenant automatiquement le token

## 📊 Structure du Token JWT

Le token contient les claims suivants :

```json
{
  "sub": "1",              // ID de l'utilisateur
  "unique_name": "admin",  // Nom d'utilisateur
  "jti": "unique-id",      // ID unique du token
  "iss": "AdvancedDevSample",
  "aud": "AdvancedDevSampleClient",
  "exp": 1707478080        // Timestamp d'expiration
}
```

## 🔒 Sécurité

### Endpoints publics
- `POST /api/auth/login` : Accessible sans token (pour obtenir un token)

### Endpoints protégés
- Tous les endpoints de `/api/products/*` nécessitent un token JWT valide

### Réponse en cas d'absence de token
```
Status: 401 Unauthorized
```

### Réponse en cas de token invalide ou expiré
```
Status: 401 Unauthorized
```

## 🛠️ Personnalisation

### Ajouter des rôles aux utilisateurs

Modifiez le `TokenService.GenerateToken` pour ajouter des claims de rôles :

```csharp
var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, userId),
    new Claim(JwtRegisteredClaimNames.UniqueName, username),
    new Claim(ClaimTypes.Role, "Admin"), // Ajout d'un rôle
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};
```

### Protéger par rôle spécifique

```csharp
[Authorize(Roles = "Admin")]
[HttpPost]
public IActionResult CreateProduct([FromBody] CreateProductRequest request)
{
    // Seuls les utilisateurs avec le rôle "Admin" peuvent accéder
}
```

### Modifier la durée de validité du token

Dans `appsettings.json`, changez la valeur :
```json
"ExpirationInMinutes": 120  // Token valide 2 heures
```

## 🧪 Tests

### Test avec requests.http

Ajoutez dans `requests.http` :

```http
### Obtenir un token
POST https://localhost:7086/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}

### Utiliser le token pour récupérer les produits
GET https://localhost:7086/api/products
Authorization: Bearer {{token}}
```

## 📝 TODO pour une implémentation en production

1. **Base de données utilisateurs** : Remplacer les identifiants en dur par une vraie base de données
2. **Hashage des mots de passe** : Utiliser BCrypt ou Argon2 pour hasher les mots de passe
3. **Refresh tokens** : Implémenter un système de refresh tokens pour renouveler les tokens expirés
4. **Rate limiting** : Limiter les tentatives de connexion
5. **Logging** : Logger les tentatives d'authentification réussies/échouées
6. **Variables d'environnement** : Stocker la clé secrète de manière sécurisée
7. **HTTPS obligatoire** : Forcer HTTPS en production
8. **Révocation de tokens** : Implémenter un système de blacklist/whitelist de tokens

## 🔗 Ressources

- [Documentation JWT](https://jwt.io/)
- [ASP.NET Core Authentication](https://docs.microsoft.com/aspnet/core/security/authentication/)
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/)
