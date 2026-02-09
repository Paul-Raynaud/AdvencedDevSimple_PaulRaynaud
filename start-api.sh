#!/bin/bash

# Script de lancement de l'API AdvancedDevSample
# Usage: ./start-api.sh

echo "🚀 Démarrage de AdvancedDevSample API..."
echo ""

# Arrêter toute instance en cours
echo "🛑 Arrêt des instances précédentes..."
pkill -f "dotnet.*AdvancedDevSample.Api" 2>/dev/null || true
sleep 1

# Se déplacer dans le répertoire de l'API
cd "$(dirname "$0")/AdvancedDevSample.Api" || exit 1

# Restaurer les packages si nécessaire
echo "📦 Vérification des packages..."
dotnet restore --verbosity quiet

# Construire le projet
echo "🔨 Construction du projet..."
dotnet build --configuration Release --verbosity quiet

# Lancer l'API
echo ""
echo "✅ Lancement de l'API..."
echo "📚 Swagger sera disponible à : http://localhost:5000/swagger"
echo "🌐 API disponible à : http://localhost:5000/api/products"
echo ""
echo "Appuyez sur Ctrl+C pour arrêter l'API"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

dotnet run --configuration Release --urls "http://localhost:5000"
