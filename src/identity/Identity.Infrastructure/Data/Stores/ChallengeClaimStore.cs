using Challenge.Core.Helpers;
using Challenge.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores
{
    public class ChallengeClaimStore(IdentityApplicationDbContext dbContext, JwtChallengeHelper jwtChallengeHelper) : IChallengeClaimStore
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly JwtChallengeHelper _jwtChallengeHelper = jwtChallengeHelper;

        public async Task<bool> ClaimExists(string claimName)
        {
            return await _dbContext.ChallengeClaims.AnyAsync(c => c.Name == claimName);
        }

        public async Task<(bool succeeded, List<ErrorDetail> errors)> CreateClaim(ChallengeClaim claim)
        {
            List<ErrorDetail> errors = [];

            if (await ClaimExists(claim.Name))
            {
                errors.Add(new ErrorDetail { Field = "Challenge Claim", Reason = "Claim already exists a claim with the same name already exists" });
            }

            if (claim.Name.Length <= 4)
            {
                errors.Add(new ErrorDetail { Field = "Challenge Claim", Reason = "Claim name is to short" });
            }

            if(errors.Count != 0) return (false, errors);

            await _dbContext.ChallengeClaims.AddAsync(claim);
            await _dbContext.SaveChangesAsync();

            return (true, errors);
        }

        public async Task<(bool succeeded, List<ErrorDetail> errors, string jwtChallengeToken)> CreateToken(ChallengeToken token)
        {
            List<ErrorDetail> errors = [];

            var claimExists = await ClaimExists(token.ChallengeClaimName);
            if (!claimExists)
            {
                errors.Add(new ErrorDetail { Field = "Challenge Claim", Reason = "Claim dose not exist" });
            }

            if(errors.Count != 0) return (false, errors, "");

            var jwt = _jwtChallengeHelper.GenerateJwtToken(token);

            await _dbContext.ChallengeTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            return (true, [], jwt);
        }

        public async Task<(bool succeeded, List<ErrorDetail> errors)> DeleteClaim(string claimName)
        {
            List<ErrorDetail> errors = [];

            if (!await ClaimExists(claimName))
            {
                errors.Add(new ErrorDetail { Field = "Challenge Claim", Reason = "Claim dose not exist" });
            }

            if (errors.Count != 0) return (false, errors);

            var claim = await GetClaim(claimName);
            if (claim != null)
            {
                _dbContext.ChallengeClaims.Remove(claim);
                await _dbContext.SaveChangesAsync();
                return (true, errors);
            }
            else
            {
                return (false, errors);
            }
        }

        public async Task<ChallengeClaim?> GetClaim(string claimName)
        {
            return await _dbContext.ChallengeClaims.FirstOrDefaultAsync(c => c.Name == claimName);
        }

        public async Task<ICollection<ChallengeClaim>> GetClaims()
        {
            return await _dbContext.ChallengeClaims.ToListAsync();
        }
    }
}
