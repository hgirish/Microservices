using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialOffers.EventFeed
{
    [Route("/events")]
    public class EventFeedController : ControllerBase
    {
        private readonly IEventStore _eventStore;

        public EventFeedController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [HttpGet("")]
        public ActionResult<EventFeedEvent[]> GetEvents([FromQuery]int start, [FromQuery]int end)
        {
            if (start < 0 || end < start)
            {
                return BadRequest();
            }
            return _eventStore.GetEvents(start, end).ToArray();
        }
    }
}
