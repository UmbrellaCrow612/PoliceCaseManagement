using Challenge.Core.Models;
using Shared.DTOs;

namespace Identity.Infrastructure.Data.Stores.Interfaces
{
    public interface IChallengeClaimStore
    {
        public Task<(bool succeeded, List<ErrorDetail> errors)> CreateClaim(ChallengeClaim claim);

        public Task<(bool succeeded, List<ErrorDetail> errors)> DeleteClaim(string claimName);

        public Task<ChallengeClaim?> GetClaim(string claimName);

        public Task<bool> ClaimExists(string claimName);

        public Task<ICollection<ChallengeClaim>> GetClaims();

        public Task<(bool succeeded, List<ErrorDetail> errors, string jwtChallengeToken)> CreateToken(ChallengeToken token);
    }
}
