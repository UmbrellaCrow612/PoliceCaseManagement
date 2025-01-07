using CAPTCHA.Core.Banks;
using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAAudioQuestionService
    {
        public static (byte[] audioInBytes, CAPTCHAAudioQuestion question) CreateQuestion()
        {
            var sentence = string.Join(" ", WordBank.GetRandomWords(2));

            var bytes = AudioService.ConvertSentenceToAudioBytes(sentence);

            var question = new CAPTCHAAudioQuestion()
            {
                AnswerInPlainText = sentence,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
            };

            return (bytes, question);
        }
    }
}
