using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Encryption
{
    internal interface ICAPTCHAHashService
    {
        byte[] Hash(byte[] data);

        byte[] Hash(CAPTCHAMathQuestion question);

        bool Compare();
    }
}
