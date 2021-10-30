using SpecialOffers.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyProgramUnitTests
{
    public class FakeEventStore : IEventStore
    {
        public IEnumerable<EventFeedEvent> GetEvents(int start, int end)
        {
            if (start > 100)
            {
                
                    Enumerable.Empty<EventFeedEvent>();
            }
            return Enumerable.Range(start, end - start)
                .Select(i => new EventFeedEvent
                (
                    i,
                    DateTimeOffset.UtcNow,
                    "some event",
                    new object()
                ));
        }

        public void RaiseEvent(string name, object content)
        {
            
        }
    }
}
