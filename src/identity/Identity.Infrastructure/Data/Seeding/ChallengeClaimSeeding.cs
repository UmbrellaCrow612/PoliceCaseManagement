using Challenge.Core.Constants;
using Challenge.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Seeding
{
    public class ChallengeClaimSeeding(IdentityApplicationDbContext identityApplicationDbContext)
    {
        private readonly IdentityApplicationDbContext _identityApplicationDbContext = identityApplicationDbContext;
        public async Task Seed()
        {
            var prevClaims = await _identityApplicationDbContext.ChallengeClaims.ToListAsync();
            _identityApplicationDbContext.RemoveRange(prevClaims);
            await _identityApplicationDbContext.SaveChangesAsync();

            var claims = ChallengeConstants.Challenges.Select(c => new ChallengeClaim { Name = c }).ToList();

            await _identityApplicationDbContext.ChallengeClaims.AddRangeAsync(claims);
            await _identityApplicationDbContext.SaveChangesAsync();
        }
    }
}
