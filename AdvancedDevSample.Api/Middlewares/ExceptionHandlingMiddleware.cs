using System.Text.Json;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.Extensions.Hosting;


namespace AdvancedDevSample.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;
    

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(
                           new { title = "Ressource introuvable", detail = ex.Message });
            }
            catch (InfrastructureException ex)
            {
                _logger.LogError(ex, "Erreur technique.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    JsonSerializer.Serialize( new { error = "Erreur technique" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur innatendue.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                
                object response = _env.IsDevelopment() 
                    ? new { title = "Erreur serveur", detail = ex.Message, stackTrace = ex.StackTrace }
                    : new { title = "Erreur serveur", detail = "Une erreur interne." };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
