using System.Net;
using System.Text.Json;

namespace Practica2.Api
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new { error = ex.Message };
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
