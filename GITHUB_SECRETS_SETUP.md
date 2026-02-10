# 🚀 Guide de configuration GitHub Secrets

## Étapes à suivre sur GitHub.com

### 1. Aller dans les paramètres du repository

1. Ouvrez votre repository sur **GitHub.com**
2. Cliquez sur l'onglet **Settings** (⚙️)
3. Dans le menu de gauche, cliquez sur **Secrets and variables** → **Actions**

### 2. Créer le secret JWT_SECRET_KEY

1. Cliquez sur le bouton vert **New repository secret**
2. Remplissez le formulaire :
   - **Name** : `JWT_SECRET_KEY`
   - **Secret** : `VotreCleSecreteTresLongueEtSecuriseeAvecMinimum32Caracteres!`
   
   ⚠️ **Important** : Pour la production, générez une clé plus forte :
   ```bash
   openssl rand -base64 32
   ```

3. Cliquez sur **Add secret**

### 3. Vérifier que le secret SONAR_TOKEN existe

Si ce n'est pas encore fait, créez également :

1. **New repository secret**
2. **Name** : `SONAR_TOKEN`
3. **Secret** : Votre token SonarCloud (disponible sur SonarCloud.io → My Account → Security)

### 4. Récapitulatif des secrets nécessaires

Votre repository doit avoir ces 2 secrets :

| Nom | Description | Où le trouver |
|-----|-------------|---------------|
| `JWT_SECRET_KEY` | Clé secrète pour les tokens JWT | À générer (voir ci-dessus) |
| `SONAR_TOKEN` | Token d'authentification SonarCloud | SonarCloud.io → My Account → Security → Generate Tokens |

### 5. Pusher les changements

Une fois les secrets configurés, committez et poussez :

```bash
git add .
git commit -m "fix: Remove JWT secret from config files (Security Blocker)"
git push origin main
```

### 6. Vérifier l'exécution de GitHub Actions

1. Allez dans l'onglet **Actions** de votre repository
2. Vérifiez que le workflow **SonarCloud Analysis** s'exécute
3. Si tout est correct :
   - ✅ Le build devrait réussir
   - ✅ L'analyse SonarCloud devrait être complétée
   - ✅ Le Security Blocker JWT ne devrait plus apparaître

## 🔍 Troubleshooting

### Erreur "JWT SecretKey is not configured"

- ✅ Vérifiez que le secret `JWT_SECRET_KEY` est bien créé dans GitHub
- ✅ Vérifiez l'orthographe exacte : `JWT_SECRET_KEY`
- ✅ Relancez le workflow

### Erreur SonarCloud "Cannot download quality profile"

- ✅ Vérifiez que l'organisation est bien `paul-raynaud` (en minuscules)
- ✅ Vérifiez que le projet existe sur SonarCloud.io
- ✅ Vérifiez que le `SONAR_TOKEN` est valide

### Tests en échec

- ℹ️ Les tests peuvent échouer, l'analyse SonarCloud continuera quand même (grâce au `|| true`)
- ℹ️ Vous devriez cependant corriger les tests pour avoir une CI/CD complète

## 📱 Capture d'écran du résultat attendu

Une fois tout configuré, sur SonarCloud vous devriez voir :

- 🟢 **Security Hotspots** : 0
- 🟢 Pas de "JWT secret keys should not be disclosed"
- ℹ️ Les autres issues (bugs, code smells) peuvent rester selon votre code

