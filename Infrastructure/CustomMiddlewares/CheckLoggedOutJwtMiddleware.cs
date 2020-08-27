using System.Text;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace todoapi.Infrastructure.CustomMiddlewares
{
    public class CheckLoggedOutJwtMiddleware
    {
        public RequestDelegate _next { get; }
        public CheckLoggedOutJwtMiddleware(RequestDelegate next)
        {
            this._next = next;

        }

        public async Task InvokeAsync(HttpContext context,IMemoryCache cache,ILogger<CheckLoggedOutJwtMiddleware> logger)
        {
            string headers=context.Request.Headers["Authorization"];
            if(string.IsNullOrEmpty(headers))
            {
                await _next(context);
                return;
            }
            var imme=headers.Split(" ");
            headers=imme[1];
            if(cache.Get(headers) != null)
            {
                //means it has expired so return 401
                context.Response.StatusCode=(int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("the token has been expired by user.");
                logger.LogWarning("{exevs}",context.Request.Headers["Authorization"]);
                return;
            }
            await _next(context);
        }
    }
}