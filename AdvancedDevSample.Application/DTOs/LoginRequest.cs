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
        public string Username { get; set; }

        /// <summary>
        /// Mot de passe
        /// </summary>
        public string Password { get; set; }
    }
}
