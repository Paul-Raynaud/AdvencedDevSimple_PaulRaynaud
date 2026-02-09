using AdvancedDevSample.Api.Middlewares;
using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Domain.Interfaces; // Ajouté pour IProductRepository
using AdvancedDevSample.Infrastructure.Repositories; // Ajouté pour EfProductRepository
using AdvancedDevSample.Application.Services; // Ajouté pour TokenService
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
});

// Configuration de l'authentification JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

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
});

builder.Services.AddAuthorization();

// Enregistrement des dépendances de l'application
// C'est ici que l'on lie l'interface à son implémentation pour corriger l'erreur d'exécution
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TokenService>();


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

// Middleware personnalisé pour la gestion des erreurs
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

// Rendre Program accessible pour les tests d'intégration
public partial class Program { }

