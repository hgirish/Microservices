using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<LoyaltyProgramUser> UpdateUser(int userId, [FromBody] LoyaltyProgramUser user)
        {
            var hasUserId = int.TryParse(
                User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value, out var userIdFromToken);
            if (!hasUserId || userId != userIdFromToken)
            {
                return Unauthorized();
            }
            return RegisteredUsers[userId] = user;
        }

        [HttpGet("{userId:int}")]
        public ActionResult<LoyaltyProgramUser> GetUser(int userId) =>
            RegisteredUsers.ContainsKey(userId)
            ? Ok(RegisteredUsers[userId])
            : NotFound();

        [HttpGet("fail")]
        public IActionResult Fail() => throw new NotImplementedException();

        private LoyaltyProgramUser RegisterUser(LoyaltyProgramUser user)
        {
            var userId = RegisteredUsers.Count;
            return RegisteredUsers[userId] = user with { Id = userId };
        }
    }
}
