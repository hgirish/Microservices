using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Middleware
{
    public class RedirectingMiddleware
    {
        private readonly RequestDelegate _next;
        public RedirectingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext ctx)
        {
            switch (ctx.Request.Path.Value?.TrimEnd('/'))
            {
                case "/oldpath":
                    {
                        ctx.Response.Redirect("/newpath", permanent: true);
                        return Task.CompletedTask;
                    }

                default:
                    return _next(ctx);
            }
        }
    }
}
