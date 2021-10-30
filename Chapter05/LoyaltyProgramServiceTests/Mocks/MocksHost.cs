using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace LoyaltyProgramServiceTests.Mocks
{
    public class MocksHost : IDisposable
    {
        private readonly IHost hostForMocks;

        public MocksHost(int port)
        {
            hostForMocks =
                Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                x.ConfigureServices(services => services.AddControllers())
                .Configure(app => app.UseRouting()
                .UseEndpoints(endpoint=> endpoint.MapControllers()))
                .UseUrls($"http://localhost:{port}"))
                .Build();

            new Thread(() => hostForMocks.Run()).Start();

        }
        public void Dispose()
        {
            hostForMocks.Dispose();
        }
    }
}
