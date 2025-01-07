using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAGridParentQuestionService
    {
        public static (string questionText, CAPTCHAGridParentQuestion question, ICollection<CAPTCHAGridChild> answerChildren, ICollection<CAPTCHAGridChild> fullChildren) CreateQuestion()
        {
            var imgService = new CAPTCHAImgService();
            var randomString = RandomStringService.GenerateRandomString(12);

            var questionText = randomString[..2];

            var question = new CAPTCHAGridParentQuestion()
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(2)
            };

            var allRandomStrings = randomString.ToCharArray(); 

            List<CAPTCHAGridChild> answerChildren = [];
            List<CAPTCHAGridChild> fullChildren = [];

            foreach (var component in allRandomStrings)
            {
                var child = new CAPTCHAGridChild
                {
                    CAPTCHAGridParentQuestionId = question.Id,
                };

                if (questionText.Contains(component))
                {
                    answerChildren.Add(child);
                }

                fullChildren.Add(child);

                var bytes = imgService.CreateImg(question, component.ToString());
                child.SetTempBytes(bytes);
            }

            fullChildren = [.. fullChildren.OrderBy(x => Guid.NewGuid())]; // change order

            return (questionText, question, answerChildren, fullChildren);
        }


    }
}
