using LoyaltyProgram;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LoyaltyProgramUnitTests
{
    public class UsersEndpoints_should : IDisposable
    {
        private readonly IHost _host;
        private readonly HttpClient sut;

        public UsersEndpoints_should()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(x => x.UseStartup<Startup>()
                .UseTestServer())
                .Start();
            sut = _host.GetTestClient();
        }
        [Fact]
        public async Task respond_not_found_when_queried_for_unregistered_user()
        {
            var actual = await sut.GetAsync("/users/1000");
            Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
        }
        public void Dispose()
        {
            _host?.Dispose();
            sut?.Dispose();
        }
    }
}
