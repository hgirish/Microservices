using Microsoft.AspNetCore.Http;
using Middleware;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MiddlewareTests
{
    public class RedirectingMiddleware_should
    {
        private readonly RedirectingMiddleware sut;
        public RedirectingMiddleware_should()
        {
            sut = new RedirectingMiddleware(_ => Task.CompletedTask);
        }
        [Fact]
        public async Task redirect_oldpath_to_newpath()
        {
            var ctx = new DefaultHttpContext
            {
                Request = { Path = "/oldpath" }
            };
            await sut.Invoke(ctx);

            Assert.Equal(StatusCodes.Status301MovedPermanently, ctx.Response.StatusCode);
            Assert.Equal("/newpath", ctx.Response.Headers["Location"]);
        }

        [Fact]
        public async Task not_redirect_other_paths()
        {
            var ctx = new DefaultHttpContext
            {
                Request = { Path = "/otherpath" }
            };

            await sut.Invoke(ctx);

            Assert.NotEqual(StatusCodes.Status301MovedPermanently, ctx.Response.StatusCode);
            Assert.DoesNotContain("Location", ctx.Response.Headers);
        }
    }
}
