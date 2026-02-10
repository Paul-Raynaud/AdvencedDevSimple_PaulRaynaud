using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour l'authentification et la génération de tokens JWT
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        /// <summary>
        /// Authentifie un utilisateur et génère un token JWT
        /// </summary>
        /// <param name="request">Identifiants de connexion</param>
        /// <returns>Token JWT si les identifiants sont valides</returns>
        /// <remarks>
        /// Utilisateurs de test:
       /// - Username: admin, Password: password
        /// - Username: user, Password: password
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validation d'authentification en dur pour la démo
            // En production, ceci devrait être remplacé par une validation avec base de données et hachage de mot de passe
            if ((request.Username == "admin" && request.Password == "password") ||
                (request.Username == "user" && request.Password == "password"))
            {
                var token = _tokenService.GenerateToken(
                    userId: request.Username == "admin" ? "1" : "2",
                    username: request.Username
                );

                var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");
                
                return Ok(new LoginResponse
                {
                    Token = token,
                    Username = request.Username,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
                });
            }

            return Unauthorized(new { message = "Identifiants invalides" });
        }
    }
}
