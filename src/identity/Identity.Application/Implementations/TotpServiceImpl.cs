using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using QRCoder;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="ITotpService"/> - test this, as well when using it else where only use the <see cref="ITotpService"/>
    /// interface not this class
    /// </summary>
    public class TotpServiceImpl(
        ICodeGenerator codeGenerator,
        IdentityApplicationDbContext dbContext
        ) : ITotpService
    {
        private readonly ICodeGenerator _codeGenerator = codeGenerator;
        private readonly IdentityApplicationDbContext _dbContext = dbContext;

        public byte[] GenerateTotpQrCode(ApplicationUser user, string base32Secret, string issuer)
        {
            const string algorithm = "SHA1";
            const int digits = 6;
            const int period = 30;

            string encodedIssuer = Uri.EscapeDataString(issuer);
            string encodedUsername = Uri.EscapeDataString(user.UserName!);

            string formattedUri =
                $"otpauth://totp/{encodedIssuer}:{encodedUsername}?secret={base32Secret}&issuer={encodedIssuer}&algorithm={algorithm}&digits={digits}&period={period}";

            var qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(formattedUri, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrCodeData);

            const int PixelsPerModule = 20;
            byte[] qrCodeImageBytes = qrCode.GetGraphic(PixelsPerModule);

            return qrCodeImageBytes;
        }

        public async Task<TotpResult> GenerateTotp(ApplicationUser user)
        {
            var result = new TotpResult();

            if (user.TotpConfirmed || !string.IsNullOrWhiteSpace(user.TotpSecret))
            {
                result.AddError(BusinessRuleCodes.TOTPExists, "TOTP already set up for the user, if you wish to change reset and try again");
                return result;
            }

            var secret = _codeGenerator.GenerateBase32Secret();
            user.TotpSecret = secret;

            var qrCodeBytes = GenerateTotpQrCode(user, secret, "PCMS"); // PCMS is name shown on app for TOTP code
            result.QrCodeBytes = qrCodeBytes;

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<IResult> ResetTotp(ApplicationUser user)
        {
            var result = new Result();

            user.ResetTotp();

            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
