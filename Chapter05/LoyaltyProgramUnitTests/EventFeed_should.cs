using Microsoft.Extensions.Hosting;
using SpecialOffers.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Text.Json;

namespace LoyaltyProgramUnitTests
{
    public class EventFeed_should : IDisposable
    {
        private readonly IHost _host;
        private readonly HttpClient sut;

        public EventFeed_should()
        {
            _host = new HostBuilder()
                .ConfigureWebHost(host => 
                host
                .ConfigureServices(x=> x
                .AddScoped<IEventStore,FakeEventStore>()
                .AddControllersByType(typeof(EventFeedController))
                .AddApplicationPart(typeof(EventFeedController).Assembly)
                )
                .Configure(x => x.UseRouting()
                .UseEndpoints(opt => opt.MapControllers()))
                .UseTestServer())
                .Start();
            sut = _host.GetTestClient();
        }

        [Fact]
        public async Task return_empty_response_when_there_are_no_more_events()
        {
            var actual = await sut.GetAsync("/events?start=200&end=300");

            var eventFeedEvents = await JsonSerializer.DeserializeAsync<IEnumerable<EventFeedEvent>>(
                await actual.Content.ReadAsStreamAsync());
            Assert.Empty(eventFeedEvents);
        }


        [Fact]
        public async Task return_list_of_events_when_there_are_events()
        {
            int start = 1;
            int end = 10;
            var actual = await sut.GetAsync($"/events?start={start}&end={end}");

            var eventContent = await actual.Content.ReadAsStreamAsync();

            var eventFeedEvents = await JsonSerializer.DeserializeAsync<IEnumerable<EventFeedEvent>>(eventContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotEmpty(eventFeedEvents);
            Assert.Equal(end- start, eventFeedEvents.Count());
        }

        public void Dispose()
        {
            _host?.Dispose();
            sut?.Dispose();
        }
    }
}
