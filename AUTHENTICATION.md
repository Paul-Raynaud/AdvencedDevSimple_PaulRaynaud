# Guide d'authentification JWT

## 🔐 Présentation

Ce projet utilise l'authentification JWT (JSON Web Token) pour sécuriser les endpoints de l'API. Toutes les routes de gestion des produits nécessitent un token JWT valide.

## 🚀 Comment utiliser l'authentification

### 1. Obtenir un token JWT

Envoyez une requête POST à `/api/auth/login` avec des identifiants valides :

```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password"
}
```

**Réponse attendue :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-09T12:53:28Z"
}
```

### 2. Utiliser le token dans vos requêtes

Ajoutez l'en-tête `Authorization` à toutes vos requêtes :

```http
GET http://localhost:5000/api/products
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

⚠️ **Important** : N'oubliez pas le préfixe `Bearer` suivi d'un espace avant le token !

### 3. Identifiants de test disponibles

Pour le développement, les identifiants suivants sont disponibles :

| Username | Password | Description |
|----------|----------|-------------|
| admin    | password | Administrateur |
| user     | password | Utilisateur standard |

## 📝 Configuration JWT

Les paramètres JWT sont configurés dans `appsettings.json` :

```json
"JwtSettings": {
  "SecretKey": "VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!",
  "Issuer": "AdvancedDevSample",
  "Audience": "AdvancedDevSampleClient",
  "ExpirationInMinutes": 60
}
```

## 🧪 Tester avec Swagger

1. Lancez l'application : `dotnet run`
2. Ouvrez Swagger : http://localhost:5000/swagger
3. Cliquez sur le bouton **"Authorize"** 🔓 (en haut à droite)
4. Entrez votre token dans le format : `Bearer votre_token_ici`
5. Cliquez sur **"Authorize"** puis **"Close"**
6. Toutes vos requêtes incluront automatiquement le token !

## 🧪 Tester avec le fichier requests.http

Le fichier `requests.http` contient des exemples de requêtes :

1. Exécutez la requête `### 1. Login - Obtenir un token JWT`
2. Copiez le token de la réponse
3. Remplacez `@token = ` par `@token = votre_token_copié` en haut du fichier
4. Exécutez les autres requêtes qui utiliseront automatiquement le token

## ❌ Erreurs courantes

### Erreur 401 Unauthorized

**Causes possibles :**
- Token absent ou invalide
- Token expiré (durée de vie : 60 minutes)
- Mauvais format de l'en-tête Authorization
- Token non préfixé par "Bearer "

**Solution :**
```http
# ✅ Correct
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

# ❌ Incorrect
Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Erreur 403 Forbidden

Vous êtes authentifié mais n'avez pas les droits nécessaires pour cette action.

## 🔍 Débogage

L'application affiche des logs de débogage dans la console pour vous aider à diagnostiquer les problèmes d'authentification :

```
✅ JWT Token Validated Successfully
   Claims: sub: 1, unique_name: admin, jti: abc123...
```

ou

```
❌ JWT Authentication Failed: The token expired at '02/09/2026 11:53:28'
```

## 🔧 Architecture technique

### Structure du token JWT

Le token contient les claims suivants :
- `sub` (Subject) : ID de l'utilisateur
- `unique_name` : Nom d'utilisateur
- `jti` (JWT ID) : Identifiant unique du token


