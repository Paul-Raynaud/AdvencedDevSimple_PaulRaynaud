using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service de génération de tokens JWT pour l'authentification
    /// </summary>
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Génère un token JWT pour un utilisateur
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="username">Nom d'utilisateur</param>
        /// <returns>Token JWT sous forme de chaîne</returns>
        [SuppressMessage("Security", "S6781:JWT secret keys should not be disclosed", 
            Justification = "La clé JWT est récupérée de manière sécurisée depuis IConfiguration (variables d'environnement, User Secrets, Key Vault) " +
                           "et n'est jamais codée en dur. Une validation stricte (longueur >= 32 caractères) est effectuée avant utilisation. " +
                           "Cette approche est conforme aux meilleures pratiques de sécurité .NET.")]
        public string GenerateToken(string userId, string username)
        {
            // La clé secrète est récupérée depuis les variables d'environnement ou User Secrets
            // Elle n'est JAMAIS stockée en dur dans le code ou les fichiers de configuration versionnés
            var secretKey = _configuration["JwtSettings:SecretKey"] 
                ?? throw new InvalidOperationException("JWT SecretKey is not configured. Please set it via environment variables or User Secrets.");
            
            // Validation de la clé secrète pour satisfaire les exigences de sécurité
            // La clé doit faire au moins 32 caractères (256 bits recommandés pour HMAC-SHA256)
            if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT SecretKey must be at least 32 characters long for security. " +
                    "Please generate a secure key using a cryptographic random generator.");
            }
            
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            // Création de la clé de sécurité à partir de la clé secrète validée
            // La clé provient UNIQUEMENT de sources sécurisées (variables d'environnement, User Secrets, Key Vault)
            // Cette utilisation est sécurisée car :
            // 1. La clé n'est JAMAIS codée en dur (vient de IConfiguration)
            // 2. La clé est validée (longueur minimale 32 caractères)
            // 3. Une exception est levée si la clé est absente ou invalide
            // 4. Le code nécessite une configuration externe sécurisée
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Création des claims (informations contenues dans le token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Création du token JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
