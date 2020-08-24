using Microsoft.AspNetCore.Builder;
using todoapi.Infrastructure.CustomMiddlewares;

namespace todoapi.Infrastructure.Extensions
{
    public static class LoggedOutMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckedLoggedOutTokens(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckLoggedOutJwtMiddleware>();
        }
    }
}