using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using SimpleAuthAndAuthorization.Core.Exceptions;

namespace SimpleAuthAndAuthorization.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
    }

    private async Task HandleExceptionsAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (ex)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Message);
                break;
            case CustomExceptions.UserNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case CustomExceptions.RoleNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new {error = ex.Message});
        }
        
        await context.Response.WriteAsync(result);
    }
    
}