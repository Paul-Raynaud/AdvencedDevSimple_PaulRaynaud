# 🚀 Guide de Démarrage Rapide - AdvancedDevSample

## ⏱️ En 5 minutes

### Étape 1 : Lancer l'application

```bash
cd AdvancedDevSample.Api
dotnet run
```

Attendez de voir :
```
✅ Now listening on: http://localhost:5069
```

### Étape 2 : Ouvrir Swagger

Dans votre navigateur : **http://localhost:5069/swagger**

### Étape 3 : S'authentifier

1. Dans Swagger, trouvez l'endpoint `POST /api/auth/login`
2. Cliquez sur **"Try it out"**
3. Utilisez ces identifiants :
```json
{
  "username": "admin",
  "password": "password"
}
```
4. Cliquez sur **"Execute"**
5. **Copiez le token** de la réponse

### Étape 4 : Autoriser Swagger

1. Cliquez sur le bouton **"Authorize" 🔓** (en haut à droite)
2. Dans le champ, entrez : `Bearer VOTRE_TOKEN`
3. Cliquez sur **"Authorize"** puis **"Close"**

### Étape 5 : Créer un produit

1. Trouvez l'endpoint `POST /api/products`
2. Cliquez sur **"Try it out"**
3. Utilisez ce body :
```json
{
  "price": 99.99
}
```
4. Cliquez sur **"Execute"**
5. ✅ Vous devriez recevoir un **201 Created**

### Étape 6 : Lister les produits

1. Trouvez l'endpoint `GET /api/products`
2. Cliquez sur **"Try it out"**
3. Cliquez sur **"Execute"**
4. ✅ Vous voyez votre produit créé !

---

## 📝 Alternative : Utiliser requests.http

Si vous utilisez **JetBrains Rider** ou **VS Code** :

### 1. Ouvrir le fichier

```
AdvancedDevSample.Api/requests.http
```

### 2. Exécuter le login

Cliquez sur le bouton ▶️ à côté de :
```http
### 1. Login - Obtenir un token JWT
POST {{baseUrl}}/api/auth/login
```

### 3. Copier le token

Dans la réponse, copiez la valeur du champ `token`.

### 4. Mettre à jour la variable

En haut du fichier, remplacez :
```http
@token = 
```

Par :
```http
@token = eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 5. Exécuter les autres requêtes

Maintenant vous pouvez exécuter toutes les autres requêtes !

---

## 🧪 Alternative : Script automatique

### 1. Rendre le script exécutable

```bash
chmod +x test-auth.sh
```

### 2. Exécuter le script

```bash
./test-auth.sh
```

Le script va :
- ✅ Se connecter automatiquement
- ✅ Récupérer un token
- ✅ Tester tous les endpoints
- ✅ Afficher les résultats

---

## 📊 Diagramme du flux

```
┌─────────────┐
│   CLIENT    │
└──────┬──────┘
       │
       │ 1. POST /api/auth/login
       │    {username, password}
       ▼
┌──────────────────┐
│  AuthController  │
└──────┬───────────┘
       │
       │ 2. Génération JWT
       ▼
┌──────────────────┐
│   TokenService   │
└──────┬───────────┘
       │
       │ 3. Retour token
       ▼
┌─────────────┐
│   CLIENT    │ ← Token stocké
└──────┬──────┘
       │
       │ 4. POST /api/products
       │    Authorization: Bearer {token}
       ▼
┌──────────────────┐
│ JWT Middleware   │ ← Validation
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│ProductsController│
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│ ProductService   │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│   Repository     │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│    Database      │
└──────────────────┘
```

---

## 🎯 Endpoints disponibles

### 🔓 Authentication (Public)

```http
POST /api/auth/login
```
- Pas d'authentification requise
- Retourne un token JWT

### 🔒 Products (Authentification requise)

```http
POST   /api/products              # Créer un produit
GET    /api/products              # Lister tous les produits  
GET    /api/products/{id}         # Obtenir un produit
PUT    /api/products/{id}         # Mettre à jour un produit
PUT    /api/products/{id}/price   # Changer le prix
DELETE /api/products/{id}         # Supprimer un produit
```

Tous nécessitent : `Authorization: Bearer {token}`

---

## 💡 Exemples de requêtes

### Créer un produit

```bash
curl -X POST http://localhost:5069/api/products \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"price": 99.99}'
```

### Lister les produits

```bash
curl -X GET http://localhost:5069/api/products \
  -H "Authorization: Bearer {token}"
```

### Mettre à jour un produit

```bash
curl -X PUT http://localhost:5069/api/products/{id} \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"price": 149.99, "isActive": true}'
```

### Changer uniquement le prix

```bash
curl -X PUT http://localhost:5069/api/products/{id}/price \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"newPrice": 199.99}'
```

### Supprimer un produit

```bash
curl -X DELETE http://localhost:5069/api/products/{id} \
  -H "Authorization: Bearer {token}"
```

---

## ❌ Erreurs courantes

### Erreur : Port déjà utilisé

```bash
# Tuer le processus sur le port 5069
lsof -ti:5069 | xargs kill -9
```

### Erreur : 401 Unauthorized

**Causes :**
- Token expiré (durée : 60 min)
- Token manquant
- Mauvais format du header

**Solution :**
```http
# ✅ Correct
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

# ❌ Incorrect (manque "Bearer")
Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Erreur : 400 Bad Request

**Causes :**
- Prix invalide (≤ 0)
- Tentative de modifier le prix d'un produit inactif
- JSON malformé

**Exemple d'erreur :**
```json
{
  "title": "Erreur métier",
  "detail": "Le prix doit être supérieur à zéro"
}
```

### Erreur : 404 Not Found

**Cause :** Le produit avec cet ID n'existe pas

**Solution :** Vérifier l'ID ou créer le produit d'abord

---

## 🧪 Tester avec Postman

### 1. Importer la collection

Créez une nouvelle collection avec ces variables :
```
baseUrl = http://localhost:5069
token = 
```

### 2. Ajouter le login

```
POST {{baseUrl}}/api/auth/login
Body (JSON):
{
  "username": "admin",
  "password": "password"
}
```

### 3. Sauvegarder le token automatiquement

Dans l'onglet **Tests** du login, ajoutez :
```javascript
pm.test("Login successful", function () {
    var jsonData = pm.response.json();
    pm.collectionVariables.set("token", jsonData.token);
});
```

### 4. Utiliser le token

Dans les autres requêtes, ajoutez le header :
```
Authorization: Bearer {{token}}
```

---

**Temps estimé** : 5-10 minutes  
**Difficulté** : Débutant  
**Prérequis** : .NET 10 SDK installé

