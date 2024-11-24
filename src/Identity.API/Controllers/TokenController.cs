using AutoMapper;
using Identity.API.DTOs;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Models;
using Identity.Infrastructure.Data.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("tokens")]
    public class TokenController(ITokenStore tokenStore, UserManager<ApplicationUser> userManager, ILogger<TokenController> logger, IdentityApplicationDbContext dbContext, IMapper mapper) : ControllerBase
    {
        private readonly ITokenStore _tokenStore = tokenStore;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<TokenController> _logger = logger;
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly IMapper _mapper = mapper;

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

            var dto = _mapper.Map<ICollection<QueryTokenDto>>(tokens);

            return Ok(dto);
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
