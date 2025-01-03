using Challenge.Core.Annotations;
using Challenge.Core.Constants;
using Challenge.Core.Models;
using Challenge.Core.Settings;
using Identity.API.DTOs;
using Identity.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("challenge")]
    public class ChallengeController(IChallengeClaimStore challengeClaimStore, UserManager<ApplicationUser> userManager, IOptions<ChallengeJwtSettings> jwtSettings) : ControllerBase
    {
        private readonly IChallengeClaimStore _challengeClaimStore = challengeClaimStore;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ChallengeJwtSettings _settings = jwtSettings.Value;

        [Authorize]
        [HttpPost("generate-challenge-token")]
        public async Task<ActionResult> GenerateToken([FromBody] GenerateChallengeTokenDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized("User ID claim not in JWT");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect) return Unauthorized("Incorrect creds");

            var claim = await _challengeClaimStore.GetClaim(dto.ClaimsScope);
            if (claim is null)
            {
                return BadRequest("Claim does not exist");
            }

            var token = new ChallengeToken
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                ChallengeClaimId = claim.Id,
                ChallengeClaimName = claim.Name
            };

            var (succeeded, errors, jwtToken) = await _challengeClaimStore.CreateToken(token);
            if (!succeeded) return BadRequest(errors);

            Response.Cookies.Append(
                  claim.Name,
                  jwtToken,
                  new CookieOptions
                  {
                      HttpOnly = true,
                      Secure = true,
                      SameSite = SameSiteMode.Strict,
                      Expires = token.ExpiresAt
                  }
              );

            return Ok(jwtToken);
        }


        [HttpGet("protected")]
        [Authorize]
        [Challenge(ChallengeConstants.DELETE_CASE, "test")] 
        public IActionResult ProtectedEndpoint()
        {
            return Ok("You have access!");
        }

    }
}
