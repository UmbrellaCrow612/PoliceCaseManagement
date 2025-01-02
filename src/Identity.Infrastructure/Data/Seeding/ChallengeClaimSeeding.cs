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

            var claims = new List<ChallengeClaim>
                {
                    new() { Name = "DeleteCase" },
                };

                await _identityApplicationDbContext.ChallengeClaims.AddRangeAsync(claims);
                await _identityApplicationDbContext.SaveChangesAsync();
        }
    }
}
