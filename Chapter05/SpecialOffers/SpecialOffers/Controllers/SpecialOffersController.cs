using Microsoft.AspNetCore.Mvc;
using SpecialOffers.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialOffers.Controllers
{
    public class SpecialOffersController : ControllerBase
    {
        private readonly IEventStore _eventStore;

        private readonly IDictionary<int, Offer> _offers = new Dictionary<int, Offer>();

        public SpecialOffersController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [HttpGet("{id: int}")]
        public ActionResult<Offer> GetOffer(int id) => _offers.ContainsKey(id) ? (ActionResult<Offer>)Ok(_offers[id]) : NotFound();

        [HttpPost("")]
        public ActionResult<Offer> CreateOffer([FromBody] Offer offer)
        {
            if (offer == null)
            {
                return BadRequest();
            }
            var newUser = NewOffer(offer);
            return Created(new Uri($"/offers/{newUser.Id}", UriKind.Relative), newUser);
        }

        [HttpPut("{id:int}")]
        public Offer UploadOffer(int id, [FromBody]Offer offer)
        {
            var offerWithId = offer with { Id = id };
            _eventStore.RaiseEvent("SpecialOfferUpdated", new { OldOfffer = _offers[id], NewOffer = offerWithId });
            return _offers[id] = offerWithId;
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteOffer(int id)
        {
            _eventStore.RaiseEvent("SpecialOfferRemoved", new { Offer = _offers[id] });
            _offers.Remove(id);
            return Ok();

        }

        private Offer NewOffer(Offer offer)
        {
            var offerId = _offers.Count;
            var newOffer = offer with { Id = offerId };
            _eventStore.RaiseEvent("SpecialOfferCreated", newOffer);
            return _offers[offerId] = newOffer;
        }

    }

    public record Offer(string Description, int Id);
}
