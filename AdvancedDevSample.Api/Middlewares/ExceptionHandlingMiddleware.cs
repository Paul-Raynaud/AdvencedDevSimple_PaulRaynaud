using System.Text.Json;
using AdvancedDevSample.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net; 
using AdvancedDevSample.Domain.Exceptions;


namespace AdvancedDevSample.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


    public async Task Invoke(HttpContext context) {
            try
                {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Erreur métier.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(

                           new { title = "Erreur métier", detail = ex.Message});
               
            }
            catch (ApplicationServiceException ex)
            {
               _logger.LogError(ex, "Erreur applicative.");
                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsJsonAsync(
                           new { title = "Ressource introuvable", detail = ex.Message });
            }
            catch (InfrastructureException ex)
            {
                _logger.LogError(ex, "Erreur technique.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    JsonSerializer.Serialize( new { error = "Erreur technique" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur innatendue.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                           new { title = "Erreur serveur", detail = "Une erreur interne." });
            }
        }
    }
}

