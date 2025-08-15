namespace Identity.Application.Helpers
{
    public class QRCodeHandler
    {
        //public static byte[] GenerateTotpQRCodeBytes(string base32Secret, string userEmail, string issuer)
        //{
        //    string totpUri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(userEmail)}?secret={base32Secret}&issuer={Uri.EscapeDataString(issuer)}";

        //    using QRCodeGenerator qrGenerator = new();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
        //    using BitmapByteQRCode qrCode = new(qrCodeData);
        //    return qrCode.GetGraphic(5);
        //}
    }
}
