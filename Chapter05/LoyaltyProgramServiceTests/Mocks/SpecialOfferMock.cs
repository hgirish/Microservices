﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyProgramServiceTests.Mocks
{
    public  class SpecialOfferMock : ControllerBase
    {
        [HttpGet("/specialoffers/events")]
        public ActionResult<object[]> GetEvents([FromQuery] int start, [FromQuery] int end)
        {
            return new[]
            {
                new
                {
                    SequenceNumber = 1,
                    Name = "baz",
                    Content = new
                    {
                        OfferName = "foo",
                        Description="bar",
                        Item = new{ProductName = "name"}
                    }
                }
            };
        }
    }
}
