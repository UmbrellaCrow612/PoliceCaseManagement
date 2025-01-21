using QRCoder;

namespace Identity.API.Helpers
{
    public class QRCodeHandler
    {
        public static byte[] GenerateQRCodeBytes(string content)
        {
            using QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new(qrCodeData);
            return qrCode.GetGraphic(5);
        }
    }
}
