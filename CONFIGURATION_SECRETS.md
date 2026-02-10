# Configuration des Secrets JWT

## 🔐 Problème de sécurité résolu

La clé secrète JWT a été retirée du fichier `appsettings.json` pour des raisons de sécurité (détecté par SonarCloud comme Security Blocker).

## 📋 Configuration en Local (Développement)

### Option 1 : User Secrets (Recommandé)

1. **Naviguez vers le projet API** :
   ```bash
   cd AdvancedDevSample.Api
   ```

2. **Configurez le secret** :
   ```bash
   dotnet user-secrets set "JwtSettings:SecretKey" "VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!"
   ```

3. **Vérifiez la configuration** :
   ```bash
   dotnet user-secrets list
   ```

### Option 2 : Variables d'environnement

1. **Créez un fichier `.env`** à la racine du projet (déjà dans .gitignore) :
   ```bash
   cp .env.example .env
   ```

2. **Modifiez le fichier `.env`** avec votre clé secrète :
   ```env
   JwtSettings__SecretKey=VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!
   ```

3. **Chargez les variables avant de lancer l'application** :
   ```bash
   export $(cat .env | xargs) && dotnet run --project AdvancedDevSample.Api
   ```

### Option 3 : launchSettings.json (pour développement uniquement)

Ajoutez dans `AdvancedDevSample.Api/Properties/launchSettings.json` :
```json
{
  "profiles": {
    "http": {
      "environmentVariables": {
        "JwtSettings__SecretKey": "VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!"
      }
    }
  }
}
```

## 🚀 Configuration en Production (GitHub Actions)

### 1. Créer le secret dans GitHub

1. Allez dans votre repository GitHub
2. **Settings** → **Secrets and variables** → **Actions**
3. Cliquez sur **New repository secret**
4. **Name** : `JWT_SECRET_KEY`
5. **Value** : `VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!` (ou une clé encore plus forte)
6. Cliquez sur **Add secret**

### 2. Le workflow GitHub Actions

Le workflow `.github/workflows/sonarcloud.yml` est déjà configuré pour utiliser ce secret :

```yaml
env:
  JwtSettings__SecretKey: ${{ secrets.JWT_SECRET_KEY }}
```

## 🔑 Générer une clé secrète forte

Pour générer une clé secrète JWT sécurisée :

```bash
# Option 1 : Avec OpenSSL
openssl rand -base64 32

# Option 2 : Avec PowerShell (Windows)
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | % {[char]$_})

# Option 3 : En C#
using System.Security.Cryptography;
Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
```

## ✅ Vérification

Pour vérifier que tout fonctionne :

1. **Lancez l'application** :
   ```bash
   dotnet run --project AdvancedDevSample.Api
   ```

2. **Testez l'authentification** via Swagger à l'adresse : http://localhost:5000/swagger

3. **Si vous obtenez l'erreur** "JWT SecretKey is not configured" :
   - Vérifiez que vous avez bien configuré le secret (option 1, 2 ou 3)
   - Redémarrez l'application

## 📝 Notes importantes

- ⚠️ **Ne committez JAMAIS** la clé secrète dans Git
- ✅ Le fichier `.env` est dans `.gitignore`
- ✅ Les User Secrets sont stockés localement hors du repository
- ✅ Les GitHub Secrets sont chiffrés et sécurisés
- 🔒 Utilisez une clé différente pour chaque environnement (dev, staging, production)

