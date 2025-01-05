using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    /// <summary>
    /// To create Images for CAPTCHAs
    /// </summary>
    public interface ICAPTCHAImgService
    {
        /// <summary>
        /// Create a byte array img for a <see cref="CAPTCHAMathQuestion"/>
        /// </summary>
        /// <param name="question">The captcha question</param>
        /// <param name="expression">The expression to be served to users you want to convert to a img.</param>
        /// <returns>Byte array of the img.</returns>
        byte[] CreateImg(CAPTCHAMathQuestion question, string expression);
    }
}
