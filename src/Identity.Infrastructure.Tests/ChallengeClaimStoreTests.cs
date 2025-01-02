using Challenge.Core.Helpers;
using Challenge.Core.Models;
using Identity.Infrastructure.Data.Stores.Interfaces;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Identity.Infrastructure.Data.Stores;
using Challenge.Core.Settings;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Tests
{
    public class ChallengeClaimStoreTests
    {
        private static IdentityApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<IdentityApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IdentityApplicationDbContext(options);
        }

        private static ChallengeClaimStore GetChallengeClaimStore(IdentityApplicationDbContext dbContext)
        {
            ChallengeJwtSettings settings = new()
            {
                Audiences = ["Audience1", "Audience2"],
                ExpiresInMinutes = 60,
                Issuer = "Issuer",
                Key = "OIEBNFOERVONEROVJIEOJPERWJMVREUOBEMNVP3NIPV3PVNCOK4IOTIPCWPFC"
            };

            IOptions<ChallengeJwtSettings> options = Options.Create(settings);

            JwtChallengeHelper jwtChallengeHelper = new(options);
            return new ChallengeClaimStore(dbContext, jwtChallengeHelper);
        }

        [Fact]
        public async Task CreateClaim_ShouldAddClaim_WhenValidClaimProvided()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var newClaim = new ChallengeClaim { Name = "ValidClaimName" };

            // Act
            var (succeeded, errors) = await store.CreateClaim(newClaim);

            // Assert
            Assert.True(succeeded);
            Assert.Empty(errors);
            Assert.Single(dbContext.ChallengeClaims);
        }

        [Fact]
        public async Task CreateClaim_ShouldReturnError_WhenClaimNameTooShort()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var newClaim = new ChallengeClaim { Name = "abc" };

            // Act
            var (succeeded, errors) = await store.CreateClaim(newClaim);

            // Assert
            Assert.False(succeeded);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Reason.Contains("Claim name is to short"));
        }

        [Fact]
        public async Task CreateClaim_ShouldReturnError_WhenClaimAlreadyExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var existingClaim = new ChallengeClaim { Name = "ExistingClaim" };
            dbContext.ChallengeClaims.Add(existingClaim);
            await dbContext.SaveChangesAsync();

            var newClaim = new ChallengeClaim { Name = "ExistingClaim" };

            // Act
            var (succeeded, errors) = await store.CreateClaim(newClaim);

            // Assert
            Assert.False(succeeded);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Reason.Contains("Claim already exists"));
        }

        [Fact]
        public async Task DeleteClaim_ShouldRemoveClaim_WhenClaimExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var claim = new ChallengeClaim { Name = "ClaimToDelete" };
            dbContext.ChallengeClaims.Add(claim);
            await dbContext.SaveChangesAsync();

            // Act
            var (succeeded, errors) = await store.DeleteClaim("ClaimToDelete");

            // Assert
            Assert.True(succeeded);
            Assert.Empty(errors);
            Assert.Empty(dbContext.ChallengeClaims);
        }

        [Fact]
        public async Task DeleteClaim_ShouldReturnError_WhenClaimDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            // Act
            var (succeeded, errors) = await store.DeleteClaim("NonExistentClaim");

            // Assert
            Assert.False(succeeded);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Reason.Contains("Claim dose not exist"));
        }

        [Fact]
        public async Task CreateToken_ShouldGenerateJwt_WhenValidTokenProvided()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var claim = new ChallengeClaim { Name = "ValidClaim" };
            dbContext.ChallengeClaims.Add(claim);
            await dbContext.SaveChangesAsync();

            var token = new ChallengeToken { ChallengeClaimName = "ValidClaim", ChallengeClaimId = "123", ExpiresAt = DateTime.UtcNow.AddDays(1), Id = "321", UserId= "456" };

            // Act
            var (succeeded, errors, jwtChallengeToken) = await store.CreateToken(token);

            // Assert
            Assert.True(succeeded);
            Assert.Empty(errors);
            Assert.NotEmpty(jwtChallengeToken);
        }

        [Fact]
        public async Task CreateToken_ShouldReturnError_WhenClaimDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var store = GetChallengeClaimStore(dbContext);

            var token = new ChallengeToken { ChallengeClaimName = "non", ChallengeClaimId = "123", ExpiresAt = DateTime.UtcNow.AddDays(1), Id = "321", UserId = "456" };

            // Act
            var (succeeded, errors, jwtChallengeToken) = await store.CreateToken(token);

            // Assert
            Assert.False(succeeded);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Reason.Contains("Claim dose not exist"));
        }
    }

}
