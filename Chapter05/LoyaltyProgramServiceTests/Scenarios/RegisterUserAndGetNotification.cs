using LoyaltyProgram;
using LoyaltyProgram.Users;
using LoyaltyProgramServiceTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace LoyaltyProgramServiceTests.Scenarios
{
    public class RegisterUserAndGetNotification : IDisposable
    {
        private static int mocksPort = 5050;
        private readonly MocksHost serviceMock;
        private readonly IHost loyaltyProgramHost;
        private readonly HttpClient sut;

        public RegisterUserAndGetNotification()
        {
            serviceMock = new MocksHost(mocksPort);
            loyaltyProgramHost = new HostBuilder()
                .ConfigureWebHost(x => x
                .UseStartup<Startup>()
                .UseTestServer())
                .Start();
            sut = loyaltyProgramHost.GetTestClient();
        }

        [Fact]
        public async Task Scenario()
        {
            await RegisterNewUser();
            await RunConsumer();
            AssertNotificationWasSent();

        }

        private void AssertNotificationWasSent()
        {
            Assert.True(NotificationsMock.ReceiveNotification);
        }

        private async  Task RunConsumer()
        {
           await EventConsumer.EventConsumer.ConsumeBatch(0, 100,
                $"http://localhost:{mocksPort}/specialoffers",
                $"http://localhost:{mocksPort}");
        }

        private async  Task RegisterNewUser()
        {
            var actual = await sut.PostAsync("/users",
                new StringContent(
                    JsonSerializer.Serialize(
                        new LoyaltyProgramUser(
                            0, "Chr", 0, new LoyaltyProgramSettings())),
                    Encoding.UTF8,
                    "application/json"));

            Assert.Equal(HttpStatusCode.Created, actual.StatusCode);

        }

        public void Dispose()
        {
            serviceMock?.Dispose();
            
            sut?.Dispose();
            loyaltyProgramHost?.Dispose();
        }
    }
}
