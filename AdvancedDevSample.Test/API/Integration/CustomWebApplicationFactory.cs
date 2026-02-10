using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdvancedDevSample.Test.API.Integration;

/// <summary>
/// Factory personnalisée pour les tests d'intégration avec configuration de test
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            // Ajouter une configuration de test avec une clé JWT de test
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:SecretKey"] = "ThisIsATestSecretKeyForIntegrationTestsOnly123456789",
                ["JwtSettings:Issuer"] = "AdvancedDevSample.Test",
                ["JwtSettings:Audience"] = "AdvancedDevSample.Test",
                ["JwtSettings:ExpirationInMinutes"] = "60"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remplacer l'authentification JWT par une authentification de test sans vérification
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", _ => { });
            
            // Désactiver l'autorisation pour les tests
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true) // Toujours autoriser
                    .Build();
            });
        });
    }
}



