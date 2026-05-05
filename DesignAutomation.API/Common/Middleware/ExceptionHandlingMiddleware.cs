using System.Net;
using System.Text.Json;
using DesignAutomation.API.Common.Exceptions;

namespace DesignAutomation.API.Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteAsync(context, HttpStatusCode.BadRequest, new { message = ex.Message, errors = ex.Errors });
        }
        catch (BadRequestException ex)
        {
            await WriteAsync(context, HttpStatusCode.BadRequest, new { message = ex.Message, errors = ex.Errors });
        }
        catch (NotFoundException ex)
        {
            await WriteAsync(context, HttpStatusCode.NotFound, new { message = ex.Message });
        }
        catch (AuthenticationException ex)
        {
            await WriteAsync(context, HttpStatusCode.Unauthorized, new { message = ex.Message });
        }
        catch (ForbiddenException ex)
        {
            await WriteAsync(context, HttpStatusCode.Forbidden, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteAsync(context, HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }

    private static Task WriteAsync(HttpContext context, HttpStatusCode status, object payload)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }));
    }
}
