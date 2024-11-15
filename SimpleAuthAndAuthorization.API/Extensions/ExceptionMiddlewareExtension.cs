using SimpleAuthAndAuthorization.API.Middlewares;

namespace SimpleAuthAndAuthorization.API.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}