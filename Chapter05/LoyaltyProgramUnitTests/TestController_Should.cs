using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Net;

namespace LoyaltyProgramUnitTests
{
    public class TestController_Should : IDisposable
    {
        private readonly IHost _host;
        private readonly HttpClient _httpClient;

        public class TestController: ControllerBase
        {
            [HttpGet("/")]
            public OkResult Get() => Ok();
        }
        public TestController_Should()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(host => 
                host.ConfigureServices(x => x.AddControllersByType(typeof(TestController)))
                .Configure(x => x.UseRouting()
                .UseEndpoints(opt => opt.MapControllers()))
                .UseTestServer())
                .Start();
            _httpClient = _host.GetTestClient();
        }
        public void Dispose()
        {
            _host?.Dispose();
            _httpClient?.Dispose();
        }

        [Fact]
        public async Task respond_ok_to_request_to_rootAsync()
        {
            var actual = await _httpClient.GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }
    }
}
