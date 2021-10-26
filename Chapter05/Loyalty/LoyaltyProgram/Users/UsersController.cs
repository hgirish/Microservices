using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyProgram.Users
{
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private static readonly Dictionary<int, LoyaltyProgramUser> RegisteredUsers = new();
        [HttpPost("")]
        public ActionResult<LoyaltyProgramUser> CreateUser([FromBody] LoyaltyProgramUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var newUser = RegisterUser(user);
            return Created(new Uri($"/users/{newUser.Id}", UriKind.Relative), newUser);
        }

        [HttpPut("{userId:int}")]
        public LoyaltyProgramUser UpdateUser(int userId, [FromBody] LoyaltyProgramUser user)
        {
            return RegisteredUsers[userId] = user;
        }

        [HttpGet("{userId:int}")]
        public ActionResult<LoyaltyProgramUser> GetUser(int userId) =>
            RegisteredUsers.ContainsKey(userId)
            ? Ok(RegisteredUsers[userId])
            : NotFound();

        private LoyaltyProgramUser RegisterUser(LoyaltyProgramUser user)
        {
            var userId = RegisteredUsers.Count;
            return RegisteredUsers[userId] = user with { Id = userId };
        }
    }
}
