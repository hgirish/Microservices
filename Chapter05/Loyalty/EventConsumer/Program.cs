using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var start = await GetStartIdFromDataStore();
            var end = 100;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using var resp = await client.GetAsync(new Uri($"http://localhost:5002/events?start={start}&end{end}"));
            await ProcessEvents(await resp.Content.ReadAsStreamAsync());
            await SaveStartIdToDataStore(start);

            Task<long> GetStartIdFromDataStore() => Task.FromResult(0L);

            async Task ProcessEvents(Stream content)
            {
                var events = await JsonSerializer.DeserializeAsync<SpecialOfferEvent[]>(content) ?? new SpecialOfferEvent[0];
                foreach (var evt in events)
                {
                    dynamic eventData = evt.Content;
                    if (ShouldSendNotification(eventData))
                    {
                        await SendNotification(eventData);
                    }
                    Console.WriteLine(evt);
                    start = Math.Max(start, evt.SequenceNumber + 1);
                }
            }

            Task SaveStartIdToDataStore(long startId) => Task.CompletedTask;
        }

        private static Task SendNotification(dynamic eventData)
        {
            Console.WriteLine(eventData);
            return Task.CompletedTask;
        }

        private static bool ShouldSendNotification(dynamic eventData)
        {
            return true;
        }
    }
}
