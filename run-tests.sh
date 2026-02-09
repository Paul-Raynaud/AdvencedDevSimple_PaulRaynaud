#!/bin/bash

# Script pour exécuter les tests du projet AdvancedDevSample
# Usage: ./run-tests.sh [options]
# Options:
#   --unit        Tests unitaires seulement
#   --integration Tests d'intégration seulement
#   --coverage    Avec couverture de code
#   --watch       Mode watch (re-exécution automatique)

echo "🧪 Exécution des tests AdvancedDevSample"
echo ""

cd "$(dirname "$0")" || exit 1

# Restaurer les packages si nécessaire
echo "📦 Vérification des packages..."
dotnet restore --verbosity quiet

case "$1" in
    --unit)
        echo "🔬 Exécution des tests unitaires uniquement..."
        dotnet test --filter "FullyQualifiedName~Domain|FullyQualifiedName~Application" --verbosity normal
        ;;
    --integration)
        echo "🌐 Exécution des tests d'intégration uniquement..."
        dotnet test --filter "FullyQualifiedName~Integration" --verbosity normal
        ;;
    --coverage)
        echo "📊 Exécution des tests avec couverture de code..."
        dotnet test --collect:"XPlat Code Coverage" --verbosity normal
        echo ""
        echo "📈 Rapport de couverture généré dans : AdvancedDevSample.Test/TestResults/"
        ;;
    --watch)
        echo "👀 Mode watch activé (Ctrl+C pour arrêter)..."
        dotnet watch test --project AdvancedDevSample.Test
        ;;
    *)
        echo "🎯 Exécution de tous les tests..."
        echo ""
        dotnet test --verbosity normal
        ;;
esac

echo ""
echo "✅ Tests terminés !"
