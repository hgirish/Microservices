using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LoyaltyProgram
{
    public class ConsoleMiddleware
    {
        private readonly RequestDelegate _next;

        public ConsoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext ctx)
        {
            Console.WriteLine("Got request in class middleware");
            return _next(ctx);
        }
    }
}
