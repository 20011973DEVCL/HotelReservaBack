using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace HotelReservaBack.Api.Infrastructure;

public sealed class ApiExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            PostgresException postgresException when postgresException.SqlState.StartsWith("23")
                => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        logger.LogError(exception, "Error procesando {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        httpContext.Response.StatusCode = statusCode;

        ProblemDetails problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode switch
            {
                StatusCodes.Status400BadRequest => "Solicitud inválida",
                StatusCodes.Status409Conflict => "Conflicto con los datos existentes",
                _ => "Error interno del servidor"
            },
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "Ocurrió un error inesperado."
                : exception.Message
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }
}
