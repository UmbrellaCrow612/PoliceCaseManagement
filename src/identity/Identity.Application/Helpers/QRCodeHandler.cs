using System.Text.Json;
using Identity.Core.Models;
using QRCoder;

namespace Identity.Application.Helpers
{
    public class QRCodeHandler
    {
        public static byte[] GenerateQRCodeBytes(string base32Secret, string userEmail, string issuer)
        {
            string totpUri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(userEmail)}?secret={base32Secret}&issuer={Uri.EscapeDataString(issuer)}";

            using QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new(qrCodeData);
            return qrCode.GetGraphic(5);
        }

        public static byte[] GenerateOTPQRCodeBytes(OTPAttempt otp)
        {
            string json = JsonSerializer.Serialize(new
            {
                otp.Id,
                otp.CreatedAt,
                otp.ExpiresAt,
                otp.Code,
            });

            using QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new(qrCodeData);
            return qrCode.GetGraphic(5);
        }
    }
}
