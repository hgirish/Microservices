using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SpecialOffers.EventFeed
{
    public class EventStore : IEventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<EventFeedEvent> Database = new List<EventFeedEvent>();
        public IEnumerable<EventFeedEvent> GetEvents(int start, int end)
        {
            return Database.Where(e => start <= e.SequenceNumber && e.SequenceNumber < end)
                .OrderBy(e => e.SequenceNumber);
        }

        public void RaiseEvent(string name, object content)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            Database.Add(new EventFeedEvent(seqNumber, DateTimeOffset.UtcNow, name, content));
        }
    }
}
