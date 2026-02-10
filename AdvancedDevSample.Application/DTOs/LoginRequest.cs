namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête de connexion avec identifiants
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Mot de passe
        /// </summary>
        public required string Password { get; set; }
    }
}
