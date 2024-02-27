using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace NST.Middleware
{
    public static class ApiMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlerApiMiddleware>();

            return builder;
        }
    }

    public class ExceptionHandlerApiMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message, stackTrace = ex.ToString() });
            }
            catch (BadDataException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message, stackTrace = ex.ToString() });
            }
            catch (UnAuthorizeException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message, stackTrace = ex.ToString() });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "Unexpected server error: " + ex.Message, stackTrace = ex.ToString() });
            }
        }
    }
}