using Identity.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("tokens")]
    public class TokenController : ControllerBase
    {
        [Authorize]
        [HttpPost("clean-expired-tokens")]
        public async Task<ActionResult> CleanExpiredTokens()
        {
            return Ok();
        }

        /// <summary>
        /// Return user tokens based on query
        /// </summary>
        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult> GetUserTokens(string userId, [FromQuery] UserTokensQuery query)
        {
            return Ok();
        }

        [Authorize]
        [HttpPatch("{tokenId}")]
        public async Task<ActionResult> UpdateTokenById(string tokenId)
        {
            // here they blaclist or revoke tokens from specific user id and token id
            return Ok();
        }

        [Authorize]
        [HttpGet("{tokenId}/deviceInfo")]
        public async Task<ActionResult> GetDeviceInfoForTokenId(string tokenId)
        {
            return Ok();
        }
    }
}
