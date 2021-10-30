using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventConsumer
{
 public static   class EventConsumer
    {
        public static async Task ConsumeBatch(int start, int end, string specialOffersHostName, string notificationsHostName)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var resp = await client.GetAsync(new Uri($"{specialOffersHostName}/events?start={start}&end={end}"));

            var events = await JsonSerializer.DeserializeAsync<dynamic[]>(
                await resp.Content.ReadAsStreamAsync())
                ?? Array.Empty<dynamic>();

            foreach (var evt in events)
            {
                // Match special offer in evt to registered users and send notification to matching user.
                await client.PostAsync($"{notificationsHostName}/notify", new StringContent(""));
            }
        }
    }
}
