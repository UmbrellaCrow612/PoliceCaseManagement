using Email.Events.V1;
using Identity.Application.Codes;
using Identity.Application.Settings;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OtpNet;
using Results.Abstractions;

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
        IOptions<TimeWindows> timeWindows,
        IPublishEndpoint publishEndpoint
        ) : IUserVerificationService
    {
        private readonly IdentityApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserVerificationServiceImpl> _logger = logger;
        private readonly ICodeGenerator _codeGenerator = codeGenerator;
        private readonly TimeWindows _timeWindows = timeWindows.Value;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task<IResult> SendEmailVerification(ApplicationUser user)
        {
            var result = new Result();

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

            var email = new EmailRequestEvent
            {
                To = user.Email!,
                Subject = "Email ver",
                Body = $"code: /domain/url/somthing?code={emailVerification.Code}",
                Format = EmailBodyFormat.Html
            };
            await _publishEndpoint.Publish(email);

            #if DEBUG
            _logger.LogInformation("Email verification sent for user: {userId} with code: {code}", user.Id, code);
            #endif

            await _dbContext.EmailVerifications.AddAsync(emailVerification);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> SendPhoneVerification(ApplicationUser user)
        {
            var result = new Result();

            if(user.PhoneNumberConfirmed)
            {
                result.AddError(BusinessRuleCodes.PhoneNumberVerified, "Phone number verified");
                return result;
            }

            var validPhoneVerificationExists = await _dbContext.PhoneVerifications.AnyAsync(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow);
            if (validPhoneVerificationExists)
            {
                result.AddError(BusinessRuleCodes.PhoneNumberVerificationExists, "Valid attempt exists");
                return result;
            }

            var code = _codeGenerator.GenerateSixDigitCode();
            var phoneVerification = new PhoneVerification
            {
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_timeWindows.PhoneConfirmationTime),
                PhoneNumber = user.PhoneNumber!,
                UserId = user.Id,
            };

            // send via sms code 
            #if DEBUG
            _logger.LogInformation("Phone verification sent for user: {userId} with code: {code}", user.Id, code);
            #endif

            await _dbContext.PhoneVerifications.AddAsync(phoneVerification);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> VerifyEmail(ApplicationUser user, string code)
        {
            var result = new Result();

            if (user.EmailConfirmed)
            {
                result.AddError(BusinessRuleCodes.EmailVerified, "Email already verified");
                return result;
            }

            var emailVerification = await _dbContext
                .EmailVerifications
                .Where(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow && x.Code == code)
                .FirstOrDefaultAsync();

            if (emailVerification is null || !emailVerification.IsValid())
            {
                result.AddError(BusinessRuleCodes.EmailVerificationInvalid, "Incorrect code");
                return result;
            }

            emailVerification.MarkUsed();
            user.MarkEmailConfirmed();

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> VerifyPhoneNumber(ApplicationUser user, string code)
        {
            var result = new Result();

            if (user.PhoneNumberConfirmed)
            {
                result.AddError(BusinessRuleCodes.PhoneNumberVerified, "Phone number already verified");
                return result;
            }

            var phoneConfirmation = await _dbContext
                .PhoneVerifications
                .Where(x => x.UserId == user.Id && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow && x.Code == code)
                .FirstOrDefaultAsync();

            if (phoneConfirmation is null || !phoneConfirmation.IsValid())
            {
                result.AddError(BusinessRuleCodes.PhoneNumberVerificationInvalid, "Incorrect code");
                return result;
            }

            user.MarkPhoneNumberConfirmed();
            phoneConfirmation.MarkUsed();


            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> VerifyTotp(ApplicationUser user, string code)
        {
            var result = new Result();

            if (user.TotpConfirmed || string.IsNullOrWhiteSpace(user.TotpSecret))
            {
                result.AddError(BusinessRuleCodes.TOTPExists, "TOTP already verified or invalid state reset");
                return result;
            }

            var totp = new Totp(Base32Encoding.ToBytes(user.TotpSecret));
            string computedTotpCode = totp.ComputeTotp();

            if (code != computedTotpCode)
            {
                result.AddError(BusinessRuleCodes.TOTPReset, "TOTP code invalid");
                return result;
            }

            user.MarkTotpConfirmed();

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
