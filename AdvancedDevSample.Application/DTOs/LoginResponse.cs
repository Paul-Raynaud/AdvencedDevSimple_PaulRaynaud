namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Réponse de connexion contenant le token JWT
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Token JWT à utiliser pour les requêtes authentifiées
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Date d'expiration du token
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
