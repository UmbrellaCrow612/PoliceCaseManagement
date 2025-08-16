using Identity.Application.Codes;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IUserVerificationService"/> - test this, as well when using it else where only use the <see cref="IUserVerificationService"/>
    /// interface not this class
    /// </summary>
    public class UserVerificationServiceImpl(
        IdentityApplicationDbContext dbContext,
        ILogger<UserVerificationServiceImpl> logger,
        ICodeGenerator codeGenerator,
        IOptions<TimeWindows> timeWindows
        ) : IUserVerificationService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserVerificationServiceImpl> _logger = logger;
        private readonly ICodeGenerator _codeGenerator = codeGenerator;
        private readonly TimeWindows _timeWindows = timeWindows.Value;

        public async Task<UserVerificationResult> SendEmailVerification(ApplicationUser user)
        {
            var result = new UserVerificationResult();

            if (user.EmailConfirmed)
            {
                result.AddError(BusinessRuleCodes.EmailVerified, "Email already verified");
                return result;
            }

            var validVerificationExists = await _dbContext.EmailVerifications.AnyAsync(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow);
            if (validVerificationExists)
            {
                result.AddError(BusinessRuleCodes.EmailVerificationExists, "Valid attempt exists");
                return result;
            }

            var code = _codeGenerator.GenerateUnique();
            var emailVerification = new EmailVerification
            {
                Code = code,
                Email = user.Email!,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.EmailConfirmationTime),
                UserId = user.Id,
            };

            // send via email service a email with link domain/some?code=code so they click link and it extract it 
            // and automatically sends it as it will be a big code ID

            #if DEBUG
            _logger.LogInformation("Email verification sent for user: {userId} with code: {code}", user.Id, code);
            #endif

            await _dbContext.EmailVerifications.AddAsync(emailVerification);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public Task<UserVerificationResult> SendPhoneVerification(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<UserVerificationResult> VerifyEmail(ApplicationUser user, string code)
        {
            throw new NotImplementedException();
        }

        public Task<UserVerificationResult> VerifyPhoneNumber(ApplicationUser user, string code)
        {
            throw new NotImplementedException();
        }
    }
}
