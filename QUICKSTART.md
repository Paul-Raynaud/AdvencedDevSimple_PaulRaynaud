# 🚀 Quick Start - AdvancedDevSample

## En 3 étapes simples

### 1️⃣ Lancer l'API
```bash
cd /Volumes/Paul_SSD/AdvancedDevSample
./start-api.sh
```

**Ou manuellement :**
```bash
cd AdvancedDevSample.Api
dotnet run
```

### 2️⃣ Ouvrir Swagger
Dans votre navigateur, allez à :
```
http://localhost:5000/swagger
```

### 3️⃣ Tester l'API
Cliquez sur un endpoint, puis sur "Try it out", et exécutez !

---

## 🧪 Lancer les tests

```bash
cd /Volumes/Paul_SSD/AdvancedDevSample
./run-tests.sh
```

**Tests spécifiques :**
```bash
./run-tests.sh --unit          # Tests unitaires seulement
./run-tests.sh --integration   # Tests d'intégration seulement
./run-tests.sh --coverage      # Avec couverture de code
```

---

## 📝 Tester avec curl

### Créer un produit
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"price": 99.99}'
```

Copiez l'`id` du produit créé, puis :

### Obtenir le produit
```bash
curl http://localhost:5000/api/products/{id}
```

### Mettre à jour le produit
```bash
curl -X PUT http://localhost:5000/api/products/{id} \
  -H "Content-Type: application/json" \
  -d '{"price": 149.99, "isActive": true}'
```

### Supprimer le produit
```bash
curl -X DELETE http://localhost:5000/api/products/{id}
```

---

## 📚 Documentation Complète

Consultez `README.md` pour plus de détails.

---

## ✅ Checklist

- [x] CRUD complet implémenté
- [x] 38 tests (unitaires + intégration)
- [x] Documentation Swagger
- [x] Architecture propre (Clean Architecture)
- [x] Scripts de démarrage
- [x] Exemples de requêtes

**Tout est prêt ! 🎉**
