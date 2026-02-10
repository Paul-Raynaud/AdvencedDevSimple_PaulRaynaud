namespace AdvancedDevSample.Application.Services;

/// <summary>
/// Interface pour le service de génération de tokens JWT
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Génère un token JWT pour un utilisateur
    /// </summary>
    /// <param name="userId">ID de l'utilisateur</param>
    /// <param name="username">Nom d'utilisateur</param>
    /// <returns>Token JWT sous forme de chaîne</returns>
    string GenerateToken(string userId, string username);
}

