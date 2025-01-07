using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAGridParentQuestionService
    {
        public static (CAPTCHAGridParentQuestion question, ICollection<CAPTCHAGridChild> parentsChildren, ICollection<CAPTCHAGridChild> fullChildren) CreateQuestion()
        {
            var imgService = new CAPTCHAImgService();
            var rn = RandomStringService.GenerateRandomString(12); 

            var question = new CAPTCHAGridParentQuestion()
            {
                ValueInPlainText = rn[..2],
                ExpiresAt = DateTime.UtcNow.AddMinutes(2)
            };

            var allRandomStrings = rn.ToCharArray(); 

            List<CAPTCHAGridChild> parentsChildren = [];
            List<CAPTCHAGridChild> fullChildren = [];

            foreach (var component in allRandomStrings)
            {
                var child = new CAPTCHAGridChild
                {
                    CAPTCHAGridParentQuestionId = question.Id,
                    ValueInPlainText = component.ToString()
                };

                if (question.ValueInPlainText.Contains(child.ValueInPlainText))
                {
                    parentsChildren.Add(child);
                }

                fullChildren.Add(child);

                var bytes = imgService.CreateImg(question, child.ValueInPlainText);
                child.SetTempBytes(bytes);
            }

            return (question, parentsChildren, fullChildren);
        }


    }
}
