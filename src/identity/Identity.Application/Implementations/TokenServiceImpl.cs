using Identity.Application.Helpers;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using MassTransit.Initializers;
using Microsoft.Extensions.Options;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="ITokenService"/> - test this, as well when using it else where only use the <see cref="ITokenService"/>
    /// interface not this class
    /// </summary>
    public class TokenServiceImpl(IRoleService roleService, JwtBearerHelper jwtBearerHelper, IOptions<JwtBearerOptions> jwtBearerOptions, IdentityApplicationDbContext dbContext) : ITokenService
    {
        private readonly IRoleService _roleService = roleService;
        private readonly JwtBearerHelper _jwtBearerHelper = jwtBearerHelper;
        private readonly JwtBearerOptions _jwtBearerOptions = jwtBearerOptions.Value;
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public async Task<Tokens> IssueTokens(ApplicationUser user, Device device)
        {
            var roles = await _roleService.GetRolesAsync(user);

            string jwtBearer = _jwtBearerHelper.GenerateBearerToken(user, [.. roles.Select(x => x.Name)]);
            var refresh = _jwtBearerHelper.GenerateRefreshToken();

            var newToken = new Token
            {
                Id = refresh,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtBearerOptions.RefreshTokenExpiriesInMinutes),
                DeviceId = device.Id,
                UserId = user.Id
            };
            await _dbContext.Tokens.AddAsync(newToken);

            return new Tokens { JwtBearerToken = jwtBearer, RefreshToken = refresh };
        }
    }
}
