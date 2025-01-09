using Identity.API.DTOs;
using Identity.API.Mappings;
using Identity.Core.Models;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("tokens")]
    public class TokenController(ITokenStore tokenStore, UserManager<ApplicationUser> userManager, ILogger<TokenController> logger, IdentityApplicationDbContext dbContext) : ControllerBase
    {
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<TokenController> _logger = logger;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly TokenMapping tokenMapping = new();

        [Authorize]
        [HttpPost("clean-expired-tokens")]
        public async Task<ActionResult> CleanExpiredTokens()
        {
            var count = await _tokenStore.CleanupExpiredTokensAsync();

            _logger.LogInformation("Clean expired token {count}", count);

            return Ok(count);
        }

        /// <summary>
        /// Return user tokens based on query
        /// </summary>
        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult> GetUserTokens(string userId, [FromQuery] UserTokensQuery query)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound("User not found.");

            var tokenQuery = _dbcontext.Tokens.AsQueryable();

            if (query.RefreshTokenExpiresAt.HasValue)
            {
                tokenQuery = tokenQuery.Where(x => x.RefreshTokenExpiresAt <= query.RefreshTokenExpiresAt);
            }

            if (query.IsRevoked.HasValue)
            {
                tokenQuery = tokenQuery.Where(x => x.IsRevoked == query.IsRevoked);
            }

            if (query.RevokedAt.HasValue)
            {
                tokenQuery = tokenQuery.Where(x => x.RevokedAt <= query.RevokedAt);
            }

            if (!string.IsNullOrWhiteSpace(query.RevokedReason))
            {
                tokenQuery = tokenQuery.Where(x => x.RevokedReason.Contains(query.RevokedReason));
            }

            if (query.IsBlackListed.HasValue)
            {
                tokenQuery = tokenQuery.Where(x => x.IsBlackListed == query.IsBlackListed);
            }

            if (query.CreatedAt.HasValue)
            {
                tokenQuery = tokenQuery.Where(x => x.CreatedAt <= query.CreatedAt);
            }

            tokenQuery = tokenQuery.Where(x => x.UserId == user.Id);

            var tokens = await tokenQuery.ToListAsync();

            var dto = tokens.Select(x =>
            {
                return tokenMapping.ToDto(x);
            });

            return Ok(dto);
        }

        [Authorize]
        [HttpDelete("{tokenId}/revoke")]
        public async Task<ActionResult> RevokeToken(string tokenId)
        {
            var token = await _tokenStore.GetTokenById(tokenId);
            if (token is null) return NotFound("Tokem not found");

            await _tokenStore.RevokeTokenAsync(token);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{tokenId}/deviceInfo")]
        public async Task<ActionResult> GetDeviceInfoForTokenId(string tokenId)
        {
            return Ok();
        }
    }
}
