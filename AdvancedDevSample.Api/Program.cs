using AdvancedDevSample.Api.Middlewares;
using System.Text;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Infrastructure.Repositories;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- AJOUT DES SERVICES AU CONTENEUR (DI) ---

builder.Services.AddControllers();

// Configuration Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;
    foreach (var xmlFile in Directory.GetFiles(basePath, "*.xml"))
    {
        options.IncludeXmlComments(xmlFile);
    }
    
    // Configuration de la sécurité JWT pour Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez votre token JWT :"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuration de l'authentification JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Si la clé secrète n'est pas configurée, on lance une exception sauf en environnement de test
if (string.IsNullOrEmpty(secretKey))
{
    var env = builder.Environment.EnvironmentName;
    // En environnement de test, on génère une clé aléatoire temporaire
    if (env == "Development" || env == "Test")
    {
        Console.WriteLine("⚠️  WARNING: Generating random JWT secret key for testing purposes only!");
        Console.WriteLine("⚠️  This should NEVER be used in production. Set JwtSettings:SecretKey via environment variables or User Secrets.");
        
        // Génération d'une clé aléatoire sécurisée de 64 bytes (512 bits)
        var randomBytes = new byte[64];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        secretKey = Convert.ToBase64String(randomBytes);
        
        // IMPORTANT : Mettre à jour la configuration pour que TokenService puisse la lire
        builder.Configuration["JwtSettings:SecretKey"] = secretKey;
    }
    else
    {
        throw new InvalidOperationException("JWT SecretKey is not configured. Please set it via environment variables or User Secrets.");
    }
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    
    // Événements de débogage pour diagnostiquer les problèmes d'authentification
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ JWT Authentication Failed: {context.Exception.Message}");
            if (context.Exception.InnerException != null)
            {
                Console.WriteLine($"   Inner Exception: {context.Exception.InnerException.Message}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("✅ JWT Token Validated Successfully");
            var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
            if (claims != null)
            {
                Console.WriteLine($"   Claims: {string.Join(", ", claims)}");
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"⚠️  JWT Challenge: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Enregistrement des dépendances de l'application
// C'est ici que l'on lie l'interface à son implémentation pour corriger l'erreur d'exécution
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();


var app = builder.Build();

// --- CONFIGURATION DU PIPELINE HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Important: L'ordre des middlewares est crucial
app.UseAuthentication();  // Doit être avant UseAuthorization
app.UseAuthorization();

// Middleware personnalisé pour la gestion des erreurs (doit être après l'authentification)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

// ⚠️ ENDPOINT DE TEST JWT - UNIQUEMENT EN DEVELOPMENT
if (app.Environment.IsDevelopment())
{
    app.MapPost("/api/auth/test-token", (ITokenService tokenService) =>
    {
        try
        {
            Console.WriteLine("🔑 Génération d'un token de test...");
            
            var token = tokenService.GenerateToken(
                userId: "test-user-123",
                username: "testuser@example.com"
            );
            
            Console.WriteLine($"✅ Token généré avec succès");
            Console.WriteLine($"   Longueur: {token.Length} caractères");
            Console.WriteLine($"   Début: {token[..Math.Min(20, token.Length)]}...");
            
            return Results.Ok(new
            {
                success = true,
                token,
                userId = "test-user-123",
                username = "testuser@example.com",
                expiresInMinutes = 60,
                message = "Token de test généré. Utilisez-le dans l'en-tête: Authorization: Bearer {token}"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erreur lors de la génération du token: {ex.Message}");
            Console.WriteLine($"   Stack: {ex.StackTrace}");
            
            return Results.Problem(
                title: "Erreur de génération du token",
                detail: $"{ex.Message}\n\nVérifiez que JwtSettings:SecretKey est configuré dans appsettings.Development.json ou via User Secrets.",
                statusCode: 500
            );
        }
    })
    .WithName("GenerateTestToken")
    .WithTags("Authentication (Dev Only)")
    .AllowAnonymous();
    
    Console.WriteLine("🔓 Endpoint de test activé: POST /api/auth/test-token");
}

await app.RunAsync();

// Rendre Program accessible pour les tests d'intégration
public partial class Program
{
    protected Program() { }
}
